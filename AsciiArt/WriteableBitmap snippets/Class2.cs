using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiArt
{
    class Class2
    {
        ////////////////////////////////////////
        // Sample Code for drawing and text in same window
        ///////////////////////////////////////////
        private double[,] data = new double[600, 2]; //Data to be plotted
        private double xRange, yRange, xOffset, yOffset;
        private double winWidth, winHeight;
        /// <summary>
        /// Creates a sinusoid and plot it to the screen by way of a
        /// visual and a bitmap
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPlot_Click(object sender, RoutedEventArgs e)
        {
            int i;
            double x, y, xIncr, f;
            double xOld, yOld;
            FormattedText fmtxt;
            winWidth = imgPlot.Width; winHeight = imgPlot.Height;
            System.Windows.Media.Pen blkPen = new System.Windows.Media.Pen(System.Windows.Media.Brushes.Black, 1);
            System.Windows.Media.Brush bluBrush = System.Windows.Media.Brushes.AliceBlue;

            DrawingVisual vis = new DrawingVisual();                            //Create a "visual" to draw on.
            DrawingContext dc = vis.RenderOpen();                   //Create a drawing context for this visual
            f = 1;
            xRange = 12; yRange = 4;
            //Initialize x and y in world coordinates
            x = -xRange / 2;
            y = Math.Sin(2 * Math.PI * f * x);
            xIncr = xRange / (data.Length / 2);
            //Load the data array
            for (i = 0; i < data.Length / 2; i++)
            {
                data[i, 0] = x;
                data[i, 1] = Math.Sin(2 * Math.PI * f * x);
                x = x + xIncr;
            }
            xOffset = winWidth / 2 + 10; yOffset = winHeight / 2 - 10;
            World2Screen(data[0, 0], data[0, 1], out xOld, out yOld);
            //Convert coordinates to screen and draw lines between points
            for (i = 1; i < data.Length / 2; i++)
            {
                World2Screen(data[i, 0], data[i, 1], out x, out y);
                dc.DrawLine(blkPen, new System.Windows.Point(xOld, yOld), new System.Windows.Point(x, y));
                xOld = x; yOld = y;
            }
            //Draw x and y axis
            dc.DrawLine(blkPen, new System.Windows.Point(xOffset, 0), new System.Windows.Point(xOffset, winHeight));
            dc.DrawLine(blkPen, new System.Windows.Point(0, yOffset), new System.Windows.Point(winWidth, yOffset));
            //Just for fun write text to screen
            fmtxt = GetFormattedText("Hello Mom!", 14, "Times New Roman");
            dc.DrawText(fmtxt, new System.Windows.Point(10, 10));
            dc.Close();

            //Create a bit map
            RenderTargetBitmap bmp = new RenderTargetBitmap(512, 512, 96, 96, PixelFormats.Pbgra32);
            //Render the visual to the bitmap
            bmp.Render(vis);
            //Make the image source the bitmap
            imgPlot.Source = bmp;
        }

        /// <summary>
        /// Changes world coordinates to screen coordinates
        /// </summary>
        /// <param name="xWld"></param>
        /// <param name="yWld"></param>
        /// <param name="xScr"></param>
        /// <param name="yScr"></param>
        private void World2Screen(double xWld, double yWld,
         out double xScr, out double yScr)
        {
            xScr = xWld * winWidth / xRange + xOffset;
            yScr = -yWld * winHeight / yRange + yOffset;
        }

        // generates a pattern of text drawn in a valid Drawing Context, dc.  Requires input of font size, style, and the topmost insert point for the first line.
        private void GenerateFontBlock(DrawingContext dc, int iSize, string sStyle, System.Windows.Point p)
        {
            double x = p.X;
            double y = p.Y;

            FormattedText fmTextUpper, fmTextLower, fmTextOther;
            fmTextUpper = GetFormattedText("ABCDEFGHIJKLMNOQRSTUVWXYZ", iSize, sStyle);
            fmTextLower = GetFormattedText("abcdefghijklmnopqrstuvwxyz", iSize, sStyle);
            fmTextOther = GetFormattedText("0123456789!@#$%^&*()<>:+=", iSize, sStyle);
            dc.DrawText(fmTextUpper, new System.Windows.Point(x, y + 0 * iSize));
            dc.DrawText(fmTextLower, new System.Windows.Point(x, y + 1 * iSize));
            dc.DrawText(fmTextOther, new System.Windows.Point(x, y + 2 * iSize));
        }

        private void DrawFormattedSingleChar(string sLetter)
        {
            FontFamily.GetType();

            DrawingVisual vis = new DrawingVisual();
            DrawingContext dc = vis.RenderOpen();
            RenderTargetBitmap bmpNewImage = new RenderTargetBitmap(100, 100, 96, 96, PixelFormats.Pbgra32);

            string fontName = currentFont.Name;

            FormattedText fmTextUpper;
            fmTextUpper = GetFormattedText(sLetter, currentFont.Size, currentFont.Style.ToString());
            dc.DrawText(fmTextUpper, new System.Windows.Point(0, 0));
            dc.Close();
            bmpNewImage.Render(vis);

            //ComputeRatio(sLetter, bmpNewImage);

            singleLetterBitmapWindow.Source = bmpNewImage;
            return;
        }

        private void DrawFormattedTextDisplayBlock()
        {
            double insertVertOffset = 0;
            DrawingVisual vis = new DrawingVisual();
            DrawingContext dc = vis.RenderOpen();
            RenderTargetBitmap bmpNewImage = new RenderTargetBitmap((int)fontBitmapWindow.Width, (int)fontBitmapWindow.Height, 96, 96, PixelFormats.Pbgra32);

            string fontName = currentFont.Name;

            for (int i = 10; i < 30; i = i + 4)
            {
                System.Windows.Point insertPt = new System.Windows.Point(0, insertVertOffset);
                GenerateFontBlock(dc, i, fontName, insertPt);
                insertVertOffset = insertVertOffset + 3 * i;
            }
            dc.Close();
            bmpNewImage.Render(vis);
            fontBitmapWindow.Source = bmpNewImage;
            return;
        }

        private void CopyBitmapPortion(string fileName, int x, int y, int width, int height)
        {
            WriteableBitmap wbmap;
            WriteableBitmap sourcewbmap;
            if (String.IsNullOrEmpty(fileName))
            {
                MessageBox.Show("Filename string is null or empty");
                return;
            }
            else
            {
                sourcewbmap = ConvertJPGToWriteableBitmap(fileName);
            }

            if (convertedPhoto.Source != null)
                wbmap = (WriteableBitmap)convertedPhoto.Source;
            {
                wbmap = new WriteableBitmap(sourcewbmap.PixelWidth, sourcewbmap.PixelHeight, 96, 96, PixelFormats.Bgra32, null);
            }

            // some error checking to make sure that the start corner of the request copy are valid (within limits -- not negative, and less than the width / height of the image)
            if ((x > width) || (y > height) || (x < 0) || (y < 0))
            {
                return;
            }

            // is the size of the sample region greater than the bounds of bitmap?
            if (((x + width) > wbmap.PixelWidth) || (y + height) > wbmap.PixelHeight)
            {
                return;
            }

            // otherwise prepare with the copying
            sourcewbmap.Lock();
            wbmap.Lock();

            Int32Rect rc = new Int32Rect(x, y, width, height);

            wbmap.WritePixels(
                rc,
                (IntPtr)(sourcewbmap.BackBuffer.ToInt64() + rc.Y * sourcewbmap.BackBufferStride + rc.X * 4),
                rc.Height * sourcewbmap.BackBufferStride,
                sourcewbmap.BackBufferStride
            );

            wbmap.Unlock();
            sourcewbmap.Unlock();
            convertedPhoto.Source = wbmap;  // write our modified bitmap to the appropriate source on our WPF
        }

        //Test code to load a JPG and then write over top of it using writeable bitmaps
        private void SampleWriteableBitmap()
        {
            //Create a writeable bitmap (which is a valid WPF image source
            WriteableBitmap wbmap = ConvertJPGToWriteableBitmap(@"..\..\photos\Image1.jpg");

            //Overwriting the image by making all pixels black -- a silly test I know.
            for (int x = 0; x < wbmap.PixelWidth - 100; ++x)
            {
                for (int y = 0; y < wbmap.PixelHeight - 100; ++y)
                {
                    // apply pixels to bitmap--Colors are A, Red, Green, Blue.  A must be 255 to be opaque
                    //                                                  A    R  G  B
                    SetPixel(wbmap, x, y, System.Drawing.Color.FromArgb(255, 0, 0, 0));
                }
            }

            // and then creating a smaller block of a different color -- again as  a test.
            for (int x = 0; x < 20; ++x)
            {
                for (int y = 0; y < 20; ++y)
                {
                    // apply pixels to bitmap--Colors are A, Red, Green, Blue.  A must be 255 to be opaque
                    //                                                   A    R  G  B
                    SetPixel(wbmap, x, y, System.Drawing.Color.FromArgb(255, 255, 0, 0));
                }
            }

            // Display some parameters for debugging...
            ByteReadData.Text = "wbmap.PixelHeight: " + wbmap.PixelHeight + "    wbmap.PixelWidth: " +
                wbmap.PixelWidth + "   wbmap.Format: " + wbmap.Format.BitsPerPixel + " ---> " +
                wbmap.PixelHeight * wbmap.PixelWidth * wbmap.Format.BitsPerPixel / 8 + "   wbmap.dpix: " + wbmap.DpiX + "   wbmap.dpiy: " + wbmap.DpiY;

            // set image source to the new bitmap
            convertedPhoto.Source = wbmap;
        }
    }
}
