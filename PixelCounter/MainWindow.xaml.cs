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
using System.Globalization;
using System.Drawing.Imaging;

namespace PixelCounter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class FontInfo
        {
            // Heights and positions in pixels.
            public float EmHeightPixels;
            public float AscentPixels;
            public float DescentPixels;
            public float CellHeightPixels;
            public float InternalLeadingPixels;
            public float LineSpacingPixels;
            public float ExternalLeadingPixels;

            // Distances from the top of the cell in pixels.
            public float RelTop;
            public float RelBaseline;
            public float RelBottom;

            // Initialize the properties.
            public FontInfo(WriteableBitmap gr, Font the_font)
            {
                float em_height =
                    the_font.FontFamily.GetEmHeight(the_font.Style);
                EmHeightPixels = ConvertUnits(gr, the_font.Size,
                    the_font.Unit, GraphicsUnit.Pixel);
                float design_to_pixels = EmHeightPixels / em_height;

                AscentPixels = design_to_pixels *
                    the_font.FontFamily.GetCellAscent(the_font.Style);
                DescentPixels = design_to_pixels *
                    the_font.FontFamily.GetCellDescent(the_font.Style);
                CellHeightPixels = AscentPixels + DescentPixels;
                InternalLeadingPixels = CellHeightPixels - EmHeightPixels;
                LineSpacingPixels = design_to_pixels *
                    the_font.FontFamily.GetLineSpacing(the_font.Style);
                ExternalLeadingPixels =
                    LineSpacingPixels - CellHeightPixels;

                RelTop = InternalLeadingPixels;
                RelBaseline = AscentPixels;
                RelBottom = CellHeightPixels;
            }

            // Convert from one type of unit to another.
            // I don't know how to do Display or World.
            private float ConvertUnits(WriteableBitmap gr, float value,
                GraphicsUnit from_unit, GraphicsUnit to_unit)
            {
                if (from_unit == to_unit) return value;

                // Convert to pixels. 
                switch (from_unit)
                {
                    case GraphicsUnit.Document:
                        value *= (float)(gr.DpiX / 300);
                        break;
                    case GraphicsUnit.Inch:
                        value *= (float)gr.DpiX;
                        break;
                    case GraphicsUnit.Millimeter:
                        value *= (float)(gr.DpiX / 25.4F);
                        break;
                    case GraphicsUnit.Pixel:
                        // Do nothing.
                        break;
                    case GraphicsUnit.Point:
                        value *= (float)(gr.DpiX / 72);
                        break;
                    default:
                        throw new Exception("Unknown input unit " +
                            from_unit.ToString() + " in FontInfo.ConvertUnits");
                }

                // Convert from pixels to the new units. 
                switch (to_unit)
                {
                    case GraphicsUnit.Document:
                        value /= (float)(gr.DpiX / 300);
                        break;
                    case GraphicsUnit.Inch:
                        value /= (float)gr.DpiX;
                        break;
                    case GraphicsUnit.Millimeter:
                        value /= (float)gr.DpiX / 25.4F;
                        break;
                    case GraphicsUnit.Pixel:
                        // Do nothing.
                        break;
                    case GraphicsUnit.Point:
                        value /= (float)gr.DpiX / 72;
                        break;
                    default:
                        throw new Exception("Unknown output unit " +
                            to_unit.ToString() + " in FontInfo.ConvertUnits");
                }

                return value;
            }

            // utility to get the width of a particular glyph.
            public System.Windows.Size GetGlyphDims(string text, System.Windows.Media.FontFamily fontFamily, double fontSize, System.Windows.FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch)
            {
                fontFamily = fontFamily ?? new TextBlock().FontFamily;
                fontSize = fontSize > 0 ? fontSize : new TextBlock().FontSize;
                var typeface = new Typeface(fontFamily, fontStyle, fontWeight, fontStretch);
                var ft = new FormattedText(text ?? string.Empty, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, fontSize, System.Windows.Media.Brushes.Black);
                //return (float)ft.Width;
                return new System.Windows.Size(ft.Width, ft.Height);
            }

            public void Display()
            {
                string str = "EmHeightPixels: " + EmHeightPixels + "\n" +
                    "AscentPixels: " + AscentPixels + "\n" +
                    "DescentPixels: " + DescentPixels + "\n" +
                    "CellHeightPixels: " + CellHeightPixels + "\n" +
                    "InernalLeadingPixels: " + InternalLeadingPixels + "\n" +
                    "LineSpacingPixels: " + LineSpacingPixels + "\n" +
                    "ExternalLeadingPixels: " + ExternalLeadingPixels + "\n" +
                    "RelTop: " + RelTop + "\n" +
                    "RelBaseline: " + RelBaseline + "\n" +
                    "RelBottom: " + RelBottom + "\n";

                MessageBox.Show(str);
            }
        }

        public class FontRatioData
        {
            public double ratio;               // the darkness ratio for our character
            public string character;           // symbolic character
            public WriteableBitmap wbmap;      // our bitmap for the current character
            public int blackpixelcount;        // store the number of black pixels in our character (for testing)

            // constructor
            public FontRatioData()
            {
                ratio = 0.0f;               // default ratio
                character = "~";            // default character
                blackpixelcount = 0;
            }

            // Method
            public string DisplayDataItem()
            {
                string str = "";
                str += string.Format("{0,2}", character) + " : " + string.Format("{0,5}", blackpixelcount) + " : " + string.Format("{0,20}", this.ratio) + "  |   ";
                return str;
            }
        }

        public class FontRatio
        {
            //Attributes
            public List<FontRatioData> DataList;

            //constructor
            public FontRatio()
            {
                DataList = new List<FontRatioData>();
            }

            public void SortAscending()
            {
                DataList.Sort(delegate (FontRatioData x, FontRatioData y)
                {
                    return (x.ratio.CompareTo(y.ratio));
                });
            }

            public void ScaleRatios()
            {
                double max_value = 0.0;
                foreach (FontRatioData data in this.DataList)
                {
                    // find the max value
                    if (data.ratio > max_value)
                        max_value = data.ratio;
                }
                foreach (FontRatioData data in this.DataList)
                {
                    data.ratio = (data.ratio / max_value);
                }
            }

            public string DisplayAll()
            {
                int count = 0;
                string str = "";
                foreach(FontRatioData item in DataList)
                {
                    count++;
                    str += item.DisplayDataItem();
                    if ((count % 5) == 0)
                    {
                        str += "\n";
                    }
                }
                return str;
            }
        }

        // Attributes
        public static Font currentFont;
        public static FontRatio FontRatioList;
        public static FontInfo fontInfo;
        public static Window DataWindow;  // create a holder for a data window
        private static WriteableBitmap source;  // stores our source photo for the app
        public static float fontpixelwidth;
        public static float fontpixelheight;

        private void ComputeFontPixelInfo()
        {
            System.Windows.Media.FontFamily mediaFontFamily = new System.Windows.Media.FontFamily(currentFont.Name);
            FontInfo finfo = new FontInfo(source, currentFont);
            System.Windows.Size fontsizepixeldims = finfo.GetGlyphDims("X", mediaFontFamily, currentFont.Size, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
            fontpixelwidth = (float)fontsizepixeldims.Width;
            fontpixelheight = (float)fontsizepixeldims.Height;
        }

        //Methods
        private void SetupWindow()
        {
            // set a blank image in our viewer source of type WriteableBitmap
            WriteableBitmap blanksourcebmap = new WriteableBitmap(512, 512, 96, 96, PixelFormats.Pbgra32, null);
            source = blanksourcebmap;  // save our blank image.
            PixelImage.Source = source;

            FontRatioList = new FontRatio();

            // Set current default window font information
            System.Drawing.FontFamily fontFamily = new System.Drawing.FontFamily("Consolas");
            currentFont = new Font(fontFamily, 20.0F, System.Drawing.FontStyle.Regular);

            // compute our font pixel info
            ComputeFontPixelInfo();
        }

        // creates a formatted text string to be used in the creation of a bitmap of the text string
        private FormattedText GetFormattedText(string sTmp, float typeSize, string sFontStyle)
        {
            FormattedText fmtxt;
            fmtxt = new FormattedText(sTmp, System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight, new Typeface(sFontStyle), typeSize, System.Windows.Media.Brushes.Black);
            return fmtxt;
        }
        public WriteableBitmap DrawTextMap(string str)
        {
            DrawingVisual vis = new DrawingVisual();
            DrawingContext dc = vis.RenderOpen();

            //setup our font info conversion for font statistics to font pixels...
            FontInfo finfo = new FontInfo(source, currentFont);
            //finfo.Display();

            RenderTargetBitmap bmpNewImage = new RenderTargetBitmap(
                (int)(fontpixelwidth),
                (int)(fontpixelheight), 96, 96, PixelFormats.Pbgra32);

            FormattedText fmText;
            fmText = GetFormattedText(str, currentFont.SizeInPoints, currentFont.Name);

            // Create rectangle.  Must create a white rectangle first to clear the transparency alpha bit of the image.
            // If this rectangle isn't used, the saved image will be black in the file.
            dc.DrawRectangle(new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 255, 255)),
                new System.Windows.Media.Pen(new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 255, 255)), 3),
                new Rect(0, 0, (int)(fontpixelwidth), (int)(fontpixelheight)));

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

        public static System.Drawing.Color GetPixel(WriteableBitmap wbm, int x, int y)
        {
            if (y > wbm.PixelHeight - 1 || x > wbm.PixelWidth - 1)
            {
                return System.Drawing.Color.FromArgb(0, 0, 0, 0);
            }
            if (y < 0 || x < 0)
            {
                return System.Drawing.Color.FromArgb(0, 0, 0, 0);
            }

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

        private void MenuFontRatioDisplay_Click(object sender, RoutedEventArgs e)
        {
            string templist = currentFont.Name + " -- Size: " + currentFont.Size + "\n";
            templist += "---------------------------------------------------------------\n";
            templist += FontRatioList.DisplayAll();
            templist += "---------------------------------------------------------------\n";
            MessageBox.Show(templist);
            return;
        }

        private void MenuFont_Click(object sender, RoutedEventArgs e)
        {
            // Delete our current Font Ratio List....
            FontRatioList.DataList.Clear();

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

                // Set current default window font information
                System.Drawing.FontFamily fontFamily = new System.Drawing.FontFamily(currentFont.Name);
                currentFont = new Font(fontFamily, currentFont.Size, System.Drawing.FontStyle.Regular);

                // compute our font pixel info
                ComputeFontPixelInfo();

                string ASCIIChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz`01234567890-=[]',./~!@#$%^&*()_+{}|:<>?";
                for (int i = 0; i < ASCIIChars.Length; i++)
                {
                    string str = ASCIIChars.Substring(i, 1);
                    WriteableBitmap mywbmap = DrawTextMap(str).Clone();

                    int pixelCount = 0;
                    float red = 0;
                    float green = 0;
                    float blue = 0;
                    for (int n = 0; n < (int)fontpixelheight; n++)
                    {
                        for (int m = 0; m < (int)fontpixelwidth; m++)
                        {
                            System.Drawing.Color color = GetPixel(mywbmap, m, n);
                            red += color.R;
                            green += color.G;
                            blue += color.B;
                            pixelCount++;
                        }
                    }
                    red = 1.0f - red / (255.0f * pixelCount);
                    green = 1.0f - green / (255.0f * pixelCount);
                    blue = 1.0f - blue / (255.0f * pixelCount);
                    FontRatioData fontratioitem = new FontRatioData();
                    fontratioitem.character = str;
                    fontratioitem.ratio = red;

                    FontRatioList.DataList.Add(fontratioitem);
                    PixelImage.Source = mywbmap;

                }
                FontRatioList.ScaleRatios();    // scale our values from 0-1.0
                FontRatioList.SortAscending();  // sort our list
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            SetupWindow();
            string ASCIIChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz`01234567890-=[]',./~!@#$%^&*()_+{}|:<>?";
            for (int i=0; i<ASCIIChars.Length; i++)
            {
                string str = ASCIIChars.Substring(i,1);
                WriteableBitmap mywbmap = DrawTextMap(str).Clone();

                int pixelCount = 0;
                float red = 0;
                float green = 0;
                float blue = 0;
                for (int n = 0; n < (int)fontpixelheight; n++)
                {
                    for (int m = 0; m < (int)fontpixelwidth; m++)
                    {
                        System.Drawing.Color color = GetPixel(mywbmap, m, n);
                        red += color.R;
                        green += color.G;
                        blue += color.B;
                        pixelCount++;
                    }
                }
                red = 1.0f - red / (255.0f * pixelCount);
                green = 1.0f - green / (255.0f * pixelCount);
                blue = 1.0f - blue / (255.0f * pixelCount);
                FontRatioData fontratioitem = new FontRatioData();
                fontratioitem.character = str;
                fontratioitem.ratio = red;

                FontRatioList.DataList.Add(fontratioitem);
                PixelImage.Source = mywbmap;
            }
            FontRatioList.ScaleRatios();    // scale our values from 0-1.0
            FontRatioList.SortAscending();  // sort our list
            //string templist = FontRatioList.DisplayAll();
            //MessageBox.Show(templist);
        }
    }
}
