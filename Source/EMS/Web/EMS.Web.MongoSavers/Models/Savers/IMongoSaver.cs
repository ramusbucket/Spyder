using System.Threading.Tasks;

namespace EMS.Web.MongoSavers.Models.Savers
{
    public interface IMongoSaver
    {
        Task Start();

        ServiceStatistics Statistics { get; }
    }
}
