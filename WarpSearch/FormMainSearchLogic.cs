using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WarpSearch.Common;

namespace WarpSearch
{
    public partial class FormMain
    {
        public void FindAndDrawWarpDestination(bool previewOnly = false)
        {
            ClearPos(previewOnly);
            if (!previewOnly)
            {
                ClearLine();
            }

            int sourceX = 0, sourceY = 0, DestX = 0, DestY = 0;
            var warpResult = rom.FindWarpDestination(selectedRoom, selectedPos);
            if (warpResult?.WarpRooms?.Count > 0)
            {
                for (int i = 0; i < warpResult.WarpRooms.Count; i++)
                {
                    var roomToDraw = warpResult.WarpRooms[i];
                    if (roomToDraw.IsStartRoom)
                    {
                        sourceX = roomToDraw.X;
                        sourceY = roomToDraw.Y;
                        AddMapSquarePos(roomToDraw.X, roomToDraw.Y, warpResult.IsBadWarp, previewOnly, warpResult.IsUncertainWarp);
                    }
                    else
                    {
                        if (!previewOnly && ! warpResult.IsBadWarp)
                        {
                            DestX = roomToDraw.X;
                            DestY = roomToDraw.Y;
                            AddMapSquarePos(roomToDraw.X, roomToDraw.Y, warpResult.IsBadWarp, previewOnly, warpResult.IsUncertainWarp);
                        }
                    }
                }

                if (!previewOnly)
                {
                    if (warpResult.IsBadWarp)
                    {
                        sourceRoom = null;
                        destRoom = null;
                    }
                    else
                    {
                        sourceRoom = selectedRoom;
                        destRoom = warpResult.DestRoom;
                        SetFlagListForDestSearch(warpResult.FlagList);

                        //画线
                        if (sourceX != DestX || sourceY != DestY)
                        {
                            AddLine(sourceX, sourceY, DestX, DestY, true);
                        }
                        if (warpResult.IsDestOutside)
                        {
                            AddLine(destRoom.Left, destRoom.Top, DestX, DestY, false);
                        }
                    }
                }
            }
        }

        public void FindAndDrawWarpSource(int searchLevel)
        {
            ClearPos();
            ClearLine();
            ClearSourceRoomList();
            var sourceRooms = rom.FindWarpSource(selectedRoom, searchLevel);

            for (int i = 0; i < sourceRooms.Count; i++)
            {
                var sourceRoom = sourceRooms[i];
                AddSourceRoom(sourceRoom.Room, sourceRoom.Exit, sourceRoom.IsUncertain, sourceRoom.FlagList);
            }
        }
    }
}
