using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarpSearch.Games
{
    public class AoSCustom : AoS
    {
        public AoSCustom(byte[] fileData, FormMain formMain, ROMPointer firstRoomPointer,
            ROMPointer mapPointer, ROMPointer mapLinePointer, GameVersion gameVersion) : base(fileData, formMain)
        {
            RoomRootPointer = null;
            FirstRoomPointer = firstRoomPointer;
            MapPointer = mapPointer;
            MapLinePointer = mapLinePointer;
            if (gameVersion == GameVersion.USA)
            {
                specialRomPointers = new List<ROMPointer>() { 0x85236a4, 0x85247b4, 0x852484c, 0x85248e4, 0x852497c, 0x8524a24, 0x8524abc, 0x8524b54,
                0x8524bec, 0x8524ca4, 0x8524d3c, 0x8524dd4, 0x8524e5c, 0x8524020, 0x85240b8, 0x8524148, 0x85241e0, 0x8524278, 0x8524310, 0x85243a8,
                0x8524440, 0x85244d8, 0x8524570, 0x8524608, 0x8524690 };
            }
            else if (gameVersion == GameVersion.JPN)
            {
                specialRomPointers = new List<ROMPointer>() { 0x84f9fa4, 0x84fb0b4, 0x84fb14c, 0x84fb1e4, 0x84fb27c, 0x84fb324, 0x84fb3bc, 0x84fb454,
                0x84fb4ec, 0x84fb5a4, 0x84fb63c, 0x84fb6d4, 0x84fb75c, 0x84fa920, 0x84fa9b8, 0x84faa48, 0x84faae0, 0x84fab78, 0x84fac10, 0x84faca8,
                0x84fad40, 0x84fadd8, 0x84fae70, 0x84faf08, 0x84faf90 };
            }
        }
    }
}
