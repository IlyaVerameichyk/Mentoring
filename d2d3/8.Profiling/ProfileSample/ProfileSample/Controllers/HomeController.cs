using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ProfileSample.DAL;
using ProfileSample.Models;
using WebGrease.Css.Extensions;

namespace ProfileSample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var context = new ProfileSampleEntities())
            {
                var sources = context.ImgSources.Take(20);

                var model = sources.Select(item => new ImageModel()
                {
                    Name = item.Name,
                    Data = item.Data
                }).ToList();

                return View(model);
            }
        }

        private static byte[] LowerImageQuality(byte[] originBytes, int jpegQuality)
        {
            Image image;
            using (var inputStream = new MemoryStream(originBytes))
            {
                image = Image.FromStream(inputStream);
                var jpegEncoder = ImageCodecInfo.GetImageDecoders()
                    .First(c => c.FormatID == ImageFormat.Jpeg.Guid);
                var encoderParameters = new EncoderParameters(1)
                {
                    Param = { [0] = new EncoderParameter(Encoder.Quality, jpegQuality) }
                };
                using (var outputStream = new MemoryStream())
                {
                    image.Save(outputStream, jpegEncoder, encoderParameters);
                    return outputStream.ToArray();
                }
            }
        }

        public ActionResult Convert()
        {
            var files = Directory.GetFiles(Server.MapPath("~/Content/Img"), "*.jpg");

            using (var context = new ProfileSampleEntities())
            {
                foreach (var contextImgSource in context.ImgSources.ToList())
                {
                    context.ImgSources.Remove(contextImgSource);
                }
                files.ForEach(file => context.ImgSources.Add(new ImgSource()
                {
                    Name = Path.GetFileName(file),
                    Data = LowerImageQuality(System.IO.File.ReadAllBytes(file), 10)
                }));

                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}