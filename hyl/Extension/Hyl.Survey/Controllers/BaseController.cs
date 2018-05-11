using System;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Mvc;
using Hyl.Survey.Infrastructure;
using ZXing;
using ZXing.Common;

namespace Hyl.Survey.Controllers
{
    public class BaseController : Controller
    {
        public ActionResult QrCode(string qrValue, int h = 250, int w = 250, int m = 0, bool download = false)
        {
            using (var bitmap = QrHelper.GetBitmap(qrValue, h, w, m))
            {
                using (var stream = new MemoryStream())
                {
                    bitmap.Save(stream, ImageFormat.Gif);
                    if (download)
                    {
                        return File(stream.ToArray(), "image/jpeg", DateTime.Now.Ticks + ".jpg");
                    }
                    return File(stream.ToArray(), "image/jpeg");
                }
            }
        }
    }
}