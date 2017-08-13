using System;
using System.Runtime.InteropServices;
using System.Security;

namespace EMS.Desktop.Client.Helpers
{
    public static class PasswordBoxHelper
    {
        public static string DecryptSecureString(this SecureString secureString)
        {
            var insecureStringPointer = IntPtr.Zero;
            var insecureString = string.Empty;
            var gcHandler = GCHandle.Alloc(insecureString, GCHandleType.Pinned);

            try
            {
                insecureStringPointer = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                insecureString = Marshal.PtrToStringUni(insecureStringPointer);

                return insecureString;
            }
            finally
            {
                insecureString = null;

                gcHandler.Free();
                Marshal.ZeroFreeGlobalAllocUnicode(insecureStringPointer);
            }
        }
    }
}
