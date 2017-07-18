using System;
using System.Net;
using System.Net.Sockets;

namespace EMS.Core.Interfaces
{
    public interface INetworkApi
    {
        event EventHandler<byte[]> OnPacketSniffed;

        void StartSniffingSingleAddress(IPAddress address, AddressFamily addressFamily = AddressFamily.InterNetwork);

        void StartSniffingAllAddresses(AddressFamily addressFamily = AddressFamily.InterNetwork);
    }
}
