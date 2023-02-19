using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarpSearch.Games
{
    public class AoSJPN : AoS
    {
        public AoSJPN(byte[] fileData, FormMain formMain) : base(fileData, formMain)
        {
            RoomRootPointer = 0x800198C;
            FirstRoomPointer = 0x84E5808;
            MapPointer = 0x80F58D8;
            MapLinePointer = 0x80F7058;
            specialRomPointers = new List<ROMPointer>() { 0x84f9fa4, 0x84fb0b4, 0x84fb14c, 0x84fb1e4, 0x84fb27c, 0x84fb324, 0x84fb3bc, 0x84fb454,
                0x84fb4ec, 0x84fb5a4, 0x84fb63c, 0x84fb6d4, 0x84fb75c, 0x84fa920, 0x84fa9b8, 0x84faa48, 0x84faae0, 0x84fab78, 0x84fac10, 0x84faca8,
                0x84fad40, 0x84fadd8, 0x84fae70, 0x84faf08, 0x84faf90 };
        }
    }
}
