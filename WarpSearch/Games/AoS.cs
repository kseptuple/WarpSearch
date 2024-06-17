using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using WarpSearch.Common;

namespace WarpSearch.Games
{
    public class AoS : GbaCv
    {
        private Dictionary<Point, RoomInfo> roomPositions = new Dictionary<Point, RoomInfo>();
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
                    MapWidth = 64;
                    MapHeight = 56;
                }
                else
                {
                    MapWidth = 52;
                    MapHeight = 48;
                }
            }
        }

        public AoS(byte[] fileData, FormMain formMain) : base(fileData, formMain)
        {
            exitGroups = new List<List<RomPointer>>() { new List<RomPointer>(), new List<RomPointer>(), new List<RomPointer>(), new List<RomPointer>() };
            GameType = GameTypeEnum.Aos;
            exitLength = 16;
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
            for (int i = 0; i < 6016; i += 2)
            {
                tmp = (getByte(pointer, i + 1) << 8) | getByte(pointer, i);
                int x = (i & 127) >> 1;
                int y = i >> 7;
                if (tmp != 0xffff)
                {
                    var tmpRoom = new RoomInfo();
                    tmpRoom.X = x;
                    tmpRoom.Y = y;
                    tmpRoom.RoomId = tmp & 63;
                    tmpRoom.Sector = (tmp >> 6) & 15;
                    tmpRoom.MapSector = tmpRoom.Sector;
                    tmpRoom.Type = RoomType.Normal;
                    if ((tmp & 0x8000) != 0)
                    {
                        tmpRoom.Type = RoomType.Save;
                    }
                    else if ((tmp & 0x4000) != 0)
                    {
                        tmpRoom.Type = RoomType.Warp;
                    }

                    if (load)
                    {
                        loadRoomInfo(tmpRoom);
                    }
                    mainForm.DrawRoom(x, y, tmpRoom.Type);
                    mapPositionList.Add(new Point(x, y));
                }
                else
                {
                    mainForm.DrawRoom(x, y, RoomType.Null);
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
                RomPointer firstSectorPointer = getRomPointer(FirstRoomPointer, 0);
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
                    //晓月only
                    if (tmpRoomPointer == firstSectorPointer) continue;
                    while (loadRoomInfo(new RoomInfo()
                    {
                        X = -1,
                        Y = -1,
                        RoomId = currentRoom,
                        Sector = currentSector,
                        MapSector = -1,
                        Type = RoomType.Normal
                    }, tmpRoomPointer, -1, -1, true, currentRoom))
                    {
                        currentRoom++;
                        tmpRoomPointer = getRomPointer(sectorPointer, currentRoom << 2);
                        if (tmpRoomPointer == null || tmpRoomPointer < 0x8_00_00_00 || tmpRoomPointer > (uint)fileSize + 0x8_00_00_00) break;
                        if (tmpRoomPointer > nextSectorPointer) break;
                        if (tmpRoomPointer == firstSectorPointer) break;
                    }
                }
            }

            specialRoomDataList = GbaCvLoader.getSpecialRooms(GameTypeEnum.Aos, UseHackSupport, specialRomPointers);

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
            for (int i = 0; i < 1504; i++)
            {
                tmp = getByte(pointer, i) >> 4;
                for (int j = 0; j < 2; j++)
                {
                    int x = ((i & 31) << 1) | j;
                    int y = i >> 5;
                    switch (tmp >> 2)
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
                    switch (tmp & 3)
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
                    tmp = getByte(pointer, i) & 15;
                }
            }

            foreach (var specialRoomData in specialRoomDataList)
            {
                DrawLinesUitls(specialRoomData.X, specialRoomData.Y, specialRoomData.TopType, specialRoomData.BottomType, specialRoomData.LeftType, specialRoomData.RightType);
            }

            if (useHackSupport)
            {
                mainForm.DrawText("Boss Rush", 12, 53, 3);
                mainForm.DrawText("Debug Room", 12, 50, 3);
                mainForm.DrawText("Bad Ending", 47, 51, 3);
            }
            else
            {
                mainForm.DrawText("Boss Rush", 15, 42, 3);
                mainForm.DrawText("Debug Room", 15, 39, 3);
            }
        }

        private void addSpecialRooms(SpecialRoomData specialRoomData, bool isLoad)
        {
            mainForm.DrawRoom(specialRoomData.X, specialRoomData.Y, RoomType.Normal);
            mapPositionList.Add(new Point(specialRoomData.X, specialRoomData.Y));
            if (isLoad)
            {
                loadRoomInfo(new RoomInfo()
                {
                    X = specialRoomData.X,
                    Y = specialRoomData.Y,
                    RoomId = -1,
                    Sector = -1,
                    MapSector = -1,
                    Type = RoomType.Normal
                }, specialRoomData.romPointer, specialRoomData.Left, specialRoomData.Top);
            }
        }

        private bool loadRoomInfo(RoomInfo roomInfo, RomPointer pointer = null, int left = -1, int top = -1, bool isFromRoomList = false, int roomId = -1)
        {
            try
            {
                RoomStruct rs = new RoomStruct();
                //房间指针
                rs.RoomPointer = pointer ?? getRomPointer(getRomPointer(FirstRoomPointer, roomInfo.Sector << 2), roomInfo.RoomId << 2);
                if (rs.RoomPointer == null || rs.RoomPointer == 0)
                {
                    return false;
                }
                Point p = new Point(roomInfo.X, roomInfo.Y);
                if (!isFromRoomList)
                {
                    //地图上对应的房间
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
                    if (rs.ExitPointer > maxExitAddress)
                    {
                        maxExitAddress = rs.ExitPointer + (uint)((exitCount - 1) * exitLength);
                    }
                    if (rs.ExitPointer < minExitAddress)
                    {
                        minExitAddress = rs.ExitPointer;
                    }
                }

                rs.Exits = new List<ExitInfo>();
                var currentExit = rs.ExitPointer;
                for (int j = 0; j < exitCount; j++)
                {
                    rs.Exits.Add(CreateExitInfo(currentExit));
                    currentExit = currentExit + (uint)exitLength;
                }
                roomInfo.Room = rs;

                //从地图加载时，是按格子加载的
                //从房间列表加载时，是整个房间加载的
                if (!isFromRoomList)
                {
                    RoomsAtPositions[p] = roomInfo;
                }
                else
                {
                    List<uint> wrongPositionRoomList = new List<uint>();
                    //currentRootRoomAddressList：与当前房间重叠的房间
                    List<uint> currentRootRoomAddressList = new List<uint>();
                    for (int i = rs.Left; i < rs.Left + rs.Width; i++)
                    {
                        for (int j = rs.Top; j < rs.Top + rs.Height; j++)
                        {
                            p = new Point(i, j);
                            if (!RoomsAtPositions.ContainsKey(p))
                            {
                                RoomsAtPositions.Add(p, roomInfo);
                            }
                            else if (RoomsAtPositions[p].Room == null)
                            {
                                RoomsAtPositions[p].RoomId = roomId;
                                RoomsAtPositions[p].Room = rs;
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
                        if (!FlagRoomLists.ContainsKey(rootRoomAddress))
                        {
                            FlagRoomLists.Add(rootRoomAddress, new List<RoomInfo>());
                        }
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
            exit.SourceX = (sbyte)(getByte(pointer,4));
            exit.SourceY = (sbyte)(getByte(pointer, 5));
            exit.XOffset = getByte(pointer, 10);
            exit.DestX = (sbyte)(getByte(pointer, 11));
            exit.YOffset = getByte(pointer, 12);
            exit.DestY = (sbyte)(getByte(pointer, 13));
            return exit;
        }
    }
}
