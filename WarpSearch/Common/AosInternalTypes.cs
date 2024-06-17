using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WarpSearch.Common
{
    [StructLayout(LayoutKind.Sequential)]
    public struct AosRoomStruct
    {
        [MarshalAs(UnmanagedType.U2)]
        public ushort LcdControl;
        [MarshalAs(UnmanagedType.U2)]
        public ushort EventFlag;
        [MarshalAs(UnmanagedType.U4)]
        public uint nextRoom;
        [MarshalAs(UnmanagedType.U4)]
        public uint layers;
        [MarshalAs(UnmanagedType.U4)]
        public uint graphics;
        [MarshalAs(UnmanagedType.U4)]
        public uint palettes;
        [MarshalAs(UnmanagedType.U4)]
        public uint objects;
        [MarshalAs(UnmanagedType.U4)]
        public uint exits;


        public RomPointer NextRoom => nextRoom;
        public RomPointer Layers => layers;
        public RomPointer Graphics => graphics;
        public RomPointer Palettes => palettes;
        public RomPointer Objects => objects;
        public RomPointer Exits => exits;
    }
}
