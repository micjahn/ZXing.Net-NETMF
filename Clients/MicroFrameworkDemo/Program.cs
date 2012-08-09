﻿using System;
using System.IO;
using Microsoft.SPOT;
using Microsoft.SPOT.Input;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;

namespace MicroFrameworkDemo
{
   public class Program : Microsoft.SPOT.Application
   {
      public static void Main()
      {
         Program myApplication = new Program();

         Window mainWindow = myApplication.CreateWindow();

         // Create the object that configures the GPIO pins to buttons.
         GPIOButtonInputProvider inputProvider = new GPIOButtonInputProvider(null);

         // Start the application
         myApplication.Run(mainWindow);
      }

      private Window mainWindow;

      public Window CreateWindow()
      {
         // Create a window object and set its size to the
         // size of the display.
         mainWindow = new Window();
         mainWindow.Height = SystemMetrics.ScreenHeight;
         mainWindow.Width = SystemMetrics.ScreenWidth;

         // Create a single text control.
         Text text = new Text();

         text.Font = Resources.GetFont(Resources.FontResources.small);
         text.TextContent = Resources.GetString(Resources.StringResources.String1);
         text.HorizontalAlignment = Microsoft.SPOT.Presentation.HorizontalAlignment.Center;
         text.VerticalAlignment = Microsoft.SPOT.Presentation.VerticalAlignment.Center;

         // Add the text control to the window.
         mainWindow.Child = text;

         // Connect the button handler to all of the buttons.
         mainWindow.AddHandler(Buttons.ButtonUpEvent, new RoutedEventHandler(OnButtonUp), false);

         // Set the window visibility to visible.
         mainWindow.Visibility = Visibility.Visible;

         // Attach the button focus to the window.
         Buttons.Focus(mainWindow);

         return mainWindow;
      }

      private void OnButtonUp(object sender, RoutedEventArgs evt)
      {
         ((Text) mainWindow.Child).TextContent = "This will take a while...";
         var operation = mainWindow.Dispatcher.BeginInvoke((obj) =>
                                              {
                                                 var bmp = new Bitmap(Convert.FromBase64String(bitmapString),
                                                                      Bitmap.BitmapImageType.Bmp);
                                                 var reader = new ZXing.BarcodeReader();
                                                 reader.TryHarder = false;
                                                 return reader.Decode(bmp);
                                              }, null);
         operation.Wait();
         if (operation.Result != null)
         {
            ((Text)mainWindow.Child).TextContent = ((ZXing.Result)(operation.Result)).Text;
         }
         else
         {
            ((Text) mainWindow.Child).TextContent = "No barcode found.";
         }
      }

      private const string bitmapString =
         "Qk0yJAAAAAAAADYEAAAoAAAAWQAAAFkAAAABAAgAAAAAAPwfAAAAAAAAAAAAAAABAAAAAAAABAIEAPz+/AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAAAAAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEAAAABAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQAAAAEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAAAAAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEAAAABAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQAAAAEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAAAAAQEBAQEBAQAAAAAAAAAAAAAAAAAAAAAAAAAAAAEBAQAAAAAAAAEBAQEBAQEBAQEBAQEBAQEBAQAAAAEBAQEBAQEBAQEBAQAAAAEBAQEBAQAAAAEBAQEBAQEAAAABAQEBAQEBAAAAAAAAAAAAAAAAAAAAAAAAAAAAAQEBAAAAAAAAAQEBAQEBAQEBAQEBAQEBAQEBAAAAAQEBAQEBAQEBAQEBAAAAAQEBAQEBAAAAAQEBAQEBAQAAAAEBAQEBAQEAAAAAAAAAAAAAAAAAAAAAAAAAAAABAQEAAAAAAAABAQEBAQEBAQEBAQEBAQEBAQEAAAABAQEBAQEBAQEBAQEAAAABAQEBAQEAAAABAQEBAQEBAAAAAQEBAQEBAQAAAAEBAQEBAQEBAQEBAQEBAQAAAAEBAQAAAAAAAAEBAQAAAAAAAAEBAQAAAAEBAQEBAQEBAQAAAAAAAAAAAAEBAQAAAAAAAAAAAAEBAQEBAQEAAAABAQEBAQEBAAAAAQEBAQEBAQEBAQEBAQEBAAAAAQEBAAAAAAAAAQEBAAAAAAAAAQEBAAAAAQEBAQEBAQEBAAAAAAAAAAAAAQEBAAAAAAAAAAAAAQEBAQEBAQAAAAEBAQEBAQEAAAABAQEBAQEBAQEBAQEBAQEAAAABAQEAAAAAAAABAQEAAAAAAAABAQEAAAABAQEBAQEBAQEAAAAAAAAAAAABAQEAAAAAAAAAAAABAQEBAQEBAAAAAQEBAQEBAQAAAAEBAQAAAAAAAAAAAAEBAQAAAAEBAQEBAQEBAQAAAAAAAAAAAAEBAQEBAQEBAQAAAAAAAAEBAQAAAAAAAAEBAQAAAAAAAAAAAAEBAQEBAQEAAAABAQEBAQEBAAAAAQEBAAAAAAAAAAAAAQEBAAAAAQEBAQEBAQEBAAAAAAAAAAAAAQEBAQEBAQEBAAAAAAAAAQEBAAAAAAAAAQEBAAAAAAAAAAAAAQEBAQEBAQAAAAEBAQEBAQEAAAABAQEAAAAAAAAAAAABAQEAAAABAQEBAQEBAQEAAAAAAAAAAAABAQEBAQEBAQEAAAAAAAABAQEAAAAAAAABAQEAAAAAAAAAAAABAQEBAQEBAAAAAQEBAQEBAQAAAAEBAQAAAAAAAAAAAAEBAQAAAAEBAQAAAAEBAQEBAQAAAAAAAAEBAQAAAAEBAQEBAQAAAAAAAAAAAAEBAQAAAAEBAQAAAAAAAAEBAQEBAQEAAAABAQEBAQEBAAAAAQEBAAAAAAAAAAAAAQEBAAAAAQEBAAAAAQEBAQEBAAAAAAAAAQEBAAAAAQEBAQEBAAAAAAAAAAAAAQEBAAAAAQEBAAAAAAAAAQEBAQEBAQAAAAEBAQEBAQEAAAABAQEAAAAAAAAAAAABAQEAAAABAQEAAAABAQEBAQEAAAAAAAABAQEAAAABAQEBAQEAAAAAAAAAAAABAQEAAAABAQEAAAAAAAABAQEBAQEBAAAAAQEBAQEBAQAAAAEBAQAAAAAAAAAAAAEBAQAAAAEBAQAAAAAAAAAAAAAAAAAAAAEBAQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEBAQAAAAEBAQEBAQEBAQEAAAABAQEBAQEBAAAAAQEBAAAAAAAAAAAAAQEBAAAAAQEBAAAAAAAAAAAAAAAAAAAAAQEBAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAQEBAAAAAQEBAQEBAQEBAQAAAAEBAQEBAQEAAAABAQEAAAAAAAAAAAABAQEAAAABAQEAAAAAAAAAAAAAAAAAAAABAQEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABAQEAAAABAQEBAQEBAQEBAAAAAQEBAQEBAQAAAAEBAQEBAQEBAQEBAQEBAQAAAAEBAQEBAQEBAQAAAAAAAAEBAQAAAAAAAAAAAAAAAAEBAQEBAQEBAQAAAAEBAQEBAQEBAQAAAAEBAQEBAQEAAAABAQEBAQEBAAAAAQEBAQEBAQEBAQEBAQEBAAAAAQEBAQEBAQEBAAAAAAAAAQEBAAAAAAAAAAAAAAAAAQEBAQEBAQEBAAAAAQEBAQEBAQEBAAAAAQEBAQEBAQAAAAEBAQEBAQEAAAABAQEBAQEBAQEBAQEBAQEAAAABAQEBAQEBAQEAAAAAAAABAQEAAAAAAAAAAAAAAAABAQEBAQEBAQEAAAABAQEBAQEBAQEAAAABAQEBAQEBAAAAAQEBAQEBAQAAAAAAAAAAAAAAAAAAAAAAAAAAAAEBAQEBAQAAAAEBAQAAAAAAAAAAAAEBAQEBAQAAAAEBAQAAAAEBAQAAAAAAAAEBAQEBAQAAAAEBAQEBAQEAAAABAQEBAQEBAAAAAAAAAAAAAAAAAAAAAAAAAAAAAQEBAQEBAAAAAQEBAAAAAAAAAAAAAQEBAQEBAAAAAQEBAAAAAQEBAAAAAAAAAQEBAQEBAAAAAQEBAQEBAQAAAAEBAQEBAQEAAAAAAAAAAAAAAAAAAAAAAAAAAAABAQEBAQEAAAABAQEAAAAAAAAAAAABAQEBAQEAAAABAQEAAAABAQEAAAAAAAABAQEBAQEAAAABAQEBAQEBAAAAAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQAAAAAAAAAAAAEBAQEBAQAAAAEBAQEBAQAAAAEBAQEBAQEBAQAAAAEBAQEBAQAAAAEBAQEBAQEBAQEAAAABAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAAAAAAAAAAAAAQEBAQEBAAAAAQEBAQEBAAAAAQEBAQEBAQEBAAAAAQEBAQEBAAAAAQEBAQEBAQEBAQAAAAEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEAAAAAAAAAAAABAQEBAQEAAAABAQEBAQEAAAABAQEBAQEBAQEAAAABAQEBAQEAAAABAQEBAQEBAQEBAAAAAQEBAQEBAQAAAAEBAQAAAAEBAQAAAAEBAQAAAAEBAQAAAAEBAQAAAAAAAAAAAAAAAAEBAQAAAAAAAAAAAAAAAAAAAAAAAAEBAQAAAAAAAAEBAQEBAQEBAQEAAAABAQEBAQEBAAAAAQEBAAAAAQEBAAAAAQEBAAAAAQEBAAAAAQEBAAAAAAAAAAAAAAAAAQEBAAAAAAAAAAAAAAAAAAAAAAAAAQEBAAAAAAAAAQEBAQEBAQEBAQAAAAEBAQEBAQEAAAABAQEAAAABAQEAAAABAQEAAAABAQEAAAABAQEAAAAAAAAAAAAAAAABAQEAAAAAAAAAAAAAAAAAAAAAAAABAQEAAAAAAAABAQEBAQEBAQEBAAAAAQEBAQEBAQAAAAEBAQAAAAEBAQAAAAAAAAEBAQAAAAEBAQAAAAEBAQAAAAAAAAEBAQAAAAEBAQEBAQEBAQEBAQEBAQEBAQEBAQAAAAEBAQAAAAEBAQEBAQEAAAABAQEBAQEBAAAAAQEBAAAAAQEBAAAAAAAAAQEBAAAAAQEBAAAAAQEBAAAAAAAAAQEBAAAAAQEBAQEBAQEBAQEBAQEBAQEBAQEBAAAAAQEBAAAAAQEBAQEBAQAAAAEBAQEBAQEAAAABAQEAAAABAQEAAAAAAAABAQEAAAABAQEAAAABAQEAAAAAAAABAQEAAAABAQEBAQEBAQEBAQEBAQEBAQEBAQEAAAABAQEAAAABAQEBAQEBAAAAAQEBAQEBAQAAAAAAAAEBAQEBAQEBAQEBAQAAAAEBAQAAAAEBAQEBAQEBAQEBAQAAAAEBAQAAAAEBAQAAAAEBAQAAAAAAAAAAAAAAAAAAAAAAAAEBAQEBAQEAAAABAQEBAQEBAAAAAAAAAQEBAQEBAQEBAQEBAAAAAQEBAAAAAQEBAQEBAQEBAQEBAAAAAQEBAAAAAQEBAAAAAQEBAAAAAAAAAAAAAAAAAAAAAAAAAQEBAQEBAQAAAAEBAQEBAQEAAAAAAAABAQEBAQEBAQEBAQEAAAABAQEAAAABAQEBAQEBAQEBAQEAAAABAQEAAAABAQEAAAABAQEAAAAAAAAAAAAAAAAAAAAAAAABAQEBAQEBAAAAAQEBAQEBAQAAAAEBAQAAAAAAAAAAAAAAAAEBAQAAAAAAAAEBAQAAAAAAAAEBAQAAAAEBAQAAAAAAAAEBAQEBAQEBAQAAAAEBAQEBAQAAAAEBAQEBAQEBAQEAAAABAQEBAQEBAAAAAQEBAAAAAAAAAAAAAAAAAQEBAAAAAAAAAQEBAAAAAAAAAQEBAAAAAQEBAAAAAAAAAQEBAQEBAQEBAAAAAQEBAQEBAAAAAQEBAQEBAQEBAQAAAAEBAQEBAQEAAAABAQEAAAAAAAAAAAAAAAABAQEAAAAAAAABAQEAAAAAAAABAQEAAAABAQEAAAAAAAABAQEBAQEBAQEAAAABAQEBAQEAAAABAQEBAQEBAQEBAAAAAQEBAQEBAQEBAQAAAAEBAQAAAAEBAQEBAQAAAAAAAAAAAAAAAAAAAAEBAQAAAAEBAQEBAQAAAAEBAQEBAQAAAAEBAQEBAQEBAQEBAQEBAQAAAAEBAQEBAQEAAAABAQEBAQEBAQEBAAAAAQEBAAAAAQEBAQEBAAAAAAAAAAAAAAAAAAAAAQEBAAAAAQEBAQEBAAAAAQEBAQEBAAAAAQEBAQEBAQEBAQEBAQEBAAAAAQEBAQEBAQAAAAEBAQEBAQEBAQEAAAABAQEAAAABAQEBAQEAAAAAAAAAAAAAAAAAAAABAQEAAAABAQEBAQEAAAABAQEBAQEAAAABAQEBAQEBAQEBAQEBAQEAAAABAQEBAQEBAAAAAQEBAQEBAQAAAAEBAQAAAAAAAAEBAQAAAAEBAQAAAAEBAQEBAQEBAQEBAQEBAQEBAQAAAAEBAQEBAQEBAQAAAAEBAQAAAAAAAAAAAAAAAAAAAAEBAQEBAQEAAAABAQEBAQEBAAAAAQEBAAAAAAAAAQEBAAAAAQEBAAAAAQEBAQEBAQEBAQEBAQEBAQEBAAAAAQEBAQEBAQEBAAAAAQEBAAAAAAAAAAAAAAAAAAAAAQEBAQEBAQAAAAEBAQEBAQEAAAABAQEAAAAAAAABAQEAAAABAQEAAAABAQEBAQEBAQEBAQEBAQEBAQEAAAABAQEBAQEBAQEAAAABAQEAAAAAAAAAAAAAAAAAAAABAQEBAQEBAAAAAQEBAQEBAQAAAAAAAAEBAQEBAQAAAAAAAAAAAAAAAAEBAQAAAAEBAQAAAAEBAQEBAQEBAQAAAAEBAQAAAAEBAQEBAQAAAAAAAAEBAQEBAQAAAAEBAQEBAQEAAAABAQEBAQEBAAAAAAAAAQEBAQEBAAAAAAAAAAAAAAAAAQEBAAAAAQEBAAAAAQEBAQEBAQEBAAAAAQEBAAAAAQEBAQEBAAAAAAAAAQEBAQEBAAAAAQEBAQEBAQAAAAEBAQEBAQEAAAAAAAABAQEBAQEAAAAAAAAAAAAAAAABAQEAAAABAQEAAAABAQEBAQEBAQEAAAABAQEAAAABAQEBAQEAAAAAAAABAQEBAQEAAAABAQEBAQEBAAAAAQEBAQEBAQEBAQAAAAEBAQAAAAEBAQAAAAEBAQAAAAEBAQEBAQAAAAAAAAEBAQEBAQAAAAAAAAAAAAEBAQEBAQAAAAAAAAAAAAAAAAAAAAEBAQEBAQEBAQEAAAABAQEBAQEBAQEBAAAAAQEBAAAAAQEBAAAAAQEBAAAAAQEBAQEBAAAAAAAAAQEBAQEBAAAAAAAAAAAAAQEBAQEBAAAAAAAAAAAAAAAAAAAAAQEBAQEBAQEBAQAAAAEBAQEBAQEBAQEAAAABAQEAAAABAQEAAAABAQEAAAABAQEBAQEAAAAAAAABAQEBAQEAAAAAAAAAAAABAQEBAQEAAAAAAAAAAAAAAAAAAAABAQEBAQEBAQEBAAAAAQEBAQEBAQAAAAAAAAEBAQAAAAAAAAEBAQAAAAEBAQEBAQAAAAEBAQAAAAAAAAEBAQAAAAEBAQAAAAEBAQAAAAEBAQEBAQEBAQEBAQEBAQAAAAEBAQEBAQEAAAABAQEBAQEBAAAAAAAAAQEBAAAAAAAAAQEBAAAAAQEBAQEBAAAAAQEBAAAAAAAAAQEBAAAAAQEBAAAAAQEBAAAAAQEBAQEBAQEBAQEBAQEBAAAAAQEBAQEBAQAAAAEBAQEBAQEAAAAAAAABAQEAAAAAAAABAQEAAAABAQEBAQEAAAABAQEAAAAAAAABAQEAAAABAQEAAAABAQEAAAABAQEBAQEBAQEBAQEBAQEAAAABAQEBAQEBAAAAAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQAAAAAAAAEBAQEBAQEBAQAAAAAAAAAAAAAAAAEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEAAAABAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAAAAAAAAAQEBAQEBAQEBAAAAAAAAAAAAAAAAAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQAAAAEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEAAAAAAAABAQEBAQEBAQEAAAAAAAAAAAAAAAABAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAAAAAQEBAQEBAQAAAAAAAAAAAAAAAAAAAAAAAAAAAAEBAQAAAAEBAQAAAAEBAQAAAAEBAQAAAAEBAQAAAAEBAQAAAAAAAAAAAAAAAAAAAAAAAAAAAAEBAQEBAQEAAAABAQEBAQEBAAAAAAAAAAAAAAAAAAAAAAAAAAAAAQEBAAAAAQEBAAAAAQEBAAAAAQEBAAAAAQEBAAAAAQEBAAAAAAAAAAAAAAAAAAAAAAAAAAAAAQEBAQEBAQAAAAEBAQEBAQEAAAAAAAAAAAAAAAAAAAAAAAAAAAABAQEAAAABAQEAAAABAQEAAAABAQEAAAABAQEAAAABAQEAAAAAAAAAAAAAAAAAAAAAAAAAAAABAQEBAQEBAAAAAQEBAQEBAQAAAAEBAQEBAQEBAQEBAQEBAQAAAAEBAQEBAQAAAAEBAQAAAAEBAQEBAQAAAAEBAQAAAAEBAQAAAAEBAQEBAQEBAQEBAQEBAQAAAAEBAQEBAQEAAAABAQEBAQEBAAAAAQEBAQEBAQEBAQEBAQEBAAAAAQEBAQEBAAAAAQEBAAAAAQEBAQEBAAAAAQEBAAAAAQEBAAAAAQEBAQEBAQEBAQEBAQEBAAAAAQEBAQEBAQAAAAEBAQEBAQEAAAABAQEBAQEBAQEBAQEBAQEAAAABAQEBAQEAAAABAQEAAAABAQEBAQEAAAABAQEAAAABAQEAAAABAQEBAQEBAQEBAQEBAQEAAAABAQEBAQEBAAAAAQEBAQEBAQAAAAEBAQAAAAAAAAAAAAEBAQAAAAEBAQEBAQEBAQAAAAAAAAAAAAEBAQAAAAEBAQAAAAEBAQAAAAEBAQAAAAAAAAAAAAEBAQAAAAEBAQEBAQEAAAABAQEBAQEBAAAAAQEBAAAAAAAAAAAAAQEBAAAAAQEBAQEBAQEBAAAAAAAAAAAAAQEBAAAAAQEBAAAAAQEBAAAAAQEBAAAAAAAAAAAAAQEBAAAAAQEBAQEBAQAAAAEBAQEBAQEAAAABAQEAAAAAAAAAAAABAQEAAAABAQEBAQEBAQEAAAAAAAAAAAABAQEAAAABAQEAAAABAQEAAAABAQEAAAAAAAAAAAABAQEAAAABAQEBAQEBAAAAAQEBAQEBAQAAAAEBAQAAAAAAAAAAAAEBAQAAAAEBAQEBAQEBAQAAAAAAAAAAAAAAAAEBAQEBAQAAAAEBAQAAAAEBAQAAAAAAAAAAAAEBAQAAAAEBAQEBAQEAAAABAQEBAQEBAAAAAQEBAAAAAAAAAAAAAQEBAAAAAQEBAQEBAQEBAAAAAAAAAAAAAAAAAQEBAQEBAAAAAQEBAAAAAQEBAAAAAAAAAAAAAQEBAAAAAQEBAQEBAQAAAAEBAQEBAQEAAAABAQEAAAAAAAAAAAABAQEAAAABAQEBAQEBAQEAAAAAAAAAAAAAAAABAQEBAQEAAAABAQEAAAABAQEAAAAAAAAAAAABAQEAAAABAQEBAQEBAAAAAQEBAQEBAQAAAAEBAQAAAAAAAAAAAAEBAQAAAAEBAQEBAQAAAAEBAQAAAAEBAQAAAAAAAAEBAQAAAAEBAQAAAAEBAQAAAAAAAAAAAAEBAQAAAAEBAQEBAQEAAAABAQEBAQEBAAAAAQEBAAAAAAAAAAAAAQEBAAAAAQEBAQEBAAAAAQEBAAAAAQEBAAAAAAAAAQEBAAAAAQEBAAAAAQEBAAAAAAAAAAAAAQEBAAAAAQEBAQEBAQAAAAEBAQEBAQEAAAABAQEAAAAAAAAAAAABAQEAAAABAQEBAQEAAAABAQEAAAABAQEAAAAAAAABAQEAAAABAQEAAAABAQEAAAAAAAAAAAABAQEAAAABAQEBAQEBAAAAAQEBAQEBAQAAAAEBAQEBAQEBAQEBAQEBAQAAAAEBAQEBAQEBAQEBAQAAAAAAAAEBAQEBAQEBAQAAAAEBAQAAAAEBAQEBAQEBAQEBAQEBAQAAAAEBAQEBAQEAAAABAQEBAQEBAAAAAQEBAQEBAQEBAQEBAQEBAAAAAQEBAQEBAQEBAQEBAAAAAAAAAQEBAQEBAQEBAAAAAQEBAAAAAQEBAQEBAQEBAQEBAQEBAAAAAQEBAQEBAQAAAAEBAQEBAQEAAAABAQEBAQEBAQEBAQEBAQEAAAABAQEBAQEBAQEBAQEAAAAAAAABAQEBAQEBAQEAAAABAQEAAAABAQEBAQEBAQEBAQEBAQEAAAABAQEBAQEBAAAAAQEBAQEBAQAAAAAAAAAAAAAAAAAAAAAAAAAAAAEBAQAAAAAAAAAAAAAAAAEBAQEBAQAAAAEBAQEBAQEBAQAAAAAAAAAAAAAAAAAAAAAAAAAAAAEBAQEBAQEAAAABAQEBAQEBAAAAAAAAAAAAAAAAAAAAAAAAAAAAAQEBAAAAAAAAAAAAAAAAAQEBAQEBAAAAAQEBAQEBAQEBAAAAAAAAAAAAAAAAAAAAAAAAAAAAAQEBAQEBAQAAAAEBAQEBAQEAAAAAAAAAAAAAAAAAAAAAAAAAAAABAQEAAAAAAAAAAAAAAAABAQEBAQEAAAABAQEBAQEBAQEAAAAAAAAAAAAAAAAAAAAAAAAAAAABAQEBAQEBAAAAAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEAAAABAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQAAAAEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAAAAAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEAAAABAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQAAAAEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAAAAAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEAAAA=";
   }
}