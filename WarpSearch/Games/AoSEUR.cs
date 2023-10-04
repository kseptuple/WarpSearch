using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarpSearch.Games
{
    public class AoSEUR : AoS
    {
        public static List<ROMPointer> DefaultSpecialRomPointers = new List<ROMPointer>() { 0x85236fc, 0x852480c, 0x85248a4, 0x852493c, 0x85249d4,
            0x8524a7c, 0x8524b14, 0x8524bac, 0x8524c44, 0x8524cfc, 0x8524d94, 0x8524e2c, 0x8524eb4, 0x8524078, 0x8524110, 0x85241a0, 0x8524238, 
            0x85242d0, 0x8524368, 0x8524400, 0x8524498, 0x8524530, 0x85245c8, 0x8524660, 0x85246e8 };

        public AoSEUR(byte[] fileData, FormMain formMain) : base(fileData, formMain)
        {
            //RoomRootPointer = 0x8001990;
            FirstRoomPointer = 0x850EF60;
            MapPointer = 0x8116664;
            MapLinePointer = 0x8117DE4;
            specialRomPointers = DefaultSpecialRomPointers;
        }
    }
}
