using System;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Hyl.Survey.Infrastructure
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlString GenerateRelayQrCode(this HtmlHelper helper, string qrValue, int height = 250, int width = 250, int margin = 0)
        {
            using (var bitmap = QrHelper.GetBitmap(qrValue,height,width,margin))
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Gif);
                var img = new TagBuilder("img");
                img.MergeAttribute("alt", "qrcode");
                img.Attributes.Add("src", String.Format("data:image/gif;base64,{0}",Convert.ToBase64String(stream.ToArray())));
                return new HtmlString(img.ToString(TagRenderMode.SelfClosing));
            }
        }
    }
}