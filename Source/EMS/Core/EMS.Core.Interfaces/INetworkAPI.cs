using System;
using System.Net;
using System.Net.Sockets;

namespace EMS.Core.Interfaces
{
    public interface INetworkAPI
    {
        event EventHandler<byte[]> PacketSniffed;

        void StartSniffingSingleAddress(IPAddress address, AddressFamily addressFamily = AddressFamily.InterNetwork);

        void StartSniffingAllAddresses(AddressFamily addressFamily = AddressFamily.InterNetwork);
    }
}
