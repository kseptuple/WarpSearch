using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarpSearch.Common;

namespace WarpSearch.Games
{
    public class AoSCustom : AoS
    {
        public AoSCustom(byte[] fileData, RomPointer firstRoomPointer,
            RomPointer mapPointer, RomPointer mapLinePointer, GameVersionEnum gameVersion) : base(fileData)
        {
            //RoomRootPointer = null;
            FirstRoomPointer = firstRoomPointer;
            MapPointer = mapPointer;
            MapLinePointer = mapLinePointer;
            if (gameVersion == GameVersionEnum.USA)
            {
                specialRomPointers = AoSUSA.DefaultSpecialRomPointers;
            }
            else if (gameVersion == GameVersionEnum.EUR)
            {
                specialRomPointers = AoSEUR.DefaultSpecialRomPointers;
            }
            else if (gameVersion == GameVersionEnum.JPN)
            {
                specialRomPointers = AoSJPN.DefaultSpecialRomPointers;
            }
        }
    }
}
