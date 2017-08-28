using System;
using System.Linq;
using System.Net;
using System.Threading;
using EMS.Core.Models.DTOs;
using EMS.Core.Models.Mongo;
using EMS.Infrastructure.Stream;
using MongoDB.Driver;

namespace EMS.Web.MongoSavers.Models.Savers
{
    public class NetworkPacketsSaver : BaseMongoSaver<CapturedNetworkPacketMongoDocument, CapturedNetworkPacketDetailsDto>
    {
        public NetworkPacketsSaver(
            CancellationToken cToken, 
            IMongoCollection<CapturedNetworkPacketMongoDocument> outCollection) 
            : base(cToken, outCollection, Topics.NetworkPackets)
        {
        }

        protected override CapturedNetworkPacketMongoDocument FormatReceivedMessage(CapturedNetworkPacketDetailsDto message)
        {
            var packet = message.NetworkPacket;
            var protocol = packet.Skip(9).First().ToProtocolString();
            var hostAddress = new IPAddress(BitConverter.ToUInt32(packet, 12)).ToString();
            var hostPort = ((ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet, 20)));
            var destinationAddress = new IPAddress(BitConverter.ToUInt32(packet, 16)).ToString();
            var destinationPort = ((ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(packet, 22)));

            var item = new CapturedNetworkPacketMongoDocument
            {
                UserId = message.UserId,
                CreatedOn = message.CreatedOn,
                Protocol = protocol,
                HostAddress = hostAddress,
                HostPort = hostPort,
                DestinationAddress = destinationAddress,
                DestinationPort = destinationPort,
                SessionId = message.SessionId
            };

            return item;
        }
    }
}