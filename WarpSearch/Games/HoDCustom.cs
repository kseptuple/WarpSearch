using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarpSearch.Common;

namespace WarpSearch.Games
{
    public class HoDCustom : HoD
    {
        public HoDCustom(byte[] fileData, FormMain formMain, RomPointer firstRoomPointer,
            RomPointer mapPointer, RomPointer mapLinePointer, GameVersionEnum gameVersion) : base(fileData, formMain)
        {
            //RoomRootPointer = null;
            FirstRoomPointer = firstRoomPointer;
            MapPointer = mapPointer;
            MapLinePointer = mapLinePointer;
            if (gameVersion == GameVersionEnum.USA)
            {
                specialRomPointers = HoDUSA.DefaultSpecialRomPointers;
            }
            else if (gameVersion == GameVersionEnum.EUR)
            {
                specialRomPointers = HoDEUR.DefaultSpecialRomPointers;
            }
            else if (gameVersion == GameVersionEnum.JPN)
            {
                specialRomPointers = HoDJPN.DefaultSpecialRomPointers;
            }
            
        }
    }
}
