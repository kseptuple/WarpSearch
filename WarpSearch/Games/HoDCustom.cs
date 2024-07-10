using WarpSearch.Common;

namespace WarpSearch.Games
{
    public class HoDCustom : HoD
    {
        public HoDCustom(byte[] fileData, RomPointer firstRoomPointer,
            RomPointer mapPointer, RomPointer mapLinePointer, GameVersionEnum gameVersion) : base(fileData)
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
