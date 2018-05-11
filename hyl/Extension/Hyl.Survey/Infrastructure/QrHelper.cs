using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using ZXing;
using ZXing.Common;

namespace Hyl.Survey.Infrastructure
{
    public class QrHelper
    {
        public static Bitmap GetBitmap(string qrValue, int height = 250, int width = 250, int margin = 0)
        {
            var barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Height = height,
                    Width = width,
                    Margin = margin
                }
            };
            return barcodeWriter.Write(qrValue);
        }
    }
}