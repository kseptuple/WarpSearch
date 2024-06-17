using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WarpSearch.Helper
{
    public static class ByteArrayToStructure
    {
        private static IntPtr pointer = IntPtr.Zero;
        private static int allocMem = 0;
        private static object locker = new object();
        public static T Convert<T>(byte[] bytes, int startIndex = 0) where T : struct
        {
            var structLayout = typeof(T).StructLayoutAttribute;
            if (structLayout == null || structLayout.Value == LayoutKind.Auto)
            {
                throw new NotSupportedException();
            }
            lock (locker)
            {
                var size = Marshal.SizeOf(typeof(T));
                if (startIndex + size > bytes.Length)
                {
                    throw new IndexOutOfRangeException();
                }

                if (size > allocMem)
                {
                    if (pointer != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(pointer);
                    }
                    pointer = Marshal.AllocHGlobal(size);
                }
                Marshal.Copy(bytes, startIndex, pointer, size);
                return Marshal.PtrToStructure<T>(pointer);
            }
        }
    }
}
