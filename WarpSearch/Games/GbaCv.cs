using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace WarpSearch.Games
{
    public class GbaCv : IDisposable
    {
        protected FormMain mainForm = null;

        protected byte[] data = null;
        public Dictionary<Point, RoomInfo> RoomsAtPositions { get; set; } = new Dictionary<Point, RoomInfo>();
        public Dictionary<uint, RoomStruct> RoomStructs { get; set; } = new Dictionary<uint, RoomStruct>();

        public Dictionary<uint, List<RoomInfo>> FlagRoomLists { get; set; } = new Dictionary<uint, List<RoomInfo>>();
        protected List<List<ROMPointer>> exitGroups { get; set; } = null;
        protected ROMPointer RoomRootPointer { get; set; } = 0;
        protected ROMPointer MapPointer { get; set; } = 0;
        protected ROMPointer MapLinePointer { get; set; } = 0;
        protected ROMPointer FirstRoomPointer { get; set; } = 0;
        protected uint RoomFlagStart { get; set; } = 0;

        protected Dictionary<uint, List<uint>> PointerAddresses { get; set; } = new Dictionary<uint, List<uint>>();
        protected bool PointerAddressInitFinished { get; set; }

        public GameTypeEnum GameType { get; set; } = GameTypeEnum.Null;

        public int MapWidth { get; set; } = 0;
        public int MapHeight { get; set; } = 0;

        protected ROMPointer maxExitAddress { get; set; } = 0x8_00_00_00;
        protected ROMPointer minExitAddress { get; set; } = 0xA_00_00_00;

        protected int exitLength { get; set; } = 0;
        protected int fileSize { get; set; } = 0;

        protected List<SpecialRoomData> specialRoomDataList { get; set; } = null;

        protected List<Point> mapPositionList { get; set; } = new List<Point>();

        protected bool useHackSupport = false;
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
            PointerAddressInitFinished = false;
        }

        //所有可能是指针的数据
        private void initPointerAddress()
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
            PointerAddressInitFinished = true;
        }

        public virtual void LoadRooms(bool load = true)
        {
            throw new NotImplementedException();
        }

        public virtual void DrawRooms()
        {
            LoadRooms(false);
        }

        //画地图房间
        protected void DrawLinesUitls(int x, int y, int topType, int bottomType, int leftType, int rightType)
        {
            switch (topType)
            {
                case 1:
                case 2:
                    mainForm.DrawLine(x, y, true, false);
                    break;
                case 3:
                    mainForm.DrawLine(x, y, true, true);
                    break;
                default:
                    break;
            }
            switch (leftType)
            {
                case 1:
                case 2:
                    mainForm.DrawLine(x, y, false, false);
                    break;
                case 3:
                    mainForm.DrawLine(x, y, false, true);
                    break;
                default:
                    break;
            }
            switch (bottomType)
            {
                case 1:
                case 2:
                    mainForm.DrawLine(x, y + 1, true, false);
                    break;
                case 3:
                    mainForm.DrawLine(x, y + 1, true, true);
                    break;
                default:
                    break;
            }
            switch (rightType)
            {
                case 1:
                case 2:
                    mainForm.DrawLine(x + 1, y, false, false);
                    break;
                case 3:
                    mainForm.DrawLine(x + 1, y, false, true);
                    break;
                default:
                    break;
            }
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
            var roomInfo = mainForm.selectedRoom;
            var sourcePos = mainForm.selectedPos;
            if (roomInfo == null || roomInfo.Room == null) return;
            var currentPointer = roomInfo.Room.ExitPointer;
            var sourceXToRoom = sourcePos.X - roomInfo.Room.Left;

            //晓月一格宽的房间右侧，超过一格的视为一格
            if (GameType == GameTypeEnum.Aos)
            {
                if (roomInfo.Room.Width == 1 && sourceXToRoom > 1) sourceXToRoom = 1;
            }
            var sourceYToRoom = sourcePos.Y - roomInfo.Room.Top;
            while (currentPointer <= maxExitAddress)
            {
                //出口的位置对应上了
                if (sourceXToRoom == (sbyte)(data[currentPointer + 4]) && sourceYToRoom == (sbyte)(data[currentPointer + 5]))
                {
                    var destPointer = getROMPointer(currentPointer);
                    if (!RoomStructs.ContainsKey(destPointer.Address))
                    {
                        Dictionary<uint, byte> flagList = new Dictionary<uint, byte>();
                        ushort flag = 0;
                        //防止后续读取时越界
                        int minimumSpace = GameType == GameTypeEnum.Aos ? 13 : 11;
                        while (destPointer.Address >= 0x8_00_00_00 && destPointer.Address < 0x8_00_00_00 + data.Length - minimumSpace)
                        {
                            flag = getUShort((uint)(destPointer + 2));
                            if (flag == 0xFFFF)
                            {
                                if (!RoomStructs.ContainsKey(destPointer.Address))
                                {
                                    mainForm.AddRoomPos(sourcePos.X, sourcePos.Y, true, previewOnly);
                                    drawBadRoom();
                                }
                                else
                                {
                                    mainForm.AddRoomPos(sourcePos.X, sourcePos.Y, false, previewOnly, true);
                                    drawGoodRoom(destPointer, true, flagList);
                                }
                                return;
                            }
                            else
                            {
                                if (GameType == GameTypeEnum.Hod)
                                {
                                    int bit = 0;
                                    uint address = getFlagAddress(flag, out bit);
                                    if (!flagList.ContainsKey(address))
                                    {
                                        flagList.Add(address, 0);
                                    }
                                    flagList[address] |= (byte)(1 << bit);
                                }
                                destPointer = getROMPointerNotAligned((uint)(destPointer + 4));
                            }
                        }

                        //房间列表里没有这样的房间，是假出口
                        mainForm.AddRoomPos(sourcePos.X, sourcePos.Y, true, previewOnly);
                        drawBadRoom();
                    }
                    else
                    {
                        mainForm.AddRoomPos(sourcePos.X, sourcePos.Y, false, previewOnly);
                        drawGoodRoom(destPointer, false, null);
                    }
                    return;
                }
                currentPointer = currentPointer.Address + (uint)exitLength;
            }
            drawBadRoom();
            mainForm.AddRoomPos(sourcePos.X, sourcePos.Y, true, previewOnly);

            void drawGoodRoom(ROMPointer pointer, bool isUncertain, Dictionary<uint, byte> flagList)
            {
                if (!previewOnly)
                {
                    mainForm.sourceRoom = roomInfo.Room;
                    mainForm.destRoom = RoomStructs[pointer.Address];
                    var destX = 0;
                    var destY = 0;
                    //获取目标房间的位置坐标
                    if (GameType == GameTypeEnum.Aos)
                    {
                        destX = RoomStructs[pointer.Address].Left + (sbyte)(data[currentPointer + 11]);
                        destY = RoomStructs[pointer.Address].Top + (sbyte)(data[currentPointer + 13]);
                    }
                    else if (GameType == GameTypeEnum.Hod)
                    {
                        destX = RoomStructs[pointer.Address].Left + (sbyte)(data[currentPointer + 9]);
                        destY = RoomStructs[pointer.Address].Top + (sbyte)(data[currentPointer + 11]);
                        var XOffset = data[currentPointer + 8];
                        if (XOffset > 0x80) destX += 1;
                    }
                    mainForm.AddRoomPos(destX, destY, false, false, isUncertain);
                    mainForm.SetFlagListForDestSearch(flagList);
                    //画线
                    if (sourcePos.X != destX || sourcePos.Y != destY)
                    {
                        mainForm.AddLine(sourcePos.X, sourcePos.Y, destX, destY);
                    }
                }
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
            if (roomInfo == null || roomInfo.Room == null) return;
            //找到通向当前房间的所有出口
            var exits = FindNormalExits(roomInfo.Room.RoomPointer);
            List<uint> addresses = new List<uint>();
            foreach (var exit in exits)
            {
                addSourceRoom(exit, false);
            }

            getAllPossibleExits(roomInfo.Room.RoomPointer.Address, new Dictionary<uint, byte>());

            void getAllPossibleExits(uint address, Dictionary<uint, byte> flagList)
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
                        ushort flag = getUShort(currentPointerActualAddress - 2);
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
                            int bit = 0;
                            uint flagAddress = getFlagAddress(flag, out bit);
                            if (!newFlagList.ContainsKey(flagAddress))
                            {
                                newFlagList.Add(flagAddress, 0);
                            }
                            newFlagList[flagAddress] |= (byte)(1 << bit);
                        }
                        //可能性1：这个指针本身是个出口
                        if (currentPointerAddress > minExitAddress)
                        {
                            if (!exits.Exists(e => e.ExitPointer.Address == currentPointerAddress))
                            {
                                addSourceRoom(CreateExitInfo(currentPointerAddress), true, newFlagList);
                            }
                        }
                        //可能性2：是其他地方跳转过来的
                        getAllPossibleExits(currentPointerAddress - 4, newFlagList);
                    }
                }
            }

            void addSourceRoom(ExitInfo exitInfo, bool isUncertain, Dictionary<uint, byte> flagList = null)
            {
                var exitGroupId = 0;
                //出口分组
                if (GameType == GameTypeEnum.Aos)
                {
                    exitGroupId = (exitInfo.ExitPointer >> 2) & 3;
                }
                else if (GameType == GameTypeEnum.Hod)
                {
                    exitGroupId = (exitInfo.ExitPointer >> 2) % 3;
                }
                //出口的位置等于目标出口的位置
                var sourceXToRoom = exitInfo.SourceX;
                var sourceYToRoom = exitInfo.SourceY;
                foreach (var pointer in exitGroups[exitGroupId])
                {
                    var room = RoomStructs[pointer.Address];
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
                                    //不要红门传送
                                    if (searchLevel == 0)
                                        continue;
                                    //红门传送，判断红门的位置
                                    if (searchLevel == 1 && (room.GateHeight == null || !room.GateHeight.Contains(sourceYToRoom)))
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
                        if (currentPointer.Address == exitInfo.ExitPointer.Address)
                        {
                            mainForm.AddSourceRoom(RoomStructs[room.RoomPointer.Address], exitInfo, isUncertain, flagList);
                            break;
                        }
                        //找到了该出口位置对应的出口
                        if (sourceXToRoom == (sbyte)(data[currentPointer + 4]) && sourceYToRoom == (sbyte)(data[currentPointer + 5]))
                        {
                            var destPointer = getROMPointer(currentPointer);
                            //房间列表里没有这样的房间，是假出口
                            if (!RoomStructs.ContainsKey(destPointer.Address))
                            {
                                break;
                            }
                            //出口不是目标房间
                            if (destPointer.Address != roomInfo.Room.RoomPointer.Address)
                            {
                                break;
                            }
                            //出口是目标房间的其他入口
                            //需要排除，防止重复
                            if (currentPointer < exitInfo.ExitPointer && exits.Find(e => e.ExitPointer.Address == currentPointer.Address) != null)
                            {
                                break;
                            }
                            else
                            {
                                mainForm.AddSourceRoom(RoomStructs[room.RoomPointer.Address], exitInfo, isUncertain, flagList);
                                break;
                            }
                        }
                        currentPointer = currentPointer.Address + (uint)exitLength;
                        num++;
                    }
                }
            }
        }
        protected List<ExitInfo> FindNormalExits(ROMPointer roomPointer)
        {
            var result = new List<ExitInfo>();
            foreach (var rs in RoomStructs.Values)
            {
                foreach (var exit in rs.Exits)
                {
                    if (exit.ExitDestination.Address == roomPointer.Address)
                    {
                        result.Add(exit);
                    }
                }
            }
            return result;
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

        protected ushort getUShort(uint start)
        {
            return (ushort)(data[start] | (data[start + 1] << 8));
        }

        public ROMPointer getROMPointer(uint start)
        {
            if (start >= data.Length) return null;
            return data[start] | (data[start + 1] << 8) | (data[start + 2] << 16) | (data[start + 3] << 24);
        }

        public ROMPointer getROMPointerNotAligned(uint start)
        {
            if (start >= data.Length) return null;
            return getUint(start);
        }

        protected virtual ExitInfo CreateExitInfo(ROMPointer pointer)
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
