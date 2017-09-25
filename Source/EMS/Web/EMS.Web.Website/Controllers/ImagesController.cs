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

        private readonly IMongoCollection<CapturedDisplaySnapshotMongoDocument> _displaySnapshots;


        public ImagesController()
        {
            _cameraSnapshots = UnityInjector.Instance
                .Resolve<IMongoCollection<CapturedCameraSnapshotMongoDocument>>();

            _displaySnapshots = UnityInjector.Instance
                .Resolve<IMongoCollection<CapturedDisplaySnapshotMongoDocument>>();
        }

        public async Task<ActionResult> GetCameraSnapshot(string cameraSnapshotId)
        {
            var id = new ObjectId(cameraSnapshotId);
            var imageData = await _cameraSnapshots
                .Find(x => x.Id == id)
                .Project(x => x.CameraSnapshot)
                .FirstOrDefaultAsync();

            return File(imageData, $"CameraSnapshot{cameraSnapshotId}/png"); 
        }

        public async Task<ActionResult> GetDisplaySnapshot(string displaySnapshotId)
        {
            var id = new ObjectId(displaySnapshotId);
            var imageData = await _displaySnapshots
                .Find(x => x.Id == id)
                .Project(x => x.DisplaySnapshot)
                .FirstOrDefaultAsync();

            return File(imageData, $"DisplaySnapshot{displaySnapshotId}/png");
        }
    }
}