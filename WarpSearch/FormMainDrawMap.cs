using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        public List<MapRoomToDraw> Rooms { get; set; }
        public List<MapLineToDraw> Lines { get; set; }
        public List<MapTextToDraw> Texts { get; set; }
    }
}
