using System;

namespace EMS.Core.Interfaces
{
    public interface ICameraApi
    {
        event EventHandler<byte[]> OnWebcamSnapshotTaken;

        bool IsWebcamAvailable();

        void StartCamera(uint index = 0);

        void StopCamera();
    }
}
