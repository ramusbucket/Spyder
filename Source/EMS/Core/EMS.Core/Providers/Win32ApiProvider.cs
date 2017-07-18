using System;
using System.Runtime.InteropServices;
using EMS.Core.Interfaces.Providers;

namespace EMS.Core.Providers
{
    public class Win32ApiProvider : IWin32ApiProvider
    {
        private const string User32 = "user32.dll";

        public IntPtr GetForegroundWindowManaged()
        {
            return GetForegroundWindow();
        }

        public int GetWindowThreadProcessIdManaged(IntPtr hWnd, out uint lpdwProcessId)
        {
            return GetWindowThreadProcessId(hWnd, out lpdwProcessId);
        }

        public int GetKeyState(int index)
        {
            return GetAsyncKeyState(index);
        }

        /// <summary>
        /// Gets the currently active window 
        /// with which the user is currently working.
        /// </summary>
        /// <returns>Returns a handle to the currently active foreground window.</returns>
        [DllImport(User32)]
        private static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// The "GetWindowThreadProcessId" function retrieves the identifier of the thread
        /// that created the specified window and, optionally, the identifier of the
        /// process that created the window.
        /// </summary>
        /// <returns></returns>
        [DllImport(User32)]
        private static extern int GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport(User32)]
        private static extern int GetAsyncKeyState(Int32 i);
    }
}
