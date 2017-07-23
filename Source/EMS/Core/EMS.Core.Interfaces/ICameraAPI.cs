using System;
using System.Collections.Generic;
using EMS.Core.Models;

namespace EMS.Core.Interfaces
{
    public interface ICameraApi
    {
        event EventHandler<byte[]> OnWebcamSnapshotTaken;

        IList<WebcamDetails> GetAvailableWebcams();

        void StartCamera(string cameraId);

        void StopCamera(string cameraId);
    }
}
