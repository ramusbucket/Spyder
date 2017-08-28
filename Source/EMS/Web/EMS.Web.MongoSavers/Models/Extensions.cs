namespace EMS.Web.MongoSavers.Models
{
    public static class Extensions
    {
        public static string ToProtocolString(this byte protocolNumberByte)
        {
            switch (protocolNumberByte)
            {
                case 1:
                    return "ICMP";
                case 2:
                    return "IGMP";
                case 6:
                    return "TCP";
                case 17:
                    return "UDP";
                default:
                    return "#" + protocolNumberByte.ToString();
            }
        }
    }
}
