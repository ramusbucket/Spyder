using System.Threading.Tasks;
using System.Web.Mvc;
using EMS.Core.Models.Mongo;
using EMS.Infrastructure.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EMS.Web.Website.Controllers
{
    public class ImagesController : Controller
    {
        private readonly IMongoCollection<CapturedCameraSnapshotMongoDocument> _cameraSnapshots;

        public ImagesController()
        {
            _cameraSnapshots = UnityInjector.Instance
                .Resolve<IMongoCollection<CapturedCameraSnapshotMongoDocument>>();
        }

        public async Task<ActionResult> GetCameraSnapshot(string cameraSnapshotId)
        {
            var id = new ObjectId(cameraSnapshotId);
            var imageData = await _cameraSnapshots
                .Find(x => x.Id == id)
                .Project(x => x.CameraSnapshot)
                .FirstOrDefaultAsync();

            return File(imageData, $"{cameraSnapshotId}/png"); 
        }
    }
}