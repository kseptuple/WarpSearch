using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarpSearch.Games
{
    public class HoDJPN : HoD
    {
        public HoDJPN(byte[] fileData, FormMain formMain) : base(fileData, formMain)
        {
            RoomRootPointer = 0x8001EC0;
            FirstRoomPointer = 0x848C3C8;
            MapPointer = 0x80D24A4;
            MapLinePointer = 0x80D38A4;
            specialRomPointers = new List<ROMPointer>() { 0x84b041c, 0x84b0b64, 0x84b067c, 0x84b07e4, 0x84b0a90, 0x84b0bd8 };
        }
    }
}
