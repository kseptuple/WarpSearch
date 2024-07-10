using System;

namespace WarpSearch.Common
{
    public class RomPointer : IComparable<RomPointer>
    {
        private uint actualAddress = 0;
        private uint romAddress = 0;

        public RomPointer()
        {
            romAddress = 0;
            actualAddress = 0;
        }

        public RomPointer(uint address)
        {
            romAddress = address;
            actualAddress = address - 0x8_00_00_00;
        }

        public uint RomOffset => actualAddress;

        public static implicit operator RomPointer(uint address)
        {
            return new RomPointer(address);
        }

        public static implicit operator RomPointer(int address)
        {
            return new RomPointer((uint)address);
        }

        public static implicit operator uint(RomPointer pointer)
        {
            return pointer.romAddress;
        }

        public static implicit operator int(RomPointer pointer)
        {
            unchecked
            {
                return (int)pointer.romAddress;
            }
        }

        public static bool operator ==(RomPointer pointer1, RomPointer pointer2)
        {
            if (ReferenceEquals(pointer1, pointer2)) return true;
            if (pointer1 is null || pointer2 is null) return false;
            return pointer1.actualAddress == pointer2.actualAddress;
        }

        public static bool operator !=(RomPointer pointer1, RomPointer pointer2)
        {
            return !(pointer1 == pointer2);
        }

        public static bool operator >(RomPointer pointer1, RomPointer pointer2)
        {
            if (ReferenceEquals(pointer1, pointer2)) return false;
            return pointer1.actualAddress > pointer2.actualAddress;
        }

        public static bool operator <(RomPointer pointer1, RomPointer pointer2)
        {
            if (ReferenceEquals(pointer1, pointer2)) return false;
            return pointer1.actualAddress < pointer2.actualAddress;
        }

        public static bool operator >=(RomPointer pointer1, RomPointer pointer2)
        {
            if (ReferenceEquals(pointer1, pointer2)) return true;
            if (pointer1 is null || pointer2 is null) return false;
            return pointer1.actualAddress >= pointer2.actualAddress;
        }

        public static bool operator <=(RomPointer pointer1, RomPointer pointer2)
        {
            if (ReferenceEquals(pointer1, pointer2)) return true;
            if (pointer1 is null || pointer2 is null) return false;
            return pointer1.actualAddress <= pointer2.actualAddress;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(RomPointer)) return false;
            return actualAddress == (obj as RomPointer).actualAddress;
        }

        public override int GetHashCode()
        {
            return (int)actualAddress;
        }

        public override string ToString()
        {
            return ((uint)this).ToString("X8");
        }

        public int CompareTo(RomPointer other)
        {
            return actualAddress.CompareTo(other.actualAddress);
        }
    }

}
