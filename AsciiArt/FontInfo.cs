using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Media;
using System.Globalization;

namespace AsciiArt
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
        //       public static System.Windows.Size GetScreenSize(string text, System.Windows.Media.FontFamily fontFamily, double fontSize, System.Windows.FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch)

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
}
