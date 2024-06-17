using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using WarpSearch.Common;

namespace WarpSearch.Games
{
    public class HoD : GbaCv
    {
        private Dictionary<Point, RoomInfo> roomPositions = new Dictionary<Point, RoomInfo>();
        private int castleYDiff = 40;
        private int topOffset = 0;

        private List<uint> currentRootRoomAddressList = new List<uint>();

        protected List<RomPointer> specialRomPointers { get; set; }
        public override bool UseHackSupport
        {
            get
            {
                return useHackSupport;
            }
            set
            {
                useHackSupport = value;
                if (value)
                {
                    MapWidth = 80;
                    MapHeight = 80;
                    castleYDiff = 40;
                    topOffset = 0;
                }
                else
                {
                    MapWidth = 64;
                    MapHeight = 72;
                    castleYDiff = 36;
                    topOffset = -3;
                }
            }
        }

        private static readonly Dictionary<int, int> MapRegionToSector = new Dictionary<int, int>()
        {
            {0, 0}, {1, 2}, {2, 4}, {3, 6}, {4, 8}, {5, 0xA}, {6, 0xC}, {7, 0xE}, {8, 0x10}, {9, 0x12}, {0xA, 2}, {0xB, 4}, {0xC, 0xE}
        };

        private static int getMapRegionToSector(int key)
        {
            if (MapRegionToSector.ContainsKey(key))
            {
                return MapRegionToSector[key];
            }
            else
            {
                return key;
            }
        }

        public HoD(byte[] fileData, FormMain formMain) : base(fileData, formMain)
        {
            exitGroups = new List<List<RomPointer>>() { new List<RomPointer>(), new List<RomPointer>(), new List<RomPointer>() };
            GameType = GameTypeEnum.Hod;
            exitLength = 12;
            //美版和日版通用
            RoomFlagStart = 0x314;
        }
        public override void LoadRooms(bool load = true)
        {
            if (load)
            {
                RoomsAtPositions.Clear();
                RoomStructs.Clear();
                FlagRoomLists.Clear();
                roomPositions.Clear();
                foreach (var exitGroup in exitGroups)
                {
                    exitGroup.Clear();
                }
                maxExitAddress = 0x8_00_00_00;
                minExitAddress = 0xA_00_00_00;
                InitPointerAddress();
            }
            RomPointer pointer = MapPointer;
            //不再使用RoomRootPointer加载房间

            int tmp = 0;
            mapPositionList.Clear();
            //从地图读取房间
            for (int i = 0; i < 5120; i += 2)
            {
                tmp = (getByte(pointer, i+1) << 8) | getByte(pointer, i);
                int x = (i & 127) >> 1;
                int y = (i >> 7) + topOffset;
                int y2 = y + castleYDiff;
                if (tmp != 0xffff)
                {
                    //表城和里城共用地图，一次加载两个格子
                    var tmpRoom = new RoomInfo();
                    var tmpRoom2 = new RoomInfo();
                    tmpRoom.X = x;
                    tmpRoom.Y = y;
                    tmpRoom.RoomId = (tmp & 63) - 1;
                    tmpRoom.MapSector = (tmp >> 8) & 15;
                    tmpRoom.Sector = getMapRegionToSector(tmpRoom.MapSector);
                    tmpRoom2.X = x;
                    tmpRoom2.Y = y2;
                    tmpRoom2.RoomId = (tmp & 63) - 1;
                    tmpRoom2.MapSector = (tmp >> 8) & 15;
                    tmpRoom2.Sector = getMapRegionToSector(tmpRoom2.MapSector) + 1;

                    tmpRoom.Type = RoomType.Normal;
                    tmpRoom2.Type = RoomType.Normal;
                    if ((tmp & 0x8000) != 0)
                    {
                        tmpRoom.Type = RoomType.Save;
                        tmpRoom2.Type = RoomType.Save;
                    }
                    else
                    {
                        if ((tmp & 0x4000) != 0)
                        {
                            tmpRoom.Type = RoomType.Warp;
                        }
                        if ((tmp & 0x2000) != 0)
                        {
                            tmpRoom2.Type = RoomType.Warp;
                        }
                    }

                    if (load)
                    {
                        loadRoomInfo(tmpRoom);
                        loadRoomInfo(tmpRoom2, null, -1, -1, true);
                    }
                    mainForm.DrawRoom(x, y, tmpRoom.Type);
                    mainForm.DrawRoom(x, y2, tmpRoom2.Type, true);
                    mapPositionList.Add(new Point(x, y));
                    mapPositionList.Add(new Point(x, y2));
                }
                else
                {
                    mainForm.DrawRoom(x, y, RoomType.Null);
                    mainForm.DrawRoom(x, y2, RoomType.Null);
                }
            }
            if (useHackSupport && load)
            {
                //从房间列表补充房间
                List<RomPointer> sectorList = new List<RomPointer>();
                int currentSector = 0;
                //获取区域列表
                while (true)
                {
                    RomPointer tmpSectorPointer = getRomPointer(FirstRoomPointer, currentSector << 2);
                    if (tmpSectorPointer == null || tmpSectorPointer == 0) break;
                    if (tmpSectorPointer < 0x8_00_00_00 || tmpSectorPointer >= (uint)fileSize + 0x8_00_00_00) break;
                    sectorList.Add(tmpSectorPointer);
                    currentSector++;
                }
                sectorList.Sort();
                for (int i = 0; i < sectorList.Count; i++)
                {
                    var sectorPointer = sectorList[i];
                    //到下个区域或者rom结束/非指针为止
                    //使用dsvedit的判断方法
                    var nextSectorPointer = i == sectorList.Count - 1 ? new RomPointer((uint)fileSize + 0x8_00_00_00) : sectorList[i + 1];
                    int currentRoom = 0;
                    RomPointer tmpRoomPointer = getRomPointer(sectorPointer, currentRoom << 2);
                    if (tmpRoomPointer == null || tmpRoomPointer < 0x8_00_00_00 || tmpRoomPointer > (uint)fileSize + 0x8_00_00_00) continue;
                    if (tmpRoomPointer > nextSectorPointer) continue;
                    while (loadRoomInfo(new RoomInfo()
                    {
                        X = -1,
                        Y = -1,
                        RoomId = currentRoom,
                        Sector = currentSector,
                        MapSector = -1,
                        Type = RoomType.Normal
                    }, tmpRoomPointer, -1, -1, false, -1, true, currentRoom))
                    {
                        currentRoom++;
                        tmpRoomPointer = getRomPointer(sectorPointer, currentRoom << 2);
                        if (tmpRoomPointer == null || tmpRoomPointer < 0x8_00_00_00 || tmpRoomPointer > (uint)fileSize + 0x8_00_00_00) break;
                        if (tmpRoomPointer > nextSectorPointer) break;
                    }
                }
            }

            specialRoomDataList = GbaCvLoader.getSpecialRooms(GameTypeEnum.Hod, UseHackSupport, specialRomPointers);

            foreach (var specialRoomData in specialRoomDataList)
            {
                addSpecialRooms(specialRoomData, load);
            }

            if (useHackSupport)
            {
                //画到地图外面的房间
                foreach (var roomPosition in roomPositions)
                {
                    if (load && !RoomsAtPositions.ContainsKey(roomPosition.Key))
                    {
                        RoomsAtPositions.Add(roomPosition.Key, roomPosition.Value);
                    }
                    if (!mapPositionList.Exists(p => p.X == roomPosition.Key.X && p.Y == roomPosition.Key.Y))
                    {
                        mainForm.DrawRoom(roomPosition.Key.X, roomPosition.Key.Y, RoomType.Error);
                        mapPositionList.Add(new Point(roomPosition.Key.X, roomPosition.Key.Y));
                    }
                }
            }

            //画地图线
            pointer = MapLinePointer;
            for (int i = 0; i < 1280; i++)
            {
                tmp = getByte(pointer, i) >> 4;
                for (int j = 0; j < 2; j++)
                {
                    int x = ((i & 31) << 1) | j;
                    int y = (i >> 5) + topOffset;
                    int y2 = y + castleYDiff;
                    switch (tmp >> 2)
                    {
                        case 1:
                        case 2:
                            mainForm.DrawLine(x, y, true, false);
                            mainForm.DrawLine(x, y2, true, false);
                            break;
                        case 3:
                            mainForm.DrawLine(x, y, true, true);
                            mainForm.DrawLine(x, y2, true, true);
                            break;
                        default:
                            break;
                    }
                    switch (tmp & 3)
                    {
                        case 1:
                        case 2:
                            mainForm.DrawLine(x, y, false, false);
                            mainForm.DrawLine(x, y2, false, false);
                            break;
                        case 3:
                            mainForm.DrawLine(x, y, false, true);
                            mainForm.DrawLine(x, y2, false, true);
                            break;
                        default:
                            break;
                    }
                    tmp = getByte(pointer, i) & 15;
                }
            }

            foreach (var specialRoomData in specialRoomDataList)
            {
                DrawLinesUitls(specialRoomData.X, specialRoomData.Y + topOffset, specialRoomData.TopType, specialRoomData.BottomType, specialRoomData.LeftType, specialRoomData.RightType);
            }

            if (useHackSupport)
            {
                mainForm.DrawText("BR Start", 67, 35, 3);
                mainForm.DrawText("BR Midway", 67, 44, 3);
                mainForm.DrawText("BR Easy", 75, 36, 3);
                mainForm.DrawText("BR Normal", 75, 38, 3);
                mainForm.DrawText("BR Hard", 75, 40, 3);
                mainForm.DrawText("Debug", 75, 42, 3);
            }
            else
            {
                mainForm.DrawText("Boss Rush Start", 34, 34, 3);
                mainForm.DrawText("Boss Rush Midway", 34, 38, 3);
                mainForm.DrawText("Boss Rush Easy", 50, 33, 3);
                mainForm.DrawText("Boss Rush Normal", 50, 35, 3);
                mainForm.DrawText("Boss Rush Hard", 50, 37, 3);
                mainForm.DrawText("Debug Room", 50, 39, 3);
            }
        }

        private void addSpecialRooms(SpecialRoomData specialRoomData, bool isLoad)
        {
            mainForm.DrawRoom(specialRoomData.X, specialRoomData.Y + topOffset, RoomType.Normal);
            mapPositionList.Add(new Point(specialRoomData.X, specialRoomData.Y));
            if (isLoad)
            {
                loadRoomInfo(new RoomInfo()
                {
                    X = specialRoomData.X,
                    Y = specialRoomData.Y + topOffset,
                    RoomId = -1,
                    Sector = -1,
                    MapSector = -1,
                    Type = RoomType.Normal
                }, specialRoomData.romPointer, specialRoomData.Left, specialRoomData.Top);
            }
        }

        private bool loadRoomInfo(RoomInfo roomInfo, RomPointer pointer = null, int left = -1, int top = -1, bool isCastleB = false, int eventFlag = -1, bool isFromRoomList = false, int roomId = -1)
        {
            try
            {
                RoomStruct rs = new RoomStruct();
                rs.EventFlag = eventFlag;
                //房间指针
                rs.RoomPointer = pointer ?? getRomPointer(getRomPointer(FirstRoomPointer, roomInfo.Sector << 2), roomInfo.RoomId << 2);
                if (rs.RoomPointer == null || rs.RoomPointer == 0)
                {
                    return false;
                }
                Point p = new Point(roomInfo.X, roomInfo.Y);
                //地图上对应的房间
                if (!isFromRoomList)
                {
                    if (!RoomsAtPositions.ContainsKey(p))
                        RoomsAtPositions.Add(p, roomInfo);
                }
                if (RoomStructs.ContainsKey(rs.RoomPointer))
                {
                    if (!isFromRoomList)
                    {
                        roomInfo.Room = RoomStructs[rs.RoomPointer];
                        RoomsAtPositions[p] = roomInfo;
                    }
                    return true;
                }
                //房间的位置和大小
                var topLeft = getUShort(rs.RoomPointer, 34);
                rs.Left = left != -1 ? left : topLeft & 127;
                rs.Top = top != -1 ? top : (topLeft >> 7) & 127;
                rs.Top += topOffset;
                if (isFromRoomList)
                {
                    isCastleB = ((topLeft >> 14) & 1) == 1;
                }
                if (isCastleB) rs.Top = rs.Top + castleYDiff;
                var firstLayer = getRomPointer(getRomPointer(rs.RoomPointer, 8), 4);
                if (firstLayer == 0)
                {
                    rs.Width = 1;
                    rs.Height = 1;
                }
                else
                {
                    rs.Width = getByte(firstLayer,0);
                    rs.Height = getByte(firstLayer, 1);
                }
                if (rs.Width == 1)
                {
                    var entityPointer = getRomPointer(rs.RoomPointer, 24);
                    if (entityPointer != 0)
                    {
                        //获取房间内部的物体，记录红门的位置
                        while (getUShort(entityPointer, 0) != 0x7fff)
                        {
                            if (getByte(entityPointer, 0) >= 0x80)
                            {
                                if ((getByte(entityPointer, 4) & 0xc0) == 0x40 && getByte(entityPointer, 5) == 5)
                                {
                                    if (rs.GateHeight == null) rs.GateHeight = new List<int>();
                                    rs.GateHeight.Add(getByte(entityPointer, 3));
                                }
                            }
                            entityPointer = entityPointer + 0xc;
                        }
                    }
                }
                //出口
                rs.ExitPointer = getRomPointer(rs.RoomPointer, 28);
                var groupId = (int)((rs.ExitPointer.RomOffset >> 2) % 3);
                exitGroups[groupId].Add(rs.RoomPointer);
                rs.Exits = new List<ExitInfo>();
                var currentExit = rs.ExitPointer;
                int exitCount = 0;
                while (true)
                {
                    var exitAddress = getRomPointer(currentExit, 0);
                    if (exitAddress < 0x8_00_00_00 || exitAddress >= (uint)fileSize + 0x8_00_00_00) break;
                    rs.Exits.Add(CreateExitInfo(currentExit));
                    currentExit = currentExit + (uint)exitLength;
                    exitCount++;
                }
                if (exitCount != 0)
                {
                    if (currentExit > maxExitAddress)
                    {
                        maxExitAddress = currentExit + (uint)((exitCount - 1) * exitLength);
                    }
                    if (currentExit < minExitAddress)
                    {
                        minExitAddress = currentExit;
                    }
                }
                if (currentExit > maxExitAddress && exitCount != 0)
                {
                    maxExitAddress = rs.ExitPointer + (uint)((exitCount - 1) * exitLength);
                }
                roomInfo.Room = rs;
                if (eventFlag == -1)
                {
                    //currentRootRoomAddressList：与当前房间重叠的房间，也可以是主房间
                    currentRootRoomAddressList.Clear();
                    //没有事件flag参数，说明是第一层房间，把当前房间设置为地图该位置对应的房间
                    //从地图加载时，是按格子加载的
                    //从房间列表加载时，是整个房间加载的
                    if (!isFromRoomList)
                    {
                        RoomsAtPositions[p] = roomInfo;
                        currentRootRoomAddressList.Add(roomInfo.Room.RoomPointer);
                    }
                    else
                    {
                        List<uint> wrongPositionRoomList = new List<uint>();
                        for (int i = rs.Left; i < rs.Left + rs.Width; i++)
                        {
                            for (int j = rs.Top; j < rs.Top + rs.Height; j++)
                            {
                                p = new Point(i, j);
                                if (!RoomsAtPositions.ContainsKey(p))
                                {
                                    RoomsAtPositions.Add(p, roomInfo);
                                    if (!currentRootRoomAddressList.Contains(roomInfo.Room.RoomPointer))
                                    {
                                        currentRootRoomAddressList.Add(roomInfo.Room.RoomPointer);
                                    }
                                }
                                else if (RoomsAtPositions[p].Room == null)
                                {
                                    RoomsAtPositions[p].RoomId = roomId;
                                    RoomsAtPositions[p].Room = rs;
                                    if (!currentRootRoomAddressList.Contains(roomInfo.Room.RoomPointer))
                                    {
                                        currentRootRoomAddressList.Add(roomInfo.Room.RoomPointer);
                                    }
                                }
                                else
                                {
                                    RoomStruct originalRoom = RoomsAtPositions[p].Room;
                                    RoomInfo originalRoomInfo = RoomsAtPositions[p];
                                    if (originalRoom.Left > p.X || originalRoom.Left + originalRoom.Width < p.X
                                        || originalRoom.Top > p.Y || originalRoom.Top + originalRoom.Height < p.Y)
                                    {
                                        if (!wrongPositionRoomList.Contains(originalRoom.RoomPointer))
                                        {
                                            wrongPositionRoomList.Add(originalRoom.RoomPointer);
                                            if (!FlagRoomLists.ContainsKey(rs.RoomPointer))
                                            {
                                                FlagRoomLists.Add(rs.RoomPointer, new List<RoomInfo>());
                                            }
                                            if (FlagRoomLists.ContainsKey(originalRoom.RoomPointer))
                                            {
                                                FlagRoomLists[rs.RoomPointer].Add(originalRoomInfo);
                                                foreach (var originalRoomList in FlagRoomLists[originalRoom.RoomPointer])
                                                {
                                                    FlagRoomLists[rs.RoomPointer].Add(originalRoomList);
                                                }
                                            }
                                        }
                                        RoomsAtPositions.Remove(p);
                                        RoomsAtPositions.Add(p, roomInfo);
                                        if (!currentRootRoomAddressList.Contains(roomInfo.Room.RoomPointer))
                                        {
                                            currentRootRoomAddressList.Add(roomInfo.Room.RoomPointer);
                                        }
                                    }
                                    else
                                    {
                                        if (!currentRootRoomAddressList.Contains(RoomsAtPositions[p].Room.RoomPointer))
                                        {
                                            currentRootRoomAddressList.Add(RoomsAtPositions[p].Room.RoomPointer);
                                        }
                                    }
                                }
                            }
                        }

                        foreach (var rootRoomAddress in currentRootRoomAddressList)
                        {
                            if (rootRoomAddress == roomInfo.Room.RoomPointer)
                                continue;
                            if (!FlagRoomLists.ContainsKey(rootRoomAddress))
                            {
                                FlagRoomLists.Add(rootRoomAddress, new List<RoomInfo>());
                            }
                            FlagRoomLists[rootRoomAddress].Add(roomInfo);
                        }
                    }
                }
                else
                {
                    foreach (var rootRoomAddress in currentRootRoomAddressList)
                    {
                        FlagRoomLists[rootRoomAddress].Add(roomInfo);
                    }
                }
                RoomStructs.Add(roomInfo.Room.RoomPointer, roomInfo.Room);
                if (useHackSupport)
                {
                    for (int i = rs.Left; i < rs.Left + rs.Width; i++)
                    {
                        for (int j = rs.Top; j < rs.Top + rs.Height; j++)
                        {
                            Point mapPoint = new Point(i, j);
                            if (!roomPositions.ContainsKey(mapPoint))
                                roomPositions.Add(mapPoint, roomInfo);
                        }
                    }
                }
                var flag = getUShort(rs.RoomPointer, 2);
                if (flag != 0xFFFF)
                {
                    //事件flag不是-1，加载下一个事件flag的房间
                    if (eventFlag == -1)
                    {
                        foreach (var rootRoomAddress in currentRootRoomAddressList)
                        {
                            if (!FlagRoomLists.ContainsKey(rootRoomAddress))
                            {
                                FlagRoomLists.Add(rootRoomAddress, new List<RoomInfo>());
                            }
                        }
                    }
                    var newPointer = getRomPointer(rs.RoomPointer, 4);
                    var newRs = new RoomInfo()
                    {
                        Sector = roomInfo.Sector,
                        MapSector = roomInfo.MapSector,
                        RoomId = roomInfo.RoomId,
                        Type = roomInfo.Type,
                        X = roomInfo.X,
                        Y = roomInfo.Y
                    };
                    loadRoomInfo(newRs, newPointer, left, top, isCastleB, flag, isFromRoomList);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected override ExitInfo CreateExitInfo(RomPointer pointer)
        {
            var exit = new ExitInfo();
            exit.ExitPointer = pointer;
            exit.ExitDestination = getRomPointer(pointer, 0);
            exit.SourceX = (sbyte)(getByte(pointer, 4));
            exit.SourceY = (sbyte)(getByte(pointer, 5));
            exit.XOffset = getByte(pointer, 8);
            exit.DestX = (sbyte)(getByte(pointer, 9));
            if (exit.XOffset > 0x80) exit.DestX += 1;
            exit.YOffset = getByte(pointer, 10);
            exit.DestY = (sbyte)(getByte(pointer, 11));
            return exit;
        }
    }
}
