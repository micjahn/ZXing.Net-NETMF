/*
* Copyright 2008 ZXing authors
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Collections;
using ZXing.Common;
using ZXing.OneD;
using ZXing.PDF417.Internal;
using ZXing.QrCode;

namespace ZXing
{
   /// <summary> This is a factory class which finds the appropriate Writer subclass for the BarcodeFormat
   /// requested and encodes the barcode with the supplied contents.
   /// 
   /// </summary>
   /// <author>  dswitkin@google.com (Daniel Switkin)
   /// </author>
   /// <author>www.Redivivus.in (suraj.supekar@redivivus.in) - Ported from ZXING Java Source 
   /// </author>
   public sealed class MultiFormatWriter : Writer
   {
      private delegate Writer CreateWriterDelegate();

      private static readonly Hashtable formatMap;

      static MultiFormatWriter()
      {
         CreateWriterDelegate createEAN8Writer = () => new EAN8Writer();
         CreateWriterDelegate createEAN13Writer = () => new EAN13Writer();
         CreateWriterDelegate createUPCAWriter = () => new UPCAWriter();
         CreateWriterDelegate createQRCodeWriter = () => new QRCodeWriter();
         CreateWriterDelegate createCode39Writer = () => new Code39Writer();
         CreateWriterDelegate createCode128Writer = () => new Code128Writer();
         CreateWriterDelegate createITFWriter = () => new ITFWriter();
         CreateWriterDelegate createPDF417Writer = () => new PDF417Writer();
         CreateWriterDelegate createCodaBarWriter = () => new CodaBarWriter();
         formatMap = new Hashtable
                        {
                           {BarcodeFormat.EAN_8, createEAN8Writer},
                           {BarcodeFormat.EAN_13, createEAN13Writer},
                           {BarcodeFormat.UPC_A, createUPCAWriter},
                           {BarcodeFormat.QR_CODE, createQRCodeWriter},
                           {BarcodeFormat.CODE_39, createCode39Writer},
                           {BarcodeFormat.CODE_128, createCode128Writer},
                           {BarcodeFormat.ITF, createITFWriter},
                           {BarcodeFormat.PDF_417, createPDF417Writer},
                           {BarcodeFormat.CODABAR, createCodaBarWriter},
                        };
      }

      /// <summary>
      /// Gets the collection of supported writers.
      /// </summary>
      public static ICollection SupportedWriters
      {
         get { return formatMap.Keys; }
      }

      public BitMatrix encode(String contents, BarcodeFormat format, int width, int height)
      {
         return encode(contents, format, width, height, null);
      }

      public BitMatrix encode(String contents, BarcodeFormat format, int width, int height, IDictionary hints)
      {
         if (!formatMap.Contains(format))
            throw new ArgumentException("No encoder available for format " + format);

         return ((CreateWriterDelegate)(formatMap[format]))().encode(contents, format, width, height, hints);
      }
   }
}