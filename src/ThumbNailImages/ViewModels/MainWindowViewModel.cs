using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Text;
using ThumbNailImages.UnmanagedWin32Code;

namespace ThumbNailImages.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";

        public Bitmap Thumbnail => WindowsThumbnailProvider.GetThumbnail(@"C:\Users\timun\Pictures\PNGPIX-COM-Red-Heart-PNG-Transparent-Image.png", 1024,1024, ThumbnailOptions.None);
    }
}
