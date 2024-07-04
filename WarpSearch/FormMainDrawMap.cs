using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using WarpSearch.Common;

namespace WarpSearch
{
    public partial class FormMain
    {
        public void DrawRoom(int x, int y, MapSquareType type, bool isHodCastleB = false)
        {
            Brush currentBrush = null;
            x += globalOffset;
            y += globalOffset;
            switch (type)
            {
                case MapSquareType.Normal:
                    if (isHodCastleB)
                    {
                        currentBrush = greenBrush;
                        GreenRooms.Add(new Point(x, y));
                    }
                    else
                    {
                        currentBrush = blueBrush;
                    }
                    break;
                case MapSquareType.Warp:
                    if (isHodCastleB)
                    {
                        GreenRooms.Add(new Point(x, y));
                    }
                    currentBrush = yellowBrush;
                    break;
                case MapSquareType.Save:
                case MapSquareType.Error:
                    if (isHodCastleB)
                    {
                        GreenRooms.Add(new Point(x, y));
                    }
                    currentBrush = redBrush;
                    break;
                default:
                    currentBrush = blackBrush;
                    break;
            }
            bitmapGraphics.FillRectangle(currentBrush, x * gridSize, y * gridSize, gridSize, gridSize);
        }

        public void DrawLine(int x, int y, bool isHorizonal, bool isSolid)
        {
            x += 2;
            y += 2;
            if (isHorizonal)
            {
                if (isSolid)
                {
                    bitmapGraphics.FillRectangle(whiteBrush, x * gridSize, y * gridSize, gridSize + lineWidth, lineWidth);
                }
                else
                {
                    if (romType == GameTypeEnum.Hod)
                    {
                        bitmapGraphics.FillRectangle(whiteBrush, x * gridSize, y * gridSize, (gridSize - lineWidth) * 0.333333333f + lineWidth, lineWidth);
                        bitmapGraphics.FillRectangle(whiteBrush, x * gridSize + (gridSize - lineWidth) * 0.666666667f + lineWidth, y * gridSize, (gridSize - lineWidth) * 0.333333333f + lineWidth, lineWidth);
                    }
                    else
                    {
                        bitmapGraphics.FillRectangle(whiteBrush, x * gridSize, y * gridSize, gridSize + lineWidth, lineWidth);
                        bitmapGraphics.FillRectangle(cyanBrush, x * gridSize + lineWidth, y * gridSize, gridSize - lineWidth, lineWidth);
                    }
                }

            }
            else
            {
                if (isSolid)
                {
                    bitmapGraphics.FillRectangle(whiteBrush, x * gridSize, y * gridSize, lineWidth, gridSize + lineWidth);
                }
                else
                {
                    if (romType == GameTypeEnum.Hod)
                    {
                        bitmapGraphics.FillRectangle(whiteBrush, x * gridSize, y * gridSize, lineWidth, (gridSize - lineWidth) * 0.333333333f + lineWidth);
                        bitmapGraphics.FillRectangle(whiteBrush, x * gridSize, y * gridSize + (gridSize - lineWidth) * 0.666666667f + lineWidth, lineWidth, (gridSize - lineWidth) * 0.333333333f);
                    }
                    else
                    {
                        bitmapGraphics.FillRectangle(whiteBrush, x * gridSize, y * gridSize, lineWidth, gridSize + lineWidth);
                        bitmapGraphics.FillRectangle(cyanBrush, x * gridSize, y * gridSize + lineWidth, lineWidth, gridSize - lineWidth);
                    }
                }
            }
        }

        public void DrawText(string text, int x, int y, float size)
        {
            x += 2;
            y += 2;
            Font drawFont = new Font(SystemFonts.DefaultFont.FontFamily, size * scale);
            bitmapGraphics.DrawString(text, drawFont, trueWhiteBrush, x * gridSize, y * gridSize);
        }

        public void DrawMap()
        {
            if (rom != null)
            {
                var mapElements = rom.MapElements;
                foreach (var room in mapElements.Rooms)
                {
                    DrawRoom(room.X, room.Y, room.SquareType, room.IsCastleB);
                }
                foreach (var line in mapElements.Lines)
                {
                    DrawLine(line.X, line.Y, line.IsHorizonal, line.IsSolid);
                }
                foreach (var text in mapElements.Texts)
                {
                    DrawText(text.Text, text.X, text.Y, text.Size);
                }
            }
        }

        public void AddMapSquarePos(int x, int y, bool isBad, bool previewOnly = false, bool isUncertain = false)
        {
            var brush = transparentRedBrush;
            if (!isBad)
            {
                if (GreenRooms.Exists(p => p.X == x + globalOffset && p.Y == y + globalOffset))
                {
                    brush = transparentBlueBrush;
                }
                else
                {
                    brush = transparentGreenBrush;
                }
            }

            if (isUncertain)
            {
                brush = transparentOrangeBrush;
            }
            if (previewOnly)
            {
                PositionPreviewToDraw.Add(new RectangleToDraw(brush, x, y, 1, 1));
            }
            else
            {
                PositionToDraw.Add(new RectangleToDraw(brush, x, y, 1, 1));
            }
        }

        public void AddLine(int startX, int startY, int endX, int endY, bool hasArrow)
        {
            if (hasArrow)
                LinesToDraw.Add(new LineToDraw(redPen, redBrush2, startX, startY, endX, endY, true));
            else
                LinesToDraw.Add(new LineToDraw(orangePen, orangeBrush, startX, startY, endX, endY, false));
        }

        //public void DrawWarpDisplayItems()
        //{
        //    if (rom != null)
        //    {
        //        var warpDisplayItems = rom.WarpDisplayItems;
        //        foreach (var room in warpDisplayItems.Rooms)
        //        {
        //            AddRoomPos(room.X, room.Y, room.IsBad, room.IsPreview, room.IsUncertain);
        //        }
        //        foreach (var line in warpDisplayItems.Lines)
        //        {
        //            AddLine(line.StartX, line.StartY, line.EndX, line.EndY, line.HasArrow);
        //        }
        //    }
        //}

    }

    public class MapRoomToDraw
    {
        public int X { get; set; }
        public int Y { get; set; }
        public MapSquareType SquareType { get; set; }
        public bool IsCastleB { get; set; } = false;
    }

    public class MapLineToDraw
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsHorizonal { get; set; }
        public bool IsSolid { get; set; }
    }

    public class MapTextToDraw
    {
        public string Text { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public float Size { get; set; }
    }

    public class MapElementsToDraw
    {
        public List<MapRoomToDraw> Rooms { get; set; } = new List<MapRoomToDraw>();
        public List<MapLineToDraw> Lines { get; set; } = new List<MapLineToDraw>();
        public List<MapTextToDraw> Texts { get; set; } = new List<MapTextToDraw>();
    }

    public class StartEndRoomToDraw
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsBad {  get; set; }
        public bool IsPreview { get; set; }
        public bool IsUncertain { get; set; }
        public bool IsStartRoom { get; set; }
    }

    public class SourceRoomInfo
    {
        public RoomStruct Room { get; set; }
        public ExitInfo Exit { get; set; }
        public bool IsUncertain { get; set; }
        public Dictionary<uint, byte> FlagList { get; set; }
    }

    public class WarpLineToDraw
    {
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int EndX { get; set; }
        public int EndY { get; set; }
        public bool HasArrow { get; set; }
    }

    public class WarpsToDraw
    {
        public bool IsBadWarp {  get; set; }
        public bool IsUncertainWarp { get; set; }
        public bool IsDestOutside { get; set; }
        public Dictionary<uint, byte> FlagList = new Dictionary<uint, byte>();
        public RoomStruct DestRoom { get; set; }
        public List<StartEndRoomToDraw> WarpRooms { get; set; } = new List<StartEndRoomToDraw>();
    }
}
