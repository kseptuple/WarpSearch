using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarpSearch.Common;

namespace WarpSearch.Games
{
    public class HoDUSA : HoD
    {
        public static List<RomPointer> DefaultSpecialRomPointers = new List<RomPointer>() { 0x84afbbc, 0x84b0304, 0x84afe1c, 0x84aff84, 0x84b0230, 0x84b0378 };

        public HoDUSA(byte[] fileData) : base(fileData)
        {
            //RoomRootPointer = 0x8001EC8;
            FirstRoomPointer = 0x8494D48;
            MapPointer = 0x80DAD94;
            MapLinePointer = 0x80DC194;
            specialRomPointers = DefaultSpecialRomPointers;
        }
    }
}
