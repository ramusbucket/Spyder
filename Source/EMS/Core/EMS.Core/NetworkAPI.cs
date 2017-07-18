using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using EMS.Core.Interfaces;

namespace EMS.Core
{
    public class NetworkAPI : INetworkAPI
    {
        public event EventHandler<byte[]> OnPacketSniffed;

        public void StartSniffingSingleAddress(IPAddress address, AddressFamily addressFamily = AddressFamily.InterNetwork)
        {
            var socket = CreateAndConfigureSocket(address, addressFamily);
            var buffer = CreateBuffer();

            StartReceivingPackets(socket, buffer);
        }

        public void StartSniffingAllAddresses(AddressFamily addressFamily = AddressFamily.InterNetwork)
        {
            var hostName = Dns.GetHostName();
            var hostEntry = Dns.GetHostEntry(hostName);
            var addresses = hostEntry.AddressList
                .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)
                .ToList();

            foreach (var address in addresses)
            {
                StartSniffingSingleAddress(address, addressFamily);
            }
        }

        /// <summary>
        /// Returns a byte array to hold the packet data we want to examine.
        /// We are assuming default size of 24 bytes (20 bytes for IP header + 4 bytes for TCP header to include ports).
        /// </summary>
        /// <param name="bufferSize">
        /// The size of the buffer which will receive the data.
        /// </param>
        /// <returns>Returns a byte array to hold the packet data we want to examine.</returns>
        private byte[] CreateBuffer(int bufferSize = 24)
        {
            return new byte[bufferSize];
        }

        /// <summary>
        /// Create and setup a socket to listen on.
        /// By default we are listening to IPv4 addresses only.
        /// </summary>
        /// <param name="address">Address to sniff.</param>
        /// <param name="addressFamily">Address family.</param>
        /// <param name="endpointPort">Endpoint port.</param>
        /// <returns>The newly created and configured socket.</returns>
        private Socket CreateAndConfigureSocket(IPAddress address, AddressFamily addressFamily, int endpointPort = 0)
        {
            var socket = new Socket(
               addressFamily,
               SocketType.Raw,
               ProtocolType.IP);

            var endpoint = new IPEndPoint(
                address,
                endpointPort);

            socket.Bind(endpoint);

            socket.SetSocketOption(
                SocketOptionLevel.IP,
                SocketOptionName.HeaderIncluded,
                optionValue: true);

            socket.IOControl(
                IOControlCode.ReceiveAll,
                optionInValue: new byte[4] { 1, 0, 0, 0 },
                optionOutValue: null);

            return socket;
        }

        private void StartReceivingPackets(Socket socket, byte[] buffer)
        {
            var onReceive = (Action<IAsyncResult>)null;

            onReceive = (asyncResult) =>
            {
                OnPacketSniffed.Invoke(this, buffer);

                buffer = CreateBuffer();
                socket.BeginReceive(
                    buffer,
                    offset: 0,
                    size: 24,
                    socketFlags: SocketFlags.None,
                    callback: new AsyncCallback(onReceive),
                    state: null);
            };

            socket.BeginReceive(
                buffer: buffer,
                offset: 0,
                size: buffer.Length,
                socketFlags: SocketFlags.None,
                callback: new AsyncCallback(onReceive),
                state: null);
        }
    }
}
