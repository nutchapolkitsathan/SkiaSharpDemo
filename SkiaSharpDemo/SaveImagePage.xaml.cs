using System;
using System.IO;

using Xamarin.Forms;

using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace SkiaSharpDemo
{
    public partial class SaveImagePage : ContentPage
    {

        private SKBitmap bitmap = BitmapExtensions.LoadBitmapResource(typeof(SaveImagePage),
                                "SkiaSharpDemo.Media.card.png");
        private SKBitmap bitmap2 = BitmapExtensions.LoadBitmapResource(typeof(SaveImagePage),
                                "SkiaSharpDemo.Media.dog.jpg");
        private SKBitmap bitmapOverlay;


        public SaveImagePage()
        {
            InitializeComponent();
            SKEncodedImageFormat imageFormat = SKEncodedImageFormat.Png;
            fileNameEntry.Text = Path.ChangeExtension(fileNameEntry.Text, imageFormat.ToString());
            statusLabel.Text = "OK";
        }

        [Obsolete]
        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            bitmapOverlay = BitmapOverlay(bitmap, bitmap2, args.Info);
            args.Surface.Canvas.DrawBitmap(bitmapOverlay, args.Info.Rect, BitmapStretch.Uniform);
        }

        //BitmapppppppppppOverlayyyyyyyyyyyy
        [Obsolete]
        public static SKBitmap BitmapOverlay(SKBitmap bmp1, SKBitmap bmp2, SKImageInfo info)
        {

            SKBitmap bmOverlay = new SKBitmap(info.Width, info.Height);
            SKCanvas canvas = new SKCanvas(bmOverlay);
            var resizedBitmap = bmp1.Resize(info, SKBitmapResizeMethod.Box);
            canvas.RotateDegrees(90, info.Width / 2, info.Height / 2);
            canvas.DrawBitmap(resizedBitmap, info.Width / 2 - resizedBitmap.Width / 2, info.Height / 2 - resizedBitmap.Height / 2);
            canvas.DrawBitmap(bmp2, new SKRect(100, 310, 500, 500), BitmapStretch.Uniform);
            canvas.DrawText("My name is Cat", 100, 600, new SKPaint { TextSize = 30 });
            canvas.DrawText("From Krungsri", 100, 650, new SKPaint { TextSize = 30 });

            return bmOverlay;
        }

        [Obsolete]
        async void OnButtonClicked(object sender, EventArgs args)
        {
            SKEncodedImageFormat imageFormat = SKEncodedImageFormat.Png;

            using (MemoryStream memStream = new MemoryStream())
            using (SKManagedWStream wstream = new SKManagedWStream(memStream))
            {
                bitmapOverlay.Encode(wstream, imageFormat, 100);
                byte[] data = memStream.ToArray();

                if (data == null)
                {
                    statusLabel.Text = "Encode returned null";
                }
                else if (data.Length == 0)
                {
                    statusLabel.Text = "Encode returned empty array";
                }
                else
                {
                    bool success = await DependencyService.Get<IPhotoLibrary>().
                        SavePhotoAsync(data, folderNameEntry.Text, fileNameEntry.Text);

                    if (!success)
                    {
                        statusLabel.Text = "SavePhotoAsync return false";
                    }
                    else
                    {
                        statusLabel.Text = "Success!";
                    }
                }
            }
        }
    }
}