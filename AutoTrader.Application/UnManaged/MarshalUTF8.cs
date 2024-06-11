using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace AutoTrader.Application.UnManaged
{
    public static class MarshalUTF8
    {
        private static UTF8Encoding _utf8;

        //--------------------------------------------------------------------------------
        static MarshalUTF8()
        {
            _utf8 = new UTF8Encoding();
        }

        //--------------------------------------------------------------------------------
        public static IntPtr StringToHGlobalUTF8(string data)
        {
            byte[] dataEncoded = _utf8.GetBytes(data + "\0");

            int size = Marshal.SizeOf(dataEncoded[0]) * dataEncoded.Length;

            IntPtr pData = Marshal.AllocHGlobal(size);

            Marshal.Copy(dataEncoded, 0, pData, dataEncoded.Length);

            return pData;
        }

        //--------------------------------------------------------------------------------
        public static string PtrToStringUTF8(IntPtr pData)
        {
            // this is just to get buffer length in bytes
            string errStr = Marshal.PtrToStringAnsi(pData);
            int length = errStr.Length;

            byte[] data = new byte[length];
            Marshal.Copy(pData, data, 0, length);

            return _utf8.GetString(data);
        }
        //--------------------------------------------------------------------------------
    }

}
