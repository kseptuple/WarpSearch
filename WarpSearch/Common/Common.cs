using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarpSearch.Common
{
    public class RoomInfo
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Sector { get; set; }
        public int MapSector { get; set; }
        public int RoomId { get; set; }
        public RoomType Type { get; set; }
        public RoomStruct Room { get; set; }
    }

    public class RoomStruct
    {
        public RomPointer RoomPointer { get; set; }
        public int EventFlag { get; set; }
        public RomPointer ExitPointer { get; set; }
        public List<int> GateHeight { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }
        public List<ExitInfo> Exits { get; set; }

        public RoomStruct()
        {
            EventFlag = -1;
        }
    }

    public class ExitInfo
    {
        public RomPointer ExitDestination { get; set; }
        public int SourceX { get; set; }
        public int SourceY { get; set; }
        public int DestX { get; set; }
        public int XOffset { get; set; }
        public int DestY { get; set; }
        public int YOffset { get; set; }
        public RomPointer ExitPointer { get; set; }
    }

    public class SpecialRoomData
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int TopType { get; set; }
        public int BottomType { get; set; }
        public int LeftType { get; set; }
        public int RightType { get; set; }
        public RomPointer romPointer { get; set; }
        public SpecialRoomData(int X, int Y, int Left, int Top, int TopType, int BottomType, int LeftType, int RightType, RomPointer romPointer)
        {
            this.X = X;
            this.Y = Y;
            this.Left = Left;
            this.Top = Top;
            this.TopType = TopType;
            this.BottomType = BottomType;
            this.LeftType = LeftType;
            this.RightType = RightType;
            this.romPointer = romPointer;
        }
    }

    public enum RoomType
    {
        Null,
        Normal,
        Warp,
        Save,
        Error
    }


    public class RoomAndExit
    {
        public RoomStruct Room { get; set; }
        public ExitInfo Exit { get; set; }
        public bool IsUncertain { get; set; }

        public RoomAndExit(RoomStruct room, ExitInfo exit)
        {
            Room = room;
            Exit = exit;
            IsUncertain = false;
        }
        public RoomAndExit(RoomStruct room, ExitInfo exit, bool isUncertain)
        {
            Room = room;
            Exit = exit;
            IsUncertain = isUncertain;
        }

        public override string ToString()
        {
            return IsUncertain ? Room.RoomPointer.ToString() + "*" : Room.RoomPointer.ToString();
        }
    }

    public enum GameTypeEnum
    {
        Null,
        Hod,
        Aos
    }

    public enum GameVersionEnum
    {
        USA,
        EUR,
        JPN
    }
}
