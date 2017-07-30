using System;

namespace EMS.Core.Interfaces
{
    public interface IDisplayApi
    {
        event EventHandler<byte[]> OnDisplaySnapshotTaken;

        void StartWatchingDisplay();

        void StopWatchingDisplay();
    }
}
