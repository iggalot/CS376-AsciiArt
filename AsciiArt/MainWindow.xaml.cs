using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.IO;
using System.Windows.Interop;

namespace AsciiArt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Attributes
        public static Font currentFont;
        public static FontRatio FontRatioList;
        public static FontInfo fontInfo;
        public static AsciiMap Asciimap;
        public static Window DataWindow;  // create a holder for a data window
        private static WriteableBitmap source;  // stores our source photo for the app
        private static int CurrentDrawMode;  // stores our current drawmode
        private static int AsciiDensityMode;
        public static float PixellatedWidth = 1;
        public static float PixellatedHeight = 1;
        public static float aspectRatio;
        public static float fontpixelwidth;
        public static float fontpixelheight;

        public static int horiz_index;
        public static int vert_index;
        public static double x_width;
        public static double y_width;

        // Our drawing modes
        private const int DRAWMODE_NONE = -1;
        private const int DRAWMODE_NEGATIVE = 0;
        private const int DRAWMODE_GRAYSCALE = 1;
        private const int DRAWMODE_GRAYPIXELLATED = 2;
        private const int DRAWMODE_COLORPIXELLATED = 3;
        private const int DRAWMODE_ASCIIART = 4;

        // Our ASCII_DENSITY MODE (small, medium, and large)
        private const int ASCIIMODE_NORMAL = 0;
        private const int ASCIIMODE_SMALL = 1;
        private const int ASCIIMODE_MEDIUM = 2;
        private const int ASCIIMODE_LARGE = 3;


        private string AssignAsciiChar_NormalSet(double red_ratio)
        {
            string asciistring = "";
            if (red_ratio < 0.0)
                asciistring += "@";
            else if (red_ratio < 0.1)
                asciistring += "&";
            else if (red_ratio < 0.2)
                asciistring += "D";
            else if (red_ratio < 0.3)
                asciistring += "E";
            else if (red_ratio < 0.4)
                asciistring += "o";
            else if (red_ratio < 0.5)
                asciistring += "x";
            else if (red_ratio < 0.6)
                asciistring += "+";
            else if (red_ratio < 0.7)
                asciistring += "~";
            else if (red_ratio < 0.8)
                asciistring += "^";
            else if (red_ratio < 0.9)
                asciistring += ".";
            else if (red_ratio <= 1.0)
                asciistring += ",";
            else
            {
                asciistring += "~~";
            }

            return asciistring;
        }

        private string AssignAsciiChar_LargeSet(double red_ratio)
        {
            string asciistring = "";

            if (red_ratio < 0.0)
                asciistring += "$";
            else if (red_ratio < 0.02)
                asciistring += "@";
            else if (red_ratio < 0.04)
                asciistring += "B";
            else if (red_ratio < 0.06)
                asciistring += "%";
            else if (red_ratio < 0.08)
                asciistring += "8";
            else if (red_ratio < 0.10)
                asciistring += "&";
            else if (red_ratio < 0.11)
                asciistring += "W";
            else if (red_ratio < 0.13)
                asciistring += "M";
            else if (red_ratio < 0.15)
                asciistring += "#";
            else if (red_ratio <= 0.16)
                asciistring += "*";

            else if (red_ratio < 0.18)
                asciistring += "o";
            else if (red_ratio < 0.20)
                asciistring += "a";
            else if (red_ratio < 0.21)
                asciistring += "h";
            else if (red_ratio < 0.23)
                asciistring += "k";
            else if (red_ratio < 0.25)
                asciistring += "b";
            else if (red_ratio < 0.27)
                asciistring += "d";
            else if (red_ratio < 0.29)
                asciistring += "p";
            else if (red_ratio < 0.30)
                asciistring += "q";
            else if (red_ratio <= 0.32)
                asciistring += "w";

            else if (red_ratio < 0.33)
                asciistring += "m";
            else if (red_ratio < 0.35)
                asciistring += "Z";
            else if (red_ratio < 0.37)
                asciistring += "O";
            else if (red_ratio < 0.39)
                asciistring += "0";
            else if (red_ratio < 0.41)
                asciistring += "Q";
            else if (red_ratio < 0.43)
                asciistring += "L";
            else if (red_ratio < 0.45)
                asciistring += "C";
            else if (red_ratio < 0.47)
                asciistring += "J";
            else if (red_ratio <= 0.49)
                asciistring += "U";

            else if (red_ratio < 0.50)
                asciistring += "Y";
            else if (red_ratio < 0.52)
                asciistring += "X";
            else if (red_ratio < 0.55)
                asciistring += "z";
            else if (red_ratio < 0.57)
                asciistring += "c";
            else if (red_ratio < 0.59)
                asciistring += "v";
            else if (red_ratio < 0.61)
                asciistring += "u";
            else if (red_ratio < 0.63)
                asciistring += "n";
            else if (red_ratio < 0.65)
                asciistring += "x";
            else if (red_ratio <= 0.67)
                asciistring += "r";

            else if (red_ratio < 0.68)
                asciistring += "j";
            else if (red_ratio < 0.70)
                asciistring += "f";
            else if (red_ratio < 0.72)
                asciistring += "t";
            else if (red_ratio < 0.73)
                asciistring += "/";
            else if (red_ratio < 0.74)
                asciistring += "|";
            else if (red_ratio < 0.75)
                asciistring += "(";
            else if (red_ratio < 0.76)
                asciistring += ")";
            else if (red_ratio < 0.77)
                asciistring += "1";
            else if (red_ratio <= 0.78)
                asciistring += "{";

            else if (red_ratio < 0.79)
                asciistring += "}";
            else if (red_ratio < 0.80)
                asciistring += "[";
            else if (red_ratio < 0.81)
                asciistring += "]";
            else if (red_ratio < 0.82)
                asciistring += "?";
            else if (red_ratio < 0.84)
                asciistring += "-";
            else if (red_ratio < 0.85)
                asciistring += "_";
            else if (red_ratio < 0.86)
                asciistring += "+";
            else if (red_ratio < 0.87)
                asciistring += "~";
            else if (red_ratio <= 0.88)
                asciistring += "<";

            else if (red_ratio < 0.89)
                asciistring += ">";
            else if (red_ratio < 0.90)
                asciistring += "i";
            else if (red_ratio < 0.92)
                asciistring += "!";
            else if (red_ratio < 0.94)
                asciistring += ":";
            else if (red_ratio < 0.95)
                asciistring += ",";
            else if (red_ratio < 0.97)
                asciistring += "^";
            else if (red_ratio < 0.98)
                asciistring += "'";
            else if (red_ratio <= 1.00)
                asciistring += ".";
            else
            {
                asciistring += "~~";
            }

            return asciistring;
        }

        private string GetAsciiChar(double red_ratio)
        {
            string str = "";
            switch(AsciiDensityMode)
            {
                case ASCIIMODE_LARGE:
                    str += AssignAsciiChar_LargeSet(red_ratio);
                    break;
                case ASCIIMODE_SMALL:
                    break;
                case ASCIIMODE_MEDIUM:
                    break;
                case ASCIIMODE_NORMAL:
                    //MessageBox.Show("Normal Mode");
                    str += AssignAsciiChar_NormalSet(red_ratio);
                    break;
                default:

                    break;
            }
            return str;
        }

        private void HandleRBCheck(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;


            switch (rb.Name)
            {
                case "RBNormal":
                    RBNormal.IsChecked = true;
                    RBLarge.IsChecked = false;
                    //MessageBox.Show("Normal Selected");
                    AsciiDensityMode = ASCIIMODE_NORMAL;
                    break;
                case "RBLarge":
                    RBNormal.IsChecked = false;
                    RBLarge.IsChecked = true;
                    //MessageBox.Show("Large Selected");
                    AsciiDensityMode = ASCIIMODE_LARGE;
                    break;
                default: 
                    MessageBox.Show("Invalid Selection received in " + rb.Name + "radio button");
                    return;
            }
            DrawPixellated(DRAWMODE_ASCIIART);
        }

        private float ComputeAspectRatioFromSource(double width, double height)
        {
            float ratioX = ((float)width) / (float)source.Width;
            float ratioY = ((float)height) / (float)source.Height;
            float ratio = ((float)ratioX < (float)ratioY ? (float)ratioX : (float)ratioY);
            return ratio;
        }

        private void SetNumberOfPixelBlocks()
        {
            aspectRatio = ComputeAspectRatioFromSource(sourcePhoto.ActualWidth, sourcePhoto.ActualHeight);

            PixellatedWidth = source.PixelWidth / fontpixelwidth;
            PixellatedHeight = source.PixelHeight / fontpixelheight;

            // set our display values
            strNumBlockHoriz.Text = "" + PixellatedWidth.ToString();
            strNumBlockVert.Text = PixellatedHeight.ToString();
        }

        private void ComputeFontPixelInfo()
        {
            System.Windows.Media.FontFamily mediaFontFamily = new System.Windows.Media.FontFamily(currentFont.Name);
            FontInfo finfo = new FontInfo(source, currentFont);
            System.Windows.Size fontsizepixeldims = finfo.GetGlyphDims("X", mediaFontFamily, currentFont.Size, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
            fontpixelwidth = (float)fontsizepixeldims.Width;
            fontpixelheight = (float)fontsizepixeldims.Height;
            SetNumberOfPixelBlocks();
            UpdateFontDisplayInfo();

            CurrentDrawMode = DRAWMODE_NONE;
        }

        //Methods
        private void SetupWindow(){
            AsciiDensityMode = ASCIIMODE_LARGE;
            // set a blank image in our viewer source of type WriteableBitmap
            WriteableBitmap blanksourcebmap = new WriteableBitmap(512, 512, 96, 96, PixelFormats.Pbgra32, null);
            source = blanksourcebmap;  // save our blank image.
            sourcePhoto.Source = source;

            // Set current default window font information
            System.Drawing.FontFamily fontFamily = new System.Drawing.FontFamily("Consolas");
            currentFont = new Font(fontFamily, 2.0F, System.Drawing.FontStyle.Regular);

            // compute our font pixel info
            ComputeFontPixelInfo();

            FontRatioList = new FontRatio();
        }

        private void MenuClearImage_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage image = new BitmapImage();
            convertedPhoto.Source = image;
        }

        private void UpdateFontDisplayInfo()
        {

            strCurrentFont.Text = currentFont.Name + ",  " + currentFont.SizeInPoints + "pt, " + currentFont.Style ;
        }

        private void MenuFont_Click(object sender, RoutedEventArgs e)
        {
            // Show the dialog.
            using (System.Windows.Forms.FontDialog fontDialog1 = new System.Windows.Forms.FontDialog())
            {
                fontDialog1.Font = currentFont;
                fontDialog1.ShowEffects = false;
                fontDialog1.ShowHelp = false;
                fontDialog1.ShowApply = false;
                fontDialog1.FontMustExist = true;
                fontDialog1.AllowScriptChange = false;
                fontDialog1.FixedPitchOnly = true;
                fontDialog1.MinSize = 2;
               
                if (fontDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    currentFont = fontDialog1.Font;
                    FontInfo finfo = new FontInfo(source, currentFont);
                    //finfo.Display();
                }

                // compute our font pixel info and update parameters
                ComputeFontPixelInfo();
            }
        }

        private void MenuSaveAs_Click(object sender, RoutedEventArgs e)
        {
            string fileName = null;
            using (System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog())
            {
                string path = System.Environment.CurrentDirectory;
                string photoDir = System.IO.Path.GetFullPath(System.IO.Path.Combine(path, @"..\..\photos"));
                saveFileDialog1.InitialDirectory = photoDir;
                saveFileDialog1.Filter = "Image files (*.jpg, *.bmp, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.bmp; *.jpeg; *.jpe; *.jfif; *.png";
                saveFileDialog1.Title = "Save Image As ...";
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.FilterIndex = 2;

                if(saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    fileName = saveFileDialog1.FileName;
                }

                if (fileName != null)
                {
                    //Do something with the file, for example read text from it
                    SaveConvertedPhotoToDisk(fileName);
                }
                else
                {
                    MessageBox.Show("No file selected!");
                    return;
                }

            }
        }

        private void MenuImageLoad_Click(object sender, RoutedEventArgs e)
        {
            string fileName = null;
            string extension = null;

            using (System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog())
            {
                string path = System.Environment.CurrentDirectory;
                string photoDir = System.IO.Path.GetFullPath(System.IO.Path.Combine(path, @"..\..\photos"));
                openFileDialog1.InitialDirectory = photoDir;
                openFileDialog1.Filter = "Image files (*.jpg, *.bmp, *.jpeg, *.jpe, *.tiff, *.png) | *.jpg; *.bmp; *.jpeg; *.jpe; *.tiff; *.png";
                openFileDialog1.Title = "Load Image ...";
                openFileDialog1.FilterIndex = 2;
                openFileDialog1.RestoreDirectory = true;

                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    fileName = openFileDialog1.FileName;
                    extension = System.IO.Path.GetExtension(fileName);
                }
            }

            if (fileName == null)
            {
                MessageBox.Show("No file selected!");
                return;
            }

            // Open a Stream and decode an image
            Stream imageStreamSource = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);

            BitmapDecoder decoder;
            string str = extension;

            // Setup our file handling for different formats...
            if ((extension == ".jpg") || (extension == ".JPG") || (extension == ".jpeg") || (extension == ".JPEG") || (extension == ".jpe") || (extension == ".JPE"))
            {
                decoder = new JpegBitmapDecoder(imageStreamSource, BitmapCreateOptions.None, BitmapCacheOption.Default);
            }
            else if ((extension == ".png") || (extension == ".PNG"))
            {
                decoder = new PngBitmapDecoder(imageStreamSource, BitmapCreateOptions.None, BitmapCacheOption.Default);
            } else if ((extension == ".gif") || (extension == ".GIF")) {
                decoder = new GifBitmapDecoder(imageStreamSource, BitmapCreateOptions.None, BitmapCacheOption.Default);
            } else if ((extension == ".bmp") || (extension == ".BMP"))
            {
                decoder = new BmpBitmapDecoder(imageStreamSource, BitmapCreateOptions.None, BitmapCacheOption.Default);
            } else if ((extension == ".tiff") || (extension == ".TIFF")) {
                decoder = new TiffBitmapDecoder(imageStreamSource, BitmapCreateOptions.None, BitmapCacheOption.Default);
            }
            else {
                MessageBox.Show("ERROR: Unknown file type.  No matching BitmapDecoder defined.");
                return;
            }

            BitmapSource bitmapSource = decoder.Frames[0];

            // Create WriteableBitmap to copy the pixel data to.      
            WriteableBitmap target = new WriteableBitmap(
              bitmapSource.PixelWidth,
              bitmapSource.PixelHeight,
              bitmapSource.DpiX, bitmapSource.DpiY,
              PixelFormats.Pbgra32, null);

            int bytesPerPixel = (target.Format.BitsPerPixel + 7) / 8; // general formula
            int stride = target.PixelWidth * bytesPerPixel;

            // Create data array to hold source pixel data
            byte[] data = new byte[stride * target.PixelHeight];

            // Copy source image pixels to the data array
            bitmapSource.CopyPixels(data, stride, 0);

            // Write the pixel data to the WriteableBitmap.
            target.WritePixels(
              new Int32Rect(0, 0, target.PixelWidth, target.PixelHeight),
              data, stride, 0);

            source = target;  // save our writeablebitmap object to our image variable.
            sourcePhoto.Source = target;
            //sourcePhoto.Stretch = Stretch.Uniform;
            //sourcePhoto.Margin = new Thickness(5);
            SetNumberOfPixelBlocks();

            source = target;  // save our writeablebitmap object to our image variable.
            ComputeFontPixelInfo();
        }

        private void MenuGrayscale_Click(object sender, RoutedEventArgs e)
        {
            WriteableBitmap sourcewbmap = source.Clone();
            System.Drawing.Color pixelColor;
            int alpha = 255;   // must be 255 for colors to be visible when rendered
            int red = 0;
            int green = 0;
            int blue = 0;

            // if there is no source photo (for some reason), we will draw the inverted image as a blue square to indicate a problem
            if (source == null)
            {
                blue = 255;
            }
            else
            {
                red = 255;
            }

            CurrentDrawMode = DRAWMODE_GRAYSCALE;

            for (int x = 0; x < sourcewbmap.PixelWidth; ++x)
            {
                for (int y = 0; y < sourcewbmap.PixelHeight; ++y)
                {
                    pixelColor = GetPixel(sourcewbmap, x, y);
                    // convert the pixels to gray scale the pixel colors
                    int L = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
                    red = (int)(0.2989*pixelColor.R);
                    green = (int)(0.5870*pixelColor.G);
                    blue = (int)(0.1140*pixelColor.B);
                    // apply pixels to bitmap--Colors are A, Red, Green, Blue.  A must be 255 to be opaque
                    //                                                          A    R  G  B
                    SetPixel(sourcewbmap, x, y, System.Drawing.Color.FromArgb(alpha, L, L, L));
                }
            }
            convertedPhoto.Source = sourcewbmap;  // write our modified bitmap to the appropriate source on our WPF

        }

        private void MenuGrayPixelated_Click(object sender, RoutedEventArgs e)
        {
            CurrentDrawMode = DRAWMODE_GRAYPIXELLATED;
            DrawPixellated(DRAWMODE_GRAYPIXELLATED);
        }

        private void MenuColorPixelated_Click(object sender, RoutedEventArgs e)
        {
            CurrentDrawMode = DRAWMODE_COLORPIXELLATED;
            DrawPixellated(DRAWMODE_COLORPIXELLATED);

        }

        private void DrawPixellated(int drawMode)
        {
            CurrentDrawMode = drawMode; // set our current drawmode;

            WriteableBitmap newconvertedwbmap = source.Clone();

            //// sample block for testing
            x_width = ((double)source.Width / (double)PixellatedWidth);
            y_width = ((double)source.Height / (double)PixellatedHeight);

            double area = x_width * y_width;
            string totalmapstring = "";

            for (int n = 0; n < PixellatedHeight; n++)
            {
                string asciistring = "";

                for (int m = 0; m < PixellatedWidth; m++)
                {
                    double red_ratio = 0.0f;
                    double green_ratio = 0.0f;
                    double blue_ratio = 0.0f;
                    double[] arrBlockRatio = { 0.0, 0.0, 0.0 };  // an array to store our RGB ratio values
                    horiz_index = (int)(m * x_width);
                    vert_index = (int)(n * y_width);

                    arrBlockRatio = ComputePixellatedBlockRatio(x_width, y_width, horiz_index, vert_index);
 
                    switch (drawMode)
                    {
                        case DRAWMODE_GRAYPIXELLATED:
                            red_ratio = (arrBlockRatio[0] + arrBlockRatio[1] + arrBlockRatio[2]) / (3.0f);
                            green_ratio = (arrBlockRatio[0] + arrBlockRatio[1] + arrBlockRatio[2]) / (3.0f);
                            blue_ratio = (arrBlockRatio[0] + arrBlockRatio[1] + arrBlockRatio[2]) / (3.0f);
                            break;
                        case DRAWMODE_COLORPIXELLATED:
                            red_ratio = arrBlockRatio[0];
                            green_ratio = arrBlockRatio[1];
                            blue_ratio = arrBlockRatio[2];
                           break;
                        // ASCII ART MODE requires a gray scale image....
                        case DRAWMODE_ASCIIART:
                            red_ratio = (arrBlockRatio[0] + arrBlockRatio[1] + arrBlockRatio[2]) / (3.0f);
                            green_ratio = (arrBlockRatio[0] + arrBlockRatio[1] + arrBlockRatio[2]) / (3.0f);
                            blue_ratio = (arrBlockRatio[0] + arrBlockRatio[1] + arrBlockRatio[2]) / (3.0f);

                            //artstring += red_ratio.ToString("0.00") + " ";

                            string str = GetAsciiChar(red_ratio);
                            //MessageBox.Show(str);
                            if(str.Length == 2)
                            {
                                MessageBox.Show("Error.  Invalid DRAWMODE received in DrawPixellated().\n" + "Row: " + n + ", " + "Col: " + m + "\n" + "Char:  " + str);
                                return;
                            } else
                            {
                                asciistring += str;
                            }
                            break;
                    }

                    if ((drawMode == DRAWMODE_GRAYPIXELLATED) || (drawMode == DRAWMODE_COLORPIXELLATED))
                    {
                        // make a block of pixels that use the pixellated values
                        for (int i = horiz_index; i < horiz_index + x_width; i++)
                        {
                            for (int j = vert_index; j < vert_index + y_width; j++)
                            {
                                double red = red_ratio * 255.0f;
                                double green = green_ratio * 255.0f;
                                double blue = blue_ratio * 255.0f;
                                SetPixel(newconvertedwbmap, i, j, System.Drawing.Color.FromArgb(255, (int)red, (int)green, (int)blue));
                            }
                        }
                    }
                }
                totalmapstring += asciistring + "\n";
            }
            if(drawMode == DRAWMODE_ASCIIART)
            {
                WriteableBitmap textmap = DrawTextMap(totalmapstring);
                //DisplayData(null, textmap, null);
                convertedPhoto.Source = textmap;      
            } else
            {
                convertedPhoto.Source = newconvertedwbmap;
            }
        }

        private double[] ComputePixellatedBlockRatio(double x_width, double y_width, int horiz_index, int vert_index)
        {
            double[] ratioarray = { 0.0f, 0.0f, 0.0f };
            int count = 0;
            for (int i = horiz_index; i < horiz_index + (int)x_width; i++)
            {
                for (int j = vert_index; j < vert_index + (int)y_width; j++)
                {
                    System.Drawing.Color pixel_color = GetPixel(source, i, j);
                    ratioarray[0] += pixel_color.R;
                    ratioarray[1] += pixel_color.G;
                    ratioarray[2] += pixel_color.B;
                    count++;
                }
            }
            ratioarray[0] = ratioarray[0] / (255.0f * count);
            ratioarray[1] = ratioarray[1] / (255.0f * count);
            ratioarray[2] = ratioarray[2] / (255.0f * count);
            
            return ratioarray;
        }

        public void MenuAsciiArt_Click(object sender, RoutedEventArgs e)
        {
            CurrentDrawMode = DRAWMODE_ASCIIART;
            DrawPixellated(DRAWMODE_ASCIIART);
        }

        // This inverts the image, making a negative of the selected image
        private void MenuInvert_Click(object sender, RoutedEventArgs e)
        {
            CurrentDrawMode = DRAWMODE_NEGATIVE;

            WriteableBitmap sourcewbmap = source.Clone();
            int alpha = 255;   // must be 255 for colors to be visible when rendered
            int red = 0;
            int green = 0;
            int blue = 0;

            // if there is no source photo (for some reason), we will draw the inverted image as a blue square to indicate a problem
            if (source == null)
            {
                blue = 255;
            } else {
                red = 255;
            }

            for(int x=0; x<sourcewbmap.PixelWidth; ++x)
            {
                for(int y=0; y<sourcewbmap.PixelHeight; ++y)
                {
                    System.Drawing.Color pixelColor = GetPixel(sourcewbmap, x, y);
                    // invert the pixel colors
                    red = 255 - pixelColor.R;
                    green = 255 - pixelColor.G;
                    blue = 255 - pixelColor.B;
                    // apply pixels to bitmap--Colors are A, Red, Green, Blue.  A must be 255 to be opaque
                    //                                                           A    R     G      B
                    SetPixel(sourcewbmap, x, y, System.Drawing.Color.FromArgb(alpha, red, green, blue));
                }
            }
            convertedPhoto.Source = sourcewbmap;  // write our modified bitmap to the appropriate source on our WPF
        }

        public void MenuFontRatioDisplay_Click(object sender, RoutedEventArgs e)
        {
            FontRatioList.ProcessList();  // get rid of filler characters (or '~') characters in our ratio list
            DisplayData(FontRatioList.DisplayAll(),null, null);
        }

        private void fontSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // ... Get Slider reference.
            Slider slider = e.OriginalSource as Slider;

            // ... Get Value.
            double value = slider.Value;

            if (slider != null)
            {
                if(currentFont != null)
                {
                    System.Drawing.FontFamily ffamily = currentFont.FontFamily;
                    System.Drawing.FontStyle fstyle = currentFont.Style;
                    currentFont = new Font(ffamily, (float)value, fstyle);
                    ComputeFontPixelInfo();
                    //DrawPixellated(DRAWMODE_ASCIIART);
                }
            }

        }

        public void MenuHelp_Click(object sender, RoutedEventArgs e)
        {
            string str = "MENU COMMANDS DESCRIPTIONS\n";
            str += "------FILE--------------------------------------------------------------------------\n";
            str += "OPEN             -- Loads an image.\n";
            str += "SAVE AS          -- Saves the currently modified image.\n";
            str += "------FONT-------------------------------------------------------------------------\n";
            str += "CHANGE FONT      -- Dialog to change to new monoscale font\n";
            str += "------TOOLS-------------------------------------------------------------------------\n";
            str += "NEGATIVE         -- inverts the photo image\n";
            str += "GRAYSCALE        -- generates a grayscale image\n";
            str += "COLOR PIXELLATED -- draws pixellated color image using current font dims\n";
            str += "GRAY PIXELLATED  -- draws pixellated gray imamge using current font dims\n";
            str += "ASCII ART        -- draws the ascii art for the selected photo\n";
            str += "CLEAR IMAGE      -- deletes the current modified image\n";
            str += "------DATA-------------------------------------------------------------------------\n";
            str += "DISPLAY FONT RATIOS     -- display currently loaded font ratio set\n";
            str += "\n";
            str += "------MOUSE CONTROLS------------------------------------------------------------\n";
            str += "RIGHT-CLICK:     -- Draws the ASCII Art Image (shortcut)\n";
            str += "LEFT-CLICK:     -- Displays the pixel coordinates of the clicked point\n";
            str += "--------------------------------------------------------------------------------\n";


            MessageBox.Show(str);
        }

        public void MenuAbout_Click(object sender, RoutedEventArgs e)
        {
            string strText = "";
            strText += "Ascii Art Converter\n";
            strText += "by Jim Allen\n";
            strText += "Copyright June 2018\n";
            MessageBox.Show(strText);
        }

        // A utility function to display string data (mostly for testing purposes) and to
        // display a WriteableBitmap on another window;
        public static void DisplayData(string strData, WriteableBitmap wbmap, RenderTargetBitmap rtbmap)
        {
            string strText = "";
            DataWindow = new Window();
            DataWindow.Width = 1200;
            DataWindow.Height = 800;
            DataWindow.FontFamily = new System.Windows.Media.FontFamily("Consolas");
            
            ScrollViewer viewer = new ScrollViewer();
            StackPanel someStackPanel = new StackPanel();

            TextBox txtDataText = new TextBox();
            txtDataText.Width = 1200;
            txtDataText.Name = "txtDataText";
            txtDataText.Text = "Window W: " + DataWindow.Width + "H: " + DataWindow.Height;
            someStackPanel.Children.Add(txtDataText);
            
            Button btnClickMe = new Button();
            btnClickMe.Height = 50;
            btnClickMe.Width = 200;
            btnClickMe.Content = "Done";
            btnClickMe.Name = "BtnNewWindowClose_Click";
            btnClickMe.Click += new RoutedEventHandler(BtnNewWindowClose_Click);
            someStackPanel.Children.Add(btnClickMe);
            
            if (!(strData == null))
            {
                TextBox txtNumber = new TextBox();
                txtNumber.Name = "txtNumber";
                if (strData == "")
                {
                    strData = "No Data To Display at this time";
                }
                else
                {
                    txtNumber.Text = strData;
                }
                someStackPanel.Children.Add(txtNumber);
            }

            if(!(wbmap == null))
            {
                System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                img.Name = "imgSource";
                img.Width = 1024;
                img.Height = 1024;
                img.Source = wbmap;
                someStackPanel.Children.Add(img);
                strText = "Window W: " + DataWindow.Width + "   H: " + DataWindow.Height + "\n" 
                    + "IMG W: " + img.Width + "     H: " + img.Height + "\n"+ "WBMAP W: " + wbmap.Width + "   H: " + wbmap.Height;
                txtDataText.Text = strText;
            } 

            if(!(rtbmap == null))
            {
                System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                img.Name = "imgSource";
                img.Width = 1024;
                img.Height = 1024;
                img.Source = rtbmap;
                someStackPanel.Children.Add(img);
                strText = "Window W: " + DataWindow.Width + "   H: " + DataWindow.Height + "\n"
                    + "IMG W: " + img.Width + "     H: " + img.Height + "\n" + "WBMAP W: " + rtbmap.Width + "   H: " + rtbmap.Height;
                txtDataText.Text = strText;
            }

            viewer.Content = someStackPanel;
            DataWindow.Content = viewer;
            DataWindow.Show();
        }

        public static void BtnNewWindowClose_Click(object sender, RoutedEventArgs e)
        {
            DataWindow.Close();
        }

        public void HandleMouseMove(object sender, MouseEventArgs e)
        {
            System.Drawing.Point cursorPt = System.Windows.Forms.Control.MousePosition;
            cursorPosition.Text = cursorPt.ToString();
            return;
        }

        private void MouseRightButtonUp_Click(object sender, MouseButtonEventArgs e)
        {
            DrawPixellated(DRAWMODE_ASCIIART);
        }

        private void MouseLeftButtonUp_Click(object sender, MouseButtonEventArgs e)
        {
            // Does nothing at this time
        }

        // creates a formatted text string to be used in the creation of a bitmap of the text string
        private FormattedText GetFormattedText(string sTmp, float typeSize, string sFontStyle)
        {
            FormattedText fmtxt;
            fmtxt = new FormattedText(sTmp, System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight, new Typeface(sFontStyle), typeSize, System.Windows.Media.Brushes.Black);
            return fmtxt;
        }

        // function to save an image to disk.
        public void SaveConvertedPhotoToDisk(string fileName)
        { 
            BitmapSource bmp = (BitmapSource)convertedPhoto.Source;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            BitmapFrame outputFrame = BitmapFrame.Create(bmp);
            encoder.Frames.Add(outputFrame);
            encoder.QualityLevel = 100;

            using (FileStream file = File.OpenWrite(fileName))
            {
                encoder.Save(file);
            }
        }

        public WriteableBitmap DrawTextMap(string str)
        {
            double width = fontpixelwidth * PixellatedWidth;
            double height = fontpixelheight * PixellatedHeight;

            DrawingVisual vis = new DrawingVisual();
            DrawingContext dc = vis.RenderOpen();

            //setup our font info conversion for font statistics to font pixels...
            FontInfo finfo = new FontInfo(source, currentFont);
            //finfo.Display();

            RenderTargetBitmap bmpNewImage = new RenderTargetBitmap(
                (int)(width),
                (int)(height), 96, 96, PixelFormats.Pbgra32);

            FormattedText fmText;
            fmText = GetFormattedText(str, currentFont.SizeInPoints, currentFont.Name);

            // Create rectangle.  Must create a white rectangle first to clear the transparency alpha bit of the image.
            // If this rectangle isn't used, the saved image will be black in the file.
            dc.DrawRectangle(new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 255, 255)),
                new System.Windows.Media.Pen(new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 255, 255)), 3),
                new Rect(0, 0, (int)(width), (int)(height)));

            dc.DrawText(fmText, new System.Windows.Point(0.0, 0.0));

            dc.Close();
            bmpNewImage.Render(vis);

            // create a bitmap from RenderTargetBitmap
            MemoryStream stream = new MemoryStream();
            BitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmpNewImage));
            encoder.Save(stream);
            Bitmap bmap = new Bitmap(stream);

            // Now convert our bitmap to a writeable bitmap
            BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmap.GetHbitmap(),
                IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            WriteableBitmap wbmap = new WriteableBitmap(bitmapSource);

            return wbmap;
        }

        // Utility function to convert a Bitmap to a BitmapSource
        public static BitmapSource ConvertBitmap(Bitmap source)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                          source.GetHbitmap(),
                          IntPtr.Zero,
                          Int32Rect.Empty,
                          BitmapSizeOptions.FromEmptyOptions());
        }

        // Utility function to convert a BitmapSource to a Bitmap
        public static Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            Bitmap bitmap;
            using (var outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new Bitmap(outStream);
            }
            return bitmap;
        }

        public static void SetPixel(WriteableBitmap wbm, int x, int y, System.Drawing.Color c)
        {
            if (y > wbm.PixelHeight - 1 || x > wbm.PixelWidth - 1) return;
            if (y < 0 || x < 0) return;
            if (!wbm.Format.Equals(PixelFormats.Pbgra32)) return;
            wbm.Lock();
            IntPtr buff = wbm.BackBuffer;
            int Stride = wbm.BackBufferStride;
            unsafe
            {
                byte* pbuff = (byte*)buff.ToPointer();
                int loc = y * Stride + x * 4;
                pbuff[loc] = c.B;
                pbuff[loc + 1] = c.G;
                pbuff[loc + 2] = c.R;
                pbuff[loc + 3] = c.A;
            }
            wbm.AddDirtyRect(new Int32Rect(x, y, 1, 1));
            wbm.Unlock();
        }
       
        public static System.Drawing.Color GetPixel(WriteableBitmap wbm, int x, int y)
        {
            if (y > wbm.PixelHeight - 1 || x > wbm.PixelWidth - 1)
                return System.Drawing.Color.FromArgb(0, 0, 0, 0);
            if (y < 0 || x < 0)
                return System.Drawing.Color.FromArgb(0, 0, 0, 0);
            if (!wbm.Format.Equals(PixelFormats.Pbgra32))
                return System.Drawing.Color.FromArgb(0, 0, 0, 0); ;
            IntPtr buff = wbm.BackBuffer;
            int Stride = wbm.BackBufferStride;
            System.Drawing.Color c;
            unsafe
            {
                byte* pbuff = (byte*)buff.ToPointer();
                int loc = y * Stride + x * 4;
                c = System.Drawing.Color.FromArgb(pbuff[loc + 3], pbuff[loc + 2],
                                       pbuff[loc + 1], pbuff[loc]);
            }
            return c;
        }

        public MainWindow()
        {
            InitializeComponent();
            SetupWindow();   // sets some basic items such as a default font size

            //FontRatioList.GenerateFontList(); // creates the font list and ratios by reading from the default file, or recomputing the ratios.
        }
    }
}
