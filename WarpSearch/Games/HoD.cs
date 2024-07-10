using System.Collections.Generic;
using System.Drawing;
using WarpSearch.Common;

namespace WarpSearch.Games
{
    public class HoD : GbaCv
    {
        private int castleYDiff = 40;
        private int topOffset = 0;

        private List<uint> currentRootRoomAddressList = new List<uint>();

        protected List<RomPointer> specialRomPointers { get; set; }

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

        public HoD(byte[] fileData) : base(fileData)
        {
            exitGroups = new List<List<RomPointer>>() { new List<RomPointer>(), new List<RomPointer>(), new List<RomPointer>() };
            GameType = GameTypeEnum.Hod;
            exitLength = 12;
            //美版和日版通用
            RoomFlagStart = 0x314;
            MapWidth = 80;
            MapHeight = 80;
            castleYDiff = 40;
            topOffset = 0;
        }
        public override void LoadRooms()
        {
            RoomsAtPositions.Clear();
            RoomStructs.Clear();
            foreach (var exitGroup in exitGroups)
            {
                exitGroup.Clear();
            }
            //maxExitAddress = 0x8_00_00_00;
            minExitAddress = 0xA_00_00_00;
            InitPointerAddress();

            MapElements.Rooms.Clear();
            MapElements.Lines.Clear();
            MapElements.Texts.Clear();

            RomPointer pointer = MapPointer;

            int tmp = 0;
            mapPositionsToDraw.Clear();
            int sector = 0, roomId = 0;

            //从地图读取房间
            for (int i = 0; i < 5120; i += 2)
            {
                tmp = getUShort(MapPointer, i);
                int x = (i & 127) >> 1;
                int y = (i >> 7) + topOffset;
                int y2 = y + castleYDiff;
                if (tmp != 0xffff)
                {
                    //表城和里城共用地图，一次加载两个格子
                    var mapSquareType1 = MapSquareType.Null;
                    var mapSquareType2 = MapSquareType.Null;

                    roomId = (tmp & 63) - 1;
                    sector = (tmp >> 8) & 15;

                    mapSquareType1 = MapSquareType.Normal;
                    mapSquareType2 = MapSquareType.Normal;
                    if ((tmp & 0x8000) != 0)
                    {
                        mapSquareType1 = MapSquareType.Save;
                        mapSquareType2 = MapSquareType.Save;
                    }
                    else
                    {
                        if ((tmp & 0x4000) != 0)
                        {
                            mapSquareType1 = MapSquareType.Warp;
                        }
                        if ((tmp & 0x2000) != 0)
                        {
                            mapSquareType2 = MapSquareType.Warp;
                        }
                    }

                    var room1 = createRoomStruct(sector, roomId, isCastleB: false);
                    if (room1 != null)
                    {
                        if (room1.Left <= x && room1.Left + room1.Width > x && room1.Top <= y && room1.Top + room1.Height > y)
                        {
                            RoomsAtPositions.Add(new Point(x, y), room1);
                        }
                    }
                    MapElements.Rooms.Add(new MapRoomToDraw { X = x, Y = y, SquareType = mapSquareType1 });

                    var room2 = createRoomStruct(sector, roomId, isCastleB: true);
                    if (room2 != null)
                    {
                        if (room2.Left <= x && room2.Left + room2.Width > x && room2.Top <= y2 && room2.Top + room2.Height > y2)
                        {
                            RoomsAtPositions.Add(new Point(x, y2), room2);
                        }
                    }
                    MapElements.Rooms.Add(new MapRoomToDraw { X = x, Y = y2, SquareType = mapSquareType2, IsCastleB = true });
                }

            }

            specialRoomDataList = GbaCvLoader.getSpecialRooms(GameTypeEnum.Hod, specialRomPointers);

            foreach (var specialRoomData in specialRoomDataList)
            {
                addSpecialRooms(specialRoomData);
            }


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
                int currentRoomId = 0;
                int currentRoomOffset = 0;
                RomPointer tmpRoomPointer = null;
                while (true)
                {
                    tmpRoomPointer = getRomPointer(sectorPointer, currentRoomOffset);
                    if (tmpRoomPointer == null || tmpRoomPointer < 0x8_00_00_00 || tmpRoomPointer > (uint)fileSize + 0x8_00_00_00) break;
                    if (sectorPointer.RomOffset + currentRoomOffset > nextSectorPointer.RomOffset) break;
                    if (createRoomStruct(i, currentRoomId, tmpRoomPointer) == null) break;

                    currentRoomId++;
                    currentRoomOffset += 4;
                }
            }

            foreach (var roomAddress in RoomStructs.Keys)
            {
                addExtraRoomsToMap(RoomStructs[roomAddress]);
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
                    AddLine(x, y, tmp >> 2, true);
                    AddLine(x, y2, tmp >> 2, true);
                    AddLine(x, y, tmp & 3, false);
                    AddLine(x, y2, tmp & 3, false);
                    tmp = getByte(pointer, i) & 15;
                }
            }

            foreach (var specialRoomData in specialRoomDataList)
            {
                DrawLinesUitls(specialRoomData.X, specialRoomData.Y + topOffset, specialRoomData.TopType, specialRoomData.BottomType, specialRoomData.LeftType, specialRoomData.RightType);
            }

            MapElements.Texts.Add(new MapTextToDraw { X = 67, Y = 35, Text = "BR Start", Size = 3 });
            MapElements.Texts.Add(new MapTextToDraw { X = 67, Y = 44, Text = "BR Midway", Size = 3 });
            MapElements.Texts.Add(new MapTextToDraw { X = 75, Y = 36, Text = "BR Easy", Size = 3 });
            MapElements.Texts.Add(new MapTextToDraw { X = 75, Y = 38, Text = "BR Normal", Size = 3 });
            MapElements.Texts.Add(new MapTextToDraw { X = 75, Y = 40, Text = "BR Hard", Size = 3 });
            MapElements.Texts.Add(new MapTextToDraw { X = 75, Y = 42, Text = "Debug", Size = 3 });

            void addSpecialRooms(SpecialRoomData specialRoomData)
            {
                var room = createRoomStruct(-1, -1, specialRoomData.romPointer, specialRoomData.Left, specialRoomData.Top);
                if (room != null)
                {
                    RoomsAtPositions.Add(new Point(specialRoomData.X, specialRoomData.Y), room);
                    MapElements.Rooms.Add(new MapRoomToDraw { X = specialRoomData.X, Y = specialRoomData.Y, SquareType = MapSquareType.Normal });
                }
            }

            void addExtraRoomsToMap(RoomStruct room, MapSquareType mapSquareType = MapSquareType.Error)
            {
                for (int i = room.Left; i < room.Left + room.Width; i++)
                {
                    for (int j = room.Top; j < room.Top + room.Height; j++)
                    {
                        Point p = new Point(i, j);
                        if (!RoomsAtPositions.ContainsKey(p))
                        {
                            RoomsAtPositions.Add(p, room);
                            if (!MapElements.Rooms.Exists(r => r.X == p.X && r.Y == p.Y))
                            {
                                MapElements.Rooms.Add(new MapRoomToDraw { X = p.X, Y = p.Y, SquareType = mapSquareType });
                            }
                        }
                        else
                        {
                            var existingRoom = RoomsAtPositions[p];
                            if (existingRoom.Equals(room))
                            {
                                continue;
                            }
                            if (!existingRoom.OverlappingRooms.Contains(room))
                            {
                                existingRoom.OverlappingRooms.Add(room);
                            }
                        }
                    }
                }
            }
        }

        private RoomStruct createRoomStruct(int mapSector = -1, int roomId = -1, RomPointer pointer = null, int left = -1, int top = -1,
            bool? isCastleB = null, int eventFlag = -1, RoomStruct rootRoom = null)
        {
            try
            {
                var sector = getMapRegionToSector(mapSector);
                if (isCastleB != null && isCastleB.Value && sector != -1)
                {
                    sector += 1;
                }
                //房间指针
                pointer = pointer ?? getRomPointer(getRomPointer(FirstRoomPointer, sector << 2), roomId << 2);
                if (pointer == null || pointer == 0)
                {
                    return null;
                }

                if (RoomStructs.ContainsKey(pointer))
                {
                    return RoomStructs[pointer];
                }

                RoomStruct rs = new RoomStruct();
                rs.EventFlag = eventFlag;
                rs.RoomPointer = pointer;

                rs.RoomId = roomId;
                rs.MapSector = mapSector;
                rs.Sector = sector;

                //房间的位置和大小
                var topLeft = getUShort(rs.RoomPointer, 34);
                rs.Left = left != -1 ? left : topLeft & 127;
                rs.Top = top != -1 ? top : (topLeft >> 7) & 127;
                rs.Top += topOffset;
                if (isCastleB == null)
                {
                    isCastleB = ((topLeft >> 14) & 1) == 1;
                }
                if (isCastleB.Value)
                {
                    rs.Top += castleYDiff;
                }

                var firstLayer = getRomPointer(getRomPointer(rs.RoomPointer, 8), 4);
                if (firstLayer == 0)
                {
                    rs.Width = 1;
                    rs.Height = 1;
                }
                else
                {
                    rs.Width = getByte(firstLayer, 0);
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
                            entityPointer += 0xc;
                        }
                    }
                }
                //出口
                rs.ExitPointer = getRomPointer(rs.RoomPointer, 28);
                var groupId = (int)((rs.ExitPointer.RomOffset >> 2) % 3);
                exitGroups[groupId].Add(rs.RoomPointer);
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
                    //if (rs.ExitPointer > maxExitAddress)
                    //{
                    //    maxExitAddress = rs.ExitPointer + (uint)((exitCount - 1) * exitLength);
                    //}
                    if (rs.ExitPointer < minExitAddress)
                    {
                        minExitAddress = rs.ExitPointer;
                    }
                }

                RoomStructs.Add(rs.RoomPointer, rs);

                var flag = getUShort(rs.RoomPointer, 2);
                if (flag != 0xFFFF)
                {
                    var newPointer = getRomPointer(rs.RoomPointer, 4);
                    if (rootRoom == null)
                    {
                        rootRoom = rs;
                    }
                    var subRoom = createRoomStruct(mapSector, roomId, newPointer, left, top, isCastleB, flag, rootRoom);
                    rootRoom.OverlappingRooms.Add(subRoom);
                }
                return rs;
            }
            catch
            {
                return null;
            }
        }

        protected override ExitInfo CreateExitInfo(RomPointer pointer)
        {
            if (ExitInfoCache.ContainsKey(pointer))
                return ExitInfoCache[pointer];
            var exit = new ExitInfo();
            exit.ExitPointer = pointer;
            exit.ExitDestination = getRomPointer(pointer, 0);
            exit.SourceX = (sbyte)getByte(pointer, 4);
            exit.SourceY = (sbyte)getByte(pointer, 5);
            exit.XOffset = getByte(pointer, 8);
            exit.DestX = (sbyte)getByte(pointer, 9);
            if (exit.XOffset > 0x80) exit.DestX += 1;
            exit.YOffset = getByte(pointer, 10);
            exit.DestY = (sbyte)getByte(pointer, 11);
            ExitInfoCache.Add(pointer, exit);
            return exit;
        }
    }
}
