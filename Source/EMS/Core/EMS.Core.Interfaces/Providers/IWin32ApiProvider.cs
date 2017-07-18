using System;

namespace EMS.Core.Interfaces.Providers
{
    public interface IWin32ApiProvider
    {
        IntPtr GetForegroundWindowManaged();

        int GetWindowThreadProcessIdManaged(IntPtr hWnd, out uint lpdwProcessId);

        int GetKeyState(int index);
    }
}
