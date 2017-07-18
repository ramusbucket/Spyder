using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace EMS.Infrastructure.Common.Utils
{
    public static class Converter
    {
        public static byte[] ImageToByteArray(Image image)
        {
            var stream = new MemoryStream();
            var format = ImageFormat.Jpeg;
            image.Save(stream, format);

            return stream.ToArray();
        }

        public static Image ImageFromByteArray(byte[] imageAsByteArray)
        {
            var stream = new MemoryStream(imageAsByteArray);
            var image = Image.FromStream(stream);

            return image;
        }
    }
}
