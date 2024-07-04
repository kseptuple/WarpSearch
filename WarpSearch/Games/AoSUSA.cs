using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarpSearch.Common;

namespace WarpSearch.Games
{
    public class AoSUSA : AoS
    {
        public static List<RomPointer> DefaultSpecialRomPointers = new List<RomPointer>() { 0x85236a4, 0x85247b4, 0x852484c, 0x85248e4, 0x852497c,
            0x8524a24, 0x8524abc, 0x8524b54, 0x8524bec, 0x8524ca4, 0x8524d3c, 0x8524dd4, 0x8524e5c, 0x8524020, 0x85240b8, 0x8524148, 0x85241e0, 
            0x8524278, 0x8524310, 0x85243a8, 0x8524440, 0x85244d8, 0x8524570, 0x8524608, 0x8524690 };

        public AoSUSA(byte[] fileData) : base(fileData)
        {
            //RoomRootPointer = 0x8001990;
            FirstRoomPointer = 0x850EF08;
            MapPointer = 0x8116650;
            MapLinePointer = 0x8117DD0;
            specialRomPointers = DefaultSpecialRomPointers;
        }
    }
}
