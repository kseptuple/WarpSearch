using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarpSearch.Games
{
    public class HoDCustom : HoD
    {
        public HoDCustom(byte[] fileData, FormMain formMain, ROMPointer firstRoomPointer,
            ROMPointer mapPointer, ROMPointer mapLinePointer, GameVersion gameVersion) : base(fileData, formMain)
        {
            RoomRootPointer = null;
            FirstRoomPointer = firstRoomPointer;
            MapPointer = mapPointer;
            MapLinePointer = mapLinePointer;
            if (gameVersion == GameVersion.USA)
            {
                specialRomPointers = new List<ROMPointer>() { 0x84afb74, 0x84b0304, 0x84afe1c, 0x84aff84, 0x84b0230, 0x84b0378 };
            }
            else if (gameVersion == GameVersion.JPN)
            {
                specialRomPointers = new List<ROMPointer>() { 0x84b03d4, 0x84b0b64, 0x84b0b64, 0x84b07e4, 0x84b0a90, 0x84b0bd8 };
            }
            
        }
    }
}
