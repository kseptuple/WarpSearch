using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using WarpSearch.Common;

namespace WarpSearch.Games
{
    public class GbaCv : IDisposable
    {
        protected FormMain mainForm = null;

        protected byte[] data = null;
        public Dictionary<Point, RoomStruct> RoomsAtPositions { get; set; } = new Dictionary<Point, RoomStruct>();
        public Dictionary<uint, RoomStruct> RoomStructs { get; set; } = new Dictionary<uint, RoomStruct>();

        public Dictionary<uint, List<MapSquareInfo>> FlagRoomLists { get; set; } = new Dictionary<uint, List<MapSquareInfo>>();
        protected List<List<RomPointer>> exitGroups { get; set; } = null;
        //protected ROMPointer RoomRootPointer { get; set; } = 0;
        protected RomPointer MapPointer { get; set; } = 0;
        protected RomPointer MapLinePointer { get; set; } = 0;
        protected RomPointer FirstRoomPointer { get; set; } = 0;
        public RomPointer GetMapPointer() => MapPointer;
        public RomPointer GetMapLinePointer() => MapLinePointer;
        public RomPointer GetFirstRoomPointer() => FirstRoomPointer;
        public byte[] GetData() => data;

        protected uint RoomFlagStart { get; set; } = 0;

        protected Dictionary<uint, List<uint>> PointerAddresses { get; set; } = new Dictionary<uint, List<uint>>();

        public GameTypeEnum GameType { get; set; } = GameTypeEnum.Null;
        public GameVersionEnum GameVersion { get; set; }

        public int MapWidth { get; set; } = 0;
        public int MapHeight { get; set; } = 0;

        protected RomPointer maxExitAddress { get; set; } = 0x8_00_00_00;
        protected RomPointer minExitAddress { get; set; } = 0xA_00_00_00;

        protected int exitLength { get; set; } = 0;
        protected int fileSize { get; set; } = 0;

        protected List<SpecialRoomData> specialRoomDataList { get; set; } = null;

        protected List<Point> mapPositionsToDraw { get; set; } = new List<Point>();

        protected bool useHackSupport = false;

        public string FileName { get; set; }
        public bool IsCustom { get; set; } = false;

        public MapElementsToDraw MapElements { get; set; } = new MapElementsToDraw();

        public virtual bool UseHackSupport
        {
            get
            {
                return useHackSupport;
            }
            set
            {
                useHackSupport = value;
            }
        }

        public GbaCv(byte[] fileData, FormMain formMain)
        {
            mainForm = formMain;
            data = fileData;
            fileSize = data.Length;
            MapElements.Rooms = new List<MapRoomToDraw>();
            MapElements.Lines = new List<MapLineToDraw>();
            MapElements.Texts = new List<MapTextToDraw>();
        }

        //所有可能是指针的数据
        protected void InitPointerAddress()
        {
            PointerAddresses.Clear();
            for (uint i = 0; i < data.Length; i++)
            {
                uint pointer = getUint(i);
                if (pointer >= 0x8_00_00_00 && pointer <= 0x8_00_00_00 + data.Length)
                {
                    if (!PointerAddresses.ContainsKey(pointer))
                    {
                        PointerAddresses.Add(pointer, new List<uint>());
                    }
                    PointerAddresses[pointer].Add(i + 0x8_00_00_00);
                }
            }
        }

        public virtual void LoadRooms()
        {
            throw new NotImplementedException();
        }

        //public virtual void DrawRooms()
        //{
        //    LoadRooms(false);
        //}

        protected void AddLine(int x, int y, int type, bool isHorizonal)
        {
            switch (type)
            {
                case 1:
                case 2:
                    MapElements.Lines.Add(new MapLineToDraw { X = x, Y = y, IsHorizonal = isHorizonal, IsSolid = false });
                    break;
                case 3:
                    MapElements.Lines.Add(new MapLineToDraw { X = x, Y = y, IsHorizonal = isHorizonal, IsSolid = true });
                    break;
                default:
                    break;
            }
        }

        //画地图房间
        protected void DrawLinesUitls(int x, int y, int topType, int bottomType, int leftType, int rightType)
        {
            AddLine(x, y, topType, true);
            AddLine(x, y, leftType, false);
            AddLine(x, y + 1, bottomType, true);
            AddLine(x + 1, y, rightType, false);
        }

        public void Dispose()
        {
            data = null;
            RoomsAtPositions = null;
        }

        public virtual void FindWarpDestination(bool previewOnly = false)
        {
            //previewOnly 仅预览，移动鼠标时显示当前位置能否出城
            mainForm.ClearPos(previewOnly);
            if (!previewOnly)
            {
                mainForm.ClearLine();
            }
            bool isUncertain = false;
            var roomInfo = mainForm.selectedRoom;
            var sourcePos = mainForm.selectedPos;
            if (roomInfo == null || roomInfo.BelongingRoom == null) return;

            var currentPointer = roomInfo.BelongingRoom.ExitPointer;
            var sourceXToRoom = sourcePos.X - roomInfo.BelongingRoom.Left;

            //晓月一格宽的房间右侧，超过一格的视为一格
            if (GameType == GameTypeEnum.Aos)
            {
                if (roomInfo.BelongingRoom.Width == 1 && sourceXToRoom > 1) sourceXToRoom = 1;
            }
            var sourceYToRoom = sourcePos.Y - roomInfo.BelongingRoom.Top;
            List<uint> destAddressList = new List<uint>();
            uint lastExitPossibleAddress = (uint)(data.Length - exitLength);
            while (currentPointer.RomOffset <= lastExitPossibleAddress)
            {
                //出口的位置对应上了
                if (sourceXToRoom == (sbyte)getByte(currentPointer, 4) && sourceYToRoom == (sbyte)getByte(currentPointer, 5))
                {
                    var destPointer = getRomPointer(currentPointer, 0);
                    Dictionary<uint, byte> flagList = new Dictionary<uint, byte>();
                    ushort flag = 0;
                    //防止后续读取时越界
                    int minimumSpace = GameType == GameTypeEnum.Aos ? 13 : 11;
                    bool isFound = false;
                    while (destPointer >= 0x8_00_00_00 && destPointer < 0x8_00_00_00 + data.Length - minimumSpace)
                    {
                        if (!destAddressList.Contains(destPointer))
                        {
                            destAddressList.Add(destPointer);
                        }
                        else
                        {
                            //出现循环
                            break;
                        }
                        flag = getUShort(destPointer, 2);
                        if (flag == 0xFFFF)
                        {
                            //不存在下一个房间
                            if (RoomStructs.ContainsKey(destPointer))
                            {
                                isFound = true;
                            }
                            break;
                        }
                        else
                        {
                            if (GameType == GameTypeEnum.Hod)
                            {
                                if (RoomStructs.ContainsKey(destPointer))
                                {
                                    isFound = true;
                                    break;
                                }
                                else
                                {
                                    isUncertain = true;
                                    uint address = getFlagAddress(flag, out int bit);
                                    if (!flagList.ContainsKey(address))
                                    {
                                        flagList.Add(address, 0);
                                    }
                                    flagList[address] |= (byte)(1 << bit);
                                }
                            }

                            destPointer = getUnalignedRomPointer(destPointer, 4);
                        }
                    }

                    if (isFound)
                    {
                        var newIsUncertain = drawGoodRoom(destPointer, isUncertain, flagList);
                        mainForm.AddRoomPos(sourcePos.X, sourcePos.Y, false, previewOnly, newIsUncertain);
                    }
                    else
                    {
                        mainForm.AddRoomPos(sourcePos.X, sourcePos.Y, true, previewOnly);
                        drawBadRoom();
                    }
                    return;
                }
                currentPointer = currentPointer + (uint)exitLength;
            }
            mainForm.AddRoomPos(sourcePos.X, sourcePos.Y, true, previewOnly);
            drawBadRoom();
            return;

            bool drawGoodRoom(RomPointer pointer, bool _isUncertain, Dictionary<uint, byte> flagList)
            {
                //获取目标房间的位置坐标
                var (exitX, exitY) = getExitDestPos();
                bool isOutsideDest = false;
                if (exitX < 0 || exitX > RoomStructs[pointer].Width - 1 || exitY < 0 || exitY > RoomStructs[pointer].Height - 1)
                {
                    isOutsideDest = true;
                    _isUncertain = true;
                }
                if (!previewOnly)
                {
                    mainForm.sourceRoom = roomInfo.BelongingRoom;
                    mainForm.destRoom = RoomStructs[pointer];
                    var destX = RoomStructs[pointer].Left + exitX;
                    var destY = RoomStructs[pointer].Top + exitY;
                    mainForm.AddRoomPos(destX, destY, false, false, _isUncertain);
                    mainForm.SetFlagListForDestSearch(flagList);
                    //画线
                    if (sourcePos.X != destX || sourcePos.Y != destY)
                    {
                        mainForm.AddLine(sourcePos.X, sourcePos.Y, destX, destY, true);
                    }
                    if (isOutsideDest)
                    {
                        mainForm.AddLine(RoomStructs[pointer].Left, RoomStructs[pointer].Top, destX, destY, false);
                    }
                }
                return _isUncertain;
            }

            (int, int) getExitDestPos()
            {
                if (GameType == GameTypeEnum.Aos)
                {
                    return ((sbyte)getByte(currentPointer, 11), (sbyte)getByte(currentPointer, 13));
                }
                else if (GameType == GameTypeEnum.Hod)
                {
                    var tmpX = 0;
                    var XOffset = getByte(currentPointer, 8);
                    if (XOffset > 0x80) tmpX = 1;
                    return ((sbyte)getByte(currentPointer, 9) + tmpX, (sbyte)getByte(currentPointer, 11));
                }
                return (0, 0);
            }

            void drawBadRoom()
            {
                if (!previewOnly)
                {
                    mainForm.sourceRoom = null;
                    mainForm.destRoom = null;
                }
            }
        }

        public virtual void FindWarpSource(int searchLevel)
        {
            mainForm.ClearPos();
            mainForm.ClearLine();
            mainForm.ClearSourceRoomList();
            var roomInfo = mainForm.selectedRoom;
            if (roomInfo == null || roomInfo.BelongingRoom == null) return;
            //找到通向当前房间的所有出口
            var exits = FindNormalExits(roomInfo.BelongingRoom.RoomPointer);
            List<uint> addresses = new List<uint>();
            foreach (var exit in exits)
            {
                addSourceRoom(exit, false);
            }

            getAllPossibleExits(roomInfo.BelongingRoom.RoomPointer, new Dictionary<uint, byte>(), false);

            void getAllPossibleExits(uint address, Dictionary<uint, byte> flagList, bool isUncertain)
            {
                //已经检查过这个指针了，防止循环
                if (addresses.Contains(address))
                {
                    return;
                }
                addresses.Add(address);

                if (PointerAddresses.ContainsKey(address))
                {
                    List<uint> addressList = PointerAddresses[address];
                    foreach (uint currentPointerAddress in addressList)
                    {
                        var currentPointerActualAddress = currentPointerAddress - 0x8_00_00_00;

                        //可能性1：这个指针本身是个出口
                        if (currentPointerAddress > minExitAddress)
                        {
                            if (!exits.Exists(e => e.ExitPointer == currentPointerAddress))
                            {
                                addSourceRoom(CreateExitInfo(currentPointerAddress), isUncertain, flagList);
                            }
                        }

                        //可能性2：是其他地方跳转过来的
                        ushort flag = getUShort(currentPointerActualAddress, -2);
                        //flag == -1时没有机会跳到下一个房间了
                        if (flag == 0xFFFF)
                        {
                            continue;
                        }

                        //复制一个新的flagList
                        Dictionary<uint, byte> newFlagList = new Dictionary<uint, byte>();
                        foreach (var flagItem in flagList)
                        {
                            newFlagList.Add(flagItem.Key, flagItem.Value);
                        }
                        if (GameType == GameTypeEnum.Hod)
                        {
                            isUncertain = true;
                            int bit = 0;
                            uint flagAddress = getFlagAddress(flag, out bit);
                            if (!newFlagList.ContainsKey(flagAddress))
                            {
                                newFlagList.Add(flagAddress, 0);
                            }
                            newFlagList[flagAddress] |= (byte)(1 << bit);
                        }

                        getAllPossibleExits(currentPointerAddress - 4, newFlagList, isUncertain);
                    }
                }
            }

            void addSourceRoom(ExitInfo exitInfo, bool isUncertain, Dictionary<uint, byte> flagList = null)
            {
                var exitGroupId = 0;
                if (exitInfo.DestX < 0 || exitInfo.DestX > roomInfo.BelongingRoom.Width - 1 || exitInfo.DestY < 0 || exitInfo.DestY > roomInfo.BelongingRoom.Height - 1)
                {
                    isUncertain = true;
                }
                //出口分组
                if (GameType == GameTypeEnum.Aos)
                {
                    exitGroupId = (int)((exitInfo.ExitPointer.RomOffset >> 2) & 3);
                }
                else if (GameType == GameTypeEnum.Hod)
                {
                    exitGroupId = (int)((exitInfo.ExitPointer.RomOffset >> 2) % 3);
                }
                //出口的位置等于目标出口的位置
                var sourceXToRoom = exitInfo.SourceX;
                var sourceYToRoom = exitInfo.SourceY;
                foreach (var pointer in exitGroups[exitGroupId])
                {
                    var room = RoomStructs[pointer];
                    //晓月不允许一格宽房间向右超过一格
                    if (GameType == GameTypeEnum.Aos && room.Width == 1 && sourceXToRoom > 1)
                        continue;
                    if (searchLevel < 4)
                    {
                        //不允许远距离换版
                        if (GameType == GameTypeEnum.Hod)
                        {
                            //白夜一格宽房间
                            if (room.Width == 1)
                            {
                                if (sourceXToRoom > 2)
                                {
                                    continue;
                                }
                                else if (sourceXToRoom == 2)
                                {
                                    //远距离换版
                                    if (sourceYToRoom <= -1 || sourceYToRoom >= room.Height)
                                        continue;
                                    //不要红门传送
                                    if (searchLevel == 0)
                                        continue;
                                    //红门传送，判断红门的位置
                                    if (searchLevel < 3 && (room.GateHeight == null || !room.GateHeight.Contains(sourceYToRoom)))
                                        continue;
                                }
                            }
                        }
                        //超过一格，或者不是白夜的一格宽房间向右两格
                        if (sourceXToRoom < -1 || sourceYToRoom < -1 || (sourceXToRoom > room.Width && (room.Width > 1 || GameType == GameTypeEnum.Aos)) || sourceYToRoom > room.Height)
                        {
                            continue;
                        }
                        //不要斜角换版
                        if (searchLevel < 2)
                        {
                            if ((sourceXToRoom == -1 && sourceYToRoom == -1)
                                || (sourceXToRoom == -1 && sourceYToRoom == room.Height)
                                || (sourceXToRoom == room.Width && sourceYToRoom == -1)
                                || (sourceXToRoom == room.Width && sourceYToRoom == room.Height))
                            {
                                continue;
                            }
                        }
                    }

                    var currentPointer = room.ExitPointer;
                    int num = 0;
                    while (currentPointer <= exitInfo.ExitPointer)
                    {
                        if (searchLevel < 5)
                        {
                            //不要房间内换版，先判断是不是正常出口
                            if (num >= room.Exits.Count)
                            {
                                if (sourceXToRoom >= 0 && sourceYToRoom >= 0 && sourceXToRoom < room.Width && sourceYToRoom < room.Height)
                                {
                                    break;
                                }
                            }
                        }
                        //找到了目标出口
                        if (currentPointer == exitInfo.ExitPointer)
                        {
                            mainForm.AddSourceRoom(RoomStructs[room.RoomPointer], exitInfo, isUncertain, flagList);
                            break;
                        }
                        //找到了该出口位置对应的出口
                        if (sourceXToRoom == (sbyte)getByte(currentPointer, 4) && sourceYToRoom == (sbyte)getByte(currentPointer, 5))
                        {
                            //如果这是一个通往目标房间的出口，那么它应当能在其他路径中搜索到
                            //所以不需要重复处理
                            break;
                        }
                        currentPointer = currentPointer + (uint)exitLength;
                        num++;
                    }
                }
            }
        }
        protected List<ExitInfo> FindNormalExits(RomPointer roomPointer)
        {
            var result = new List<ExitInfo>();
            foreach (var rs in RoomStructs.Values)
            {
                foreach (var exit in rs.Exits)
                {
                    if (exit.ExitDestination == roomPointer)
                    {
                        result.Add(exit);
                    }
                }
            }
            return result;
        }

        protected byte getByte(RomPointer start, int offset)
        {
            return data[start.RomOffset + offset];
        }

        protected uint getUint(uint start)
        {
            //GBA读取未对齐的4字节数据
            switch (start % 4)
            {
                case 0:
                    return (uint)(data[start] | (data[start + 1] << 8) | (data[start + 2] << 16) | (data[start + 3] << 24));
                case 1:
                    return (uint)(data[start] | (data[start + 1] << 8) | (data[start + 2] << 16) | (data[start - 1] << 24));
                case 2:
                    return (uint)(data[start] | (data[start + 1] << 8) | (data[start - 2] << 16) | (data[start - 1] << 24));
                case 3:
                    return (uint)(data[start] | (data[start - 3] << 8) | (data[start - 2] << 16) | (data[start - 1] << 24));
            }
            return 0;
        }

        protected ushort getUShort(RomPointer start, int offset)
        {
            uint actualAddress = (uint)(start.RomOffset + offset);
            return (ushort)(data[actualAddress] | (data[actualAddress + 1] << 8));
        }

        protected uint getUnalignedUShort(RomPointer start, int offset)
        {
            uint actualAddress = (uint)(start.RomOffset + offset);
            return (uint)(data[actualAddress] | (data[actualAddress - 1] << 24));
        }

        public RomPointer getRomPointer(RomPointer start, int offset)
        {
            uint actualAddress = (uint)(start.RomOffset + offset);
            if (actualAddress >= data.Length) return null;
            return data[actualAddress] | (data[actualAddress + 1] << 8) | (data[actualAddress + 2] << 16) | (data[actualAddress + 3] << 24);
        }

        public RomPointer getUnalignedRomPointer(RomPointer start, int offset)
        {
            uint actualAddress = (uint)(start.RomOffset + offset);
            if (actualAddress >= data.Length) return null;
            return getUint(actualAddress);
        }

        protected virtual ExitInfo CreateExitInfo(RomPointer pointer)
        {
            throw new NotImplementedException();
        }

        protected uint getFlagAddress(ushort flagId, out int bit)
        {
            bit = flagId % 8;
            return (uint)(RoomFlagStart + flagId / 8);
        }
    }
}
