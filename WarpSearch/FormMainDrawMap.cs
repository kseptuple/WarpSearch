using System;
using System.Collections.Generic;
using System.Drawing;
using WarpSearch.Common;

namespace WarpSearch
{
    public partial class FormMain
    {
        private readonly Brush blueBrush = new SolidBrush(Color.FromArgb(0, 0, 224));
        private readonly Brush blackBrush = new SolidBrush(Color.FromArgb(0, 0, 0));
        private readonly Brush cyanBrush = new SolidBrush(Color.FromArgb(0, 200, 200));
        private readonly Brush redBrush = new SolidBrush(Color.FromArgb(240, 0, 0));
        private readonly Brush yellowBrush = new SolidBrush(Color.FromArgb(248, 248, 8));
        private readonly Brush whiteBrush = new SolidBrush(Color.FromArgb(248, 248, 248));
        private readonly Brush transparentWhiteBrush = new SolidBrush(Color.FromArgb(192, 255, 255, 255));
        private readonly Brush transparentGreenBrush = new SolidBrush(Color.FromArgb(127, 0, 255, 0));
        private readonly Brush transparentBlueBrush = new SolidBrush(Color.FromArgb(127, 0, 0, 255));
        private readonly Brush transparentRedBrush = new SolidBrush(Color.FromArgb(127, 255, 0, 0));
        private readonly Brush transparentOrangeBrush = new SolidBrush(Color.FromArgb(192, 255, 127, 0));
        private readonly Brush transparentBlackBrush = new SolidBrush(Color.FromArgb(127, 0, 0, 0));
        private readonly Brush trueWhiteBrush = new SolidBrush(Color.FromArgb(255, 255, 255));
        private readonly Brush greenBrush = new SolidBrush(Color.FromArgb(0, 224, 0));

        private readonly Pen redPen = new Pen(Color.FromArgb(192, 0, 0));
        private readonly Brush redBrush2 = new SolidBrush(Color.FromArgb(192, 0, 0));
        private readonly Pen orangePen = new Pen(Color.FromArgb(255, 127, 0));
        private readonly Brush orangeBrush = new SolidBrush(Color.FromArgb(255, 127, 0));

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
                SquarePreviewsToDraw.Add(new RectangleToDraw(brush, x, y, 1, 1));
            }
            else
            {
                SquaresToDraw.Add(new RectangleToDraw(brush, x, y, 1, 1));
            }
        }

        public void AddLine(int startX, int startY, int endX, int endY, bool hasArrow)
        {
            if (hasArrow)
                LinesToDraw.Add(new LineToDraw(redPen, redBrush2, startX, startY, endX, endY, true));
            else
                LinesToDraw.Add(new LineToDraw(orangePen, orangeBrush, startX, startY, endX, endY, false));
        }

        public void AddRoomRectangleToDraw(RoomStruct room, bool isBlack)
        {
            var brush = isBlack ? transparentBlackBrush : transparentWhiteBrush;
            RectanglesToDraw.Add(new RectangleToDraw(brush, room.Left, room.Top, room.Width, room.Height));
        }

        public void ClearRooms()
        {
            RectanglesToDraw.Clear();
        }
        public void ClearPos(bool previewOnly = false)
        {
            SquarePreviewsToDraw.Clear();
            if (!previewOnly)
            {
                SquaresToDraw.Clear();
            }
        }
        public void ClearLine()
        {
            LinesToDraw.Clear();
        }
        public void ClearSourceRoomList()
        {
            listSourceRoom.Items.Clear();
            SourceRoomPointers.Clear();
            if (RectanglesToDraw.Count > 1)
            {
                RectanglesToDraw.RemoveRange(1, RectanglesToDraw.Count - 1);
            }
        }
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
        public bool IsBad { get; set; }
        public bool IsPreview { get; set; }
        public bool IsUncertain { get; set; }
        public bool IsStartRoom { get; set; }
    }

    public class SourceRoomInfo
    {
        public RoomStruct Room { get; set; }
        public ExitInfo Exit { get; set; }
        public bool IsUncertain { get; set; }
        public bool IsDestOutside { get; set; }
        public Dictionary<uint, byte> FlagList { get; set; }
        public WarpTechInfo TechInfo { get; set; }
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
        public bool IsBadWarp { get; set; }
        public bool IsUncertainWarp { get; set; }
        public bool IsDestOutside { get; set; }
        public Dictionary<uint, byte> FlagList = new Dictionary<uint, byte>();
        public RoomStruct DestRoom { get; set; }
        public List<StartEndRoomToDraw> WarpRooms { get; set; } = new List<StartEndRoomToDraw>();
        public WarpTechInfo TechInfo { get; set; }
    }

    public class WarpTechInfo
    {
        public RomPointer ExitPointerStart { get; set; }
        public int ExitIndex { get; set; }
        public int StartX { get; set; }
        public int StartY { get; set; }
        public bool IsLoop { get; set; } = false;
        public bool IsOutOfBound { get; set; } = false;
        public bool IsNormalExit { get; set; }
        public List<WarpTechInfoRooms> DestRooms { get; set; }
    }

    public class WarpTechInfoRooms
    {
        public RomPointer RoomPointer { get; set; }
        public uint Flag { get; set; }
        public bool IsInvalidRoom { get; set; } = false;
        public bool IsOutOfBoundRoom { get; set; } = false;
    }

    public class RectangleToDraw
    {
        public Brush Brush { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public RectangleToDraw(Brush brush, int left, int top, int width, int height)
        {
            Brush = brush;
            Top = top;
            Left = left;
            Width = width;
            Height = height;
        }

        public void Draw(Graphics g, int offset, float gridSize)
        {
            g?.FillRectangle(Brush, (Left + offset) * gridSize, (Top + offset) * gridSize, Width * gridSize, Height * gridSize);
        }
    }

    public class LineToDraw
    {
        public Pen Pen { get; set; }
        public Brush Brush { get; set; }
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int EndX { get; set; }
        public int EndY { get; set; }
        public bool HasArrow { get; set; }

        private static ArrowSize arrowSize1 = new ArrowSize() { Angle = Math.PI / 4, LengthFactor = (float)(1 / Math.Cos(Math.PI / 4)) };
        private static ArrowSize arrowSize2 = new ArrowSize() { Angle = Math.PI / 6, LengthFactor = (float)(1 / Math.Cos(Math.PI / 6)) };
        private static ArrowSize arrowSize3 = new ArrowSize() { Angle = Math.PI / 8, LengthFactor = (float)(1 / Math.Cos(Math.PI / 8)) };
        private static ArrowSize arrowSize4 = new ArrowSize() { Angle = Math.PI / 12, LengthFactor = (float)(1 / Math.Cos(Math.PI / 12)) };

        public LineToDraw(Pen pen, Brush brush, int startX, int startY, int endX, int endY, bool hasArrow)
        {
            Pen = pen;
            Brush = brush;
            StartX = startX;
            StartY = startY;
            EndX = endX;
            EndY = endY;
            HasArrow = hasArrow;
        }

        public void Draw(Graphics g, int offset, float gridSize)
        {
            var angle = Math.Atan2(EndY - StartY, EndX - StartX);
            var xDiff = (float)(0.5f * Math.Cos(angle));
            var yDiff = (float)(0.5f * Math.Sin(angle));
            PointF pointStart = new PointF((StartX + offset + 0.5f) * gridSize, (StartY + offset + 0.5f) * gridSize);
            PointF pointEnd = default;
            if (HasArrow)
            {
                pointEnd = new PointF((EndX + offset + 0.5f - xDiff) * gridSize, (EndY + offset + 0.5f - yDiff) * gridSize);
            }
            else
            {
                pointEnd = new PointF((EndX + offset + 0.5f) * gridSize, (EndY + offset + 0.5f) * gridSize);
            }

            g?.DrawLine(Pen, pointStart, pointEnd);

            if (HasArrow)
            {
                var pointEndCenter = new PointF((EndX + offset + 0.5f) * gridSize, (EndY + offset + 0.5f) * gridSize);
                var maxLength = ((EndY - StartY) * (EndY - StartY) + (EndX - StartX) * (EndX - StartX)) / 4f;
                //length: 箭头两边的长度
                //maxLength: 箭头中轴允许的最大长度的平方
                var length = 0f;
                var angle1 = 0d;
                var angle2 = 0d;
                ArrowSize arrowSize = null;
                length = (float)Math.Sqrt(maxLength);
                if (maxLength < 0.5f)
                {
                    arrowSize = arrowSize1;
                }
                else if (maxLength < 2f)
                {
                    arrowSize = arrowSize2;
                }
                else if (maxLength < 3f)
                {
                    arrowSize = arrowSize3;
                }
                else
                {
                    arrowSize = arrowSize4;
                }
                length *= arrowSize.LengthFactor;
                angle1 = angle + arrowSize.Angle;
                angle2 = angle - arrowSize.Angle;
                if (length > 2f)
                    length = 2f;

                var point1 = new PointF((EndX + offset + 0.5f - (float)Math.Cos(angle1) * length) * gridSize, (EndY + offset + 0.5f - (float)Math.Sin(angle1) * length) * gridSize);
                var point2 = new PointF((EndX + offset + 0.5f - (float)Math.Cos(angle2) * length) * gridSize, (EndY + offset + 0.5f - (float)Math.Sin(angle2) * length) * gridSize);
                g?.FillPolygon(Brush, new PointF[] { pointEndCenter, point1, point2 });
            }
        }

        class ArrowSize
        {
            public float LengthFactor { get; set; }
            public double Angle { get; set; }
        }
    }
}
