using System.Collections.Generic;
using System.Drawing;
using WarpSearch.Common;

namespace WarpSearch.Games
{
    public class AoS : GbaCv
    {
        protected List<RomPointer> specialRomPointers { get; set; }

        public AoS(byte[] fileData) : base(fileData)
        {
            exitGroups = new List<List<RomPointer>>() { new List<RomPointer>(), new List<RomPointer>(), new List<RomPointer>(), new List<RomPointer>() };
            GameType = GameTypeEnum.Aos;
            exitLength = 16;
            MapWidth = 64;
            MapHeight = 56;
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

            int tmp = 0;
            mapPositionsToDraw.Clear();
            int sector = 0, roomId = 0;

            //从地图读取房间
            for (int i = 0; i < 6016; i += 2)
            {
                tmp = getUShort(MapPointer, i);
                int x = (i & 127) >> 1;
                int y = i >> 7;
                var mapSquareType = MapSquareType.Null;
                if (tmp != 0xffff)
                {
                    roomId = tmp & 63;
                    sector = (tmp >> 6) & 15;

                    mapSquareType = MapSquareType.Normal;
                    if ((tmp & 0x8000) != 0)
                    {
                        mapSquareType = MapSquareType.Save;
                    }
                    else if ((tmp & 0x4000) != 0)
                    {
                        mapSquareType = MapSquareType.Warp;
                    }

                    var room = createRoomStruct(sector, roomId);
                    if (room != null)
                    {
                        if (room.Left <= x && room.Left + room.Width > x && room.Top <= y && room.Top + room.Height > y)
                        {
                            RoomsAtPositions.Add(new Point(x, y), room);
                        }
                    }
                    MapElements.Rooms.Add(new MapRoomToDraw { X = x, Y = y, SquareType = mapSquareType });
                }
            }

            specialRoomDataList = GbaCvLoader.getSpecialRooms(GameTypeEnum.Aos, specialRomPointers);

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
            RomPointer firstSectorPointer = getRomPointer(FirstRoomPointer, 0);
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
                    if (tmpRoomPointer == firstSectorPointer) break;
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
            for (int i = 0; i < 1504; i++)
            {
                tmp = getByte(MapLinePointer, i) >> 4;
                for (int j = 0; j < 2; j++)
                {
                    int x = ((i & 31) << 1) | j;
                    int y = i >> 5;
                    AddLine(x, y, tmp >> 2, true);
                    AddLine(x, y, tmp & 3, false);
                    tmp = getByte(MapLinePointer, i) & 15;
                }
            }

            foreach (var specialRoomData in specialRoomDataList)
            {
                DrawLinesUitls(specialRoomData.X, specialRoomData.Y, specialRoomData.TopType, specialRoomData.BottomType, specialRoomData.LeftType, specialRoomData.RightType);
            }

            MapElements.Texts.Add(new MapTextToDraw { X = 12, Y = 53, Text = "Boss Rush", Size = 3 });
            MapElements.Texts.Add(new MapTextToDraw { X = 12, Y = 50, Text = "Debug Room", Size = 3 });
            MapElements.Texts.Add(new MapTextToDraw { X = 47, Y = 51, Text = "Bad Ending", Size = 3 });

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

        private RoomStruct createRoomStruct(int sector = -1, int roomId = -1, RomPointer pointer = null, int left = -1, int top = -1, RoomStruct rootRoom = null)
        {
            try
            {
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
                rs.RoomPointer = pointer;

                rs.RoomId = roomId;
                rs.MapSector = sector;
                rs.Sector = sector;

                //房间的位置和大小
                var topLeft = getUShort(rs.RoomPointer, 34);
                rs.Left = left != -1 ? left : topLeft & 127;
                rs.Top = top != -1 ? top : (topLeft >> 7) & 127;

                var firstLayer = getRomPointer(getRomPointer(rs.RoomPointer, 8), 8);
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

                //出口
                rs.ExitPointer = getRomPointer(rs.RoomPointer, 24);
                var groupId = (int)((rs.ExitPointer.RomOffset >> 2) & 3);
                exitGroups[groupId].Add(rs.RoomPointer);
                int exitCount = getUShort(rs.RoomPointer, 28);
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

                var currentExit = rs.ExitPointer;
                for (int j = 0; j < exitCount; j++)
                {
                    rs.Exits.Add(CreateExitInfo(currentExit));
                    currentExit = currentExit + (uint)exitLength;
                }
                RoomStructs.Add(rs.RoomPointer, rs);
                var flag = getUShort(rs.RoomPointer, 2);
                rs.NextRoomFlag = flag;

                if (flag != 0xFFFF)
                {
                    var newPointer = getRomPointer(rs.RoomPointer, 4);
                    if (rootRoom == null)
                    {
                        rootRoom = rs;
                    }
                    var subRoom = createRoomStruct(sector, roomId, newPointer, left, top, rootRoom);
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
            exit.XOffset = getByte(pointer, 10);
            exit.DestX = (sbyte)getByte(pointer, 11);
            exit.YOffset = getByte(pointer, 12);
            exit.DestY = (sbyte)getByte(pointer, 13);
            ExitInfoCache.Add(pointer, exit);
            return exit;
        }
    }
}
