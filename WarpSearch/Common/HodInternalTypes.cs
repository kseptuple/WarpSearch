using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WarpSearch.Common
{
    [StructLayout(LayoutKind.Sequential)]
    public struct HodRoomStruct
    {
        [MarshalAs(UnmanagedType.U2)]
        public ushort LcdControl;
        [MarshalAs(UnmanagedType.U2)]
        public ushort EventFlag;
        [MarshalAs(UnmanagedType.U4)]
        public RomPointer NextRoom;
        [MarshalAs(UnmanagedType.U4)]
        public RomPointer Layers;
        [MarshalAs(UnmanagedType.U4)]
        public RomPointer Graphics;
        [MarshalAs(UnmanagedType.U4)]
        public RomPointer ObjectGraphics;
        [MarshalAs(UnmanagedType.U4)]
        public RomPointer Palettes;
        [MarshalAs(UnmanagedType.U4)]
        public RomPointer Objects;
        [MarshalAs(UnmanagedType.U4)]
        public RomPointer Exits;
    }
}
