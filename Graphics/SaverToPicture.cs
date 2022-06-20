using Graphics.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Graphics
{
    public static class SaverToPicture
    {

        public static void Save(Window canvas, string path)
        {
            var numberPicture = (int)Settings.Default["numberPicture"];
            Settings.Default["numberPicture"] = (numberPicture + 1) % Int32.MaxValue;
            Settings.Default.Save();
            numberPicture = (int)Settings.Default["numberPicture"];

            canvas.LayoutTransform = null;
            double dpi = 300;
            double scale = dpi / 96;

            Size size = canvas.RenderSize;
            RenderTargetBitmap image = new RenderTargetBitmap((int)(size.Width * scale), (int)(size.Height * scale), dpi, dpi, PixelFormats.Pbgra32);
            canvas.Measure(size);
            canvas.Arrange(new Rect(size));

            image.Render(canvas);
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (FileStream file = File.Create(path+$"\\{numberPicture}.jpeg"))
                encoder.Save(file);  
        }

    }

}
