using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarpSearch.Games
{
    public class HoDEUR : HoD
    {
        public static List<ROMPointer> DefaultSpecialRomPointers = new List<ROMPointer>() { 0x84afbbc, 0x84b0304, 0x84afe1c, 0x84aff84, 0x84b0230, 0x84b0378 };

        public HoDEUR(byte[] fileData, FormMain formMain) : base(fileData, formMain)
        {
            //RoomRootPointer = 0x8001EC8;
            FirstRoomPointer = 0x8494D48;
            MapPointer = 0x80DAD94;
            MapLinePointer = 0x80DC194;
            specialRomPointers = DefaultSpecialRomPointers;
        }
    }
}
