using EMS.Core.Models;
using EMS.Infrastructure.Stream;
using EMS.Web.Common.Mongo;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Net;
using System.Threading;

namespace EMS.Web.Worker.MongoSaver.Models
{
    public class NetworkPacketsSaver : BaseMongoSaver<CapturedNetworkPacket, CapturedNetworkPacketDetailsDTO>
    {
        public NetworkPacketsSaver(
            CancellationToken cToken, 
            IMongoCollection<CapturedNetworkPacket> mongoCollection) 
            : base(cToken, mongoCollection, Topics.NetworkPackets)
        {
        }

        protected override CapturedNetworkPacket FormatReceivedMessage(CapturedNetworkPacketDetailsDTO message)
        {
            var packet = message.NetworkPacket;
            var protocol = packet.Skip(9).First().ToProtocolString();
            var hostAddress = new IPAddress(BitConverter.ToUInt32(packet, 12)).ToString();
            var hostPort = ((ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet, 20)));
            var destinationAddress = new IPAddress(BitConverter.ToUInt32(packet, 16)).ToString();
            var destinationPort = ((ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet, 22)));

            var item = new CapturedNetworkPacket
            {
                UserId = message.UserId,
                CreatedOn = message.CreatedOn,
                Protocol = protocol,
                HostAddress = hostAddress,
                HostPort = hostPort,
                DestinationAddress = destinationAddress,
                DestinationPort = destinationPort
            };

            return item;
        }
    }
}