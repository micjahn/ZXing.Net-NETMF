using System;
using System.Runtime.InteropServices;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation.Media;
using Math = System.Math;

namespace ZXing
{
   public partial class RGBLuminanceSource
   {
      public RGBLuminanceSource(Bitmap bitmap)
         : this(bitmap, bitmap.Width, bitmap.Height)
      {
      }

      public RGBLuminanceSource(Bitmap bitmap, int width, int height)
         : base(width, height)
      {
         // In order to measure pure decoding speed, we convert the entire image to a greyscale array
         Color c;
         for (int y = 0; y < height; y++)
         {
            int offset = y * width;
            for (int x = 0; x < width; x++)
            {
               c = bitmap.GetPixel(x, y);
               var r = ColorUtility.GetRValue(c);
               var g = ColorUtility.GetGValue(c);
               var b = ColorUtility.GetBValue(c);
               luminances[offset + x] = (byte)(0.3 * r + 0.59 * g + 0.11 * b + 0.01);
            }
         }
      }
   }
}