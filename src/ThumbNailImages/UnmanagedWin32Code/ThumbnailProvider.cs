using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using dwg = System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace ThumbNailImages.UnmanagedWin32Code
{
    [Flags]
    public enum ThumbnailOptions
    {
        None = 0x00,
        BiggerSizeOk = 0x01,
        InMemoryOnly = 0x02,
        IconOnly = 0x04,
        ThumbnailOnly = 0x08,
        InCacheOnly = 0x10,
        IconBackground = 0x00000080
    }

    public class WindowsThumbnailProvider
    {
        private const string IShellItem2Guid = "7E9FB0D3-919F-4307-AB2E-9B1860310C93";

        [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int SHCreateItemFromParsingName(
            [MarshalAs(UnmanagedType.LPWStr)] string path,
            // The following parameter is not used - binding context.
            IntPtr pbc,
            ref Guid riid,
            [MarshalAs(UnmanagedType.Interface)] out IShellItem shellItem);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeleteObject(IntPtr hObject);

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe IntPtr memcpy(void* dst, void* src, UIntPtr count);

        public static Bitmap GetThumbnail(string fileName, int width, int height, ThumbnailOptions options)
        {
            var hBitmap = GetHBitmap(Path.GetFullPath(fileName), width, height, options);

            try
            {
                // return a System.Drawing.Bitmap from the hBitmap
                return GetBitmapFromHBitmap_2(hBitmap);
            }
            finally
            {
                // delete HBitmap to avoid memory leaks
                DeleteObject(hBitmap);
            }
        }

        public static Bitmap GetBitmapFromHBitmap(IntPtr nativeHBitmap)
        {
            var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(nativeHBitmap, IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            var stride = 4 * (bitmapSource.PixelWidth * bitmapSource.Format.BitsPerPixel + 31) / 32;

            using var ms = new MemoryStream();

            var enc = new PngBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create(bitmapSource));
            enc.Save(ms);


            ms.Position = 0;

            return new Bitmap(ms);
        }

        // First attempt
        public static Bitmap GetBitmapFromHBitmap_1(IntPtr nativeHBitmap)
        {
            var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                nativeHBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            // There were missing parentheses here (they are important because this is integer arithmetic.
            // Also worth noting, the code below only supports 32bpp images. In this case, stride
            // is always equal to width, so a micro-optim would be to not compute the stride.
            var stride = 4 * ((bitmapSource.PixelWidth * bitmapSource.Format.BitsPerPixel + 31) / 32);

            var destinationBuffer = new byte[stride * bitmapSource.PixelHeight];
            bitmapSource.CopyPixels(destinationBuffer, stride, 0);

            var pinned = GCHandle.Alloc(destinationBuffer, GCHandleType.Pinned);
            try
            {
                var pointer = pinned.AddrOfPinnedObject();

                var dpi = new Avalonia.Vector(bitmapSource.DpiX, bitmapSource.DpiY);
                var size = new PixelSize(bitmapSource.PixelWidth, bitmapSource.PixelHeight);
                var pixelFormat = bitmapSource.Format.BitsPerPixel == 32
                    ? PixelFormat.Bgra8888
                    : throw new NotSupportedException("Only 32bpp images are supported");

                return new Bitmap(pixelFormat, AlphaFormat.Unpremul, pointer, size, dpi, stride);
            }
            finally
            {
                pinned.Free();
            }
        }

        // Better attempt
        public static Bitmap GetBitmapFromHBitmap_2(IntPtr nativeHBitmap)
        {
            var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                nativeHBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            var dpi = new Avalonia.Vector(bitmapSource.DpiX, bitmapSource.DpiY);
            var size = new PixelSize(bitmapSource.PixelWidth, bitmapSource.PixelHeight);
            var pixelFormat = bitmapSource.Format.BitsPerPixel == 32
                ? PixelFormat.Bgra8888
                : throw new NotSupportedException("Only 32bpp images are supported");

            var bitmap = new Avalonia.Media.Imaging.WriteableBitmap(size, dpi, pixelFormat, AlphaFormat.Unpremul);
            using (var buffer = bitmap.Lock())
                bitmapSource.CopyPixels(new Int32Rect(0, 0, size.Width, size.Height), buffer.Address, buffer.RowBytes * size.Height, buffer.RowBytes);
            return bitmap;
        }

        private static IntPtr GetHBitmap(string fileName, int width, int height, ThumbnailOptions options)
        {
            IShellItem nativeShellItem;
            Guid shellItem2Guid = new Guid(IShellItem2Guid);
            int retCode = SHCreateItemFromParsingName(fileName, IntPtr.Zero, ref shellItem2Guid, out nativeShellItem);

            if (retCode != 0)
                throw Marshal.GetExceptionForHR(retCode);

            NativeSize nativeSize = new NativeSize();
            nativeSize.Width = width;
            nativeSize.Height = height;

            IntPtr hBitmap;
            HResult hr = ((IShellItemImageFactory)nativeShellItem).GetImage(nativeSize, options, out hBitmap);

            Marshal.ReleaseComObject(nativeShellItem);

            if (hr == HResult.Ok) return hBitmap;

            throw Marshal.GetExceptionForHR((int)hr);
        }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")]
        internal interface IShellItem
        {
            void BindToHandler(IntPtr pbc,
                [MarshalAs(UnmanagedType.LPStruct)] Guid bhid,
                [MarshalAs(UnmanagedType.LPStruct)] Guid riid,
                out IntPtr ppv);

            void GetParent(out IShellItem ppsi);
            void GetDisplayName(SIGDN sigdnName, out IntPtr ppszName);
            void GetAttributes(uint sfgaoMask, out uint psfgaoAttribs);
            void Compare(IShellItem psi, uint hint, out int piOrder);
        }

        internal enum SIGDN : uint
        {
            NORMALDISPLAY = 0,
            PARENTRELATIVEPARSING = 0x80018001,
            PARENTRELATIVEFORADDRESSBAR = 0x8001c001,
            DESKTOPABSOLUTEPARSING = 0x80028000,
            PARENTRELATIVEEDITING = 0x80031001,
            DESKTOPABSOLUTEEDITING = 0x8004c000,
            FILESYSPATH = 0x80058000,
            URL = 0x80068000
        }

        internal enum HResult
        {
            Ok = 0x0000,
            False = 0x0001,
            InvalidArguments = unchecked((int)0x80070057),
            OutOfMemory = unchecked((int)0x8007000E),
            NoInterface = unchecked((int)0x80004002),
            Fail = unchecked((int)0x80004005),
            ElementNotFound = unchecked((int)0x80070490),
            TypeElementNotFound = unchecked((int)0x8002802B),
            NoObject = unchecked((int)0x800401E5),
            Win32ErrorCanceled = 1223,
            Canceled = unchecked((int)0x800704C7),
            ResourceInUse = unchecked((int)0x800700AA),
            AccessDenied = unchecked((int)0x80030005)
        }

        [ComImport]
        [Guid("bcc18b79-ba16-442f-80c4-8a59c30c463b")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IShellItemImageFactory
        {
            [PreserveSig]
            HResult GetImage(
                [In, MarshalAs(UnmanagedType.Struct)] NativeSize size,
                [In] ThumbnailOptions flags,
                [Out] out IntPtr phbm);
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct NativeSize
        {
            private int width;
            private int height;

            public int Width
            {
                set { width = value; }
            }

            public int Height
            {
                set { height = value; }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RGBQUAD
        {
            public byte rgbBlue;
            public byte rgbGreen;
            public byte rgbRed;
            public byte rgbReserved;
        }
    }
}
