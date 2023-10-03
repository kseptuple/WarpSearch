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
            //RoomRootPointer = null;
            FirstRoomPointer = firstRoomPointer;
            MapPointer = mapPointer;
            MapLinePointer = mapLinePointer;
            if (gameVersion == GameVersion.USA)
            {
                specialRomPointers = HoDUSA.DefaultSpecialRomPointers;
            }
            else if (gameVersion == GameVersion.EUR)
            {
                specialRomPointers = HoDEUR.DefaultSpecialRomPointers;
            }
            else if (gameVersion == GameVersion.JPN)
            {
                specialRomPointers = HoDJPN.DefaultSpecialRomPointers;
            }
            
        }
    }
}
