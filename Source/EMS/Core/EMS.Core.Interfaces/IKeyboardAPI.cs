using System;
using System.Threading.Tasks;
using EMS.Infrastructure.Common.Enums;

namespace EMS.Core.Interfaces
{
    public interface IKeyboardApi
    {
        event EventHandler<KeyboardKey> OnKeyPressed;

        void StartListeningToKeyboard();

        void StopListeningToKeyboard();
    }
}
