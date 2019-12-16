using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.IO;

namespace AsciiArt
{
    // stores the information of our AsciiMap (should help with reconstruction)
    public class AsciiMapItem
    {
        public double ratio;
        public int x;
        public int y;
        public FontRatioData data;         // the data of the character at this map location

        public AsciiMapItem(int x, int y, double ratio)
        {
            this.x = x;
            this.y = y;
            this.ratio = ratio;
            this.data = new FontRatioData();
        }

        public void AssignCharacter()
        {
            //now find the font ratio element structure that defines this block.
            int index = MainWindow.FontRatioList.DataList.Count - 1; // set our default to the last index value in the list (should be the largest ratio)
            if (index <= 0)
            {
                MessageBox.Show("FontRatioList has an invalid length.  Did you compute the ratios?");
                return;
            }

            foreach(FontRatioData data in MainWindow.FontRatioList.DataList)
            {
                // Not the most efficient way to assign characters
                if(this.ratio >= data.ratio)
                {
                    this.data.character = data.character;
                    this.data.wbmap = data.wbmap;
                } else
                {
                    break;
                }
            }
        }

        public string DisplayDataItem()
        {
            string str = "";
            str += "(" + x + ", " + y + ")" + string.Format("{0,2}", data.character) + " : " + ratio + "      | ";
            return str;
        }
    }

    public class AsciiMap
    {
        public string CurrentMap;
        public string PreviousMap;
        public int MapWidth;    // the pixel width of our map
        public int MapHeight;   // the pixel height of our map

        public List<AsciiMapItem> AsciiMapList;

        public AsciiMap()
        {
            AsciiMapList = new List<AsciiMapItem>();
            CurrentMap = "";
            PreviousMap = "";
            MapWidth = 0;
            MapHeight = 0;
        }

        // assigns characters to all in the list
        public void AssignCharactersToAll()
        {
            foreach (AsciiMapItem item in AsciiMapList)
            {
                item.AssignCharacter();
            }
        }

        public string DisplayAll()
        {
            string str = "";
            int tempcount = 0;
            foreach (AsciiMapItem data in this.AsciiMapList)
            {
                str += data.DisplayDataItem();
                tempcount++;
                if ((tempcount % MainWindow.PixellatedWidth) == 0)
                {
                    str += "\n";
                }
            }
            str += "\n------------------------------------------------------------------------------------\n";
            return str;
        }

        public string DrawMapString()
        {
            PreviousMap = CurrentMap;
            string str = "";
            int tempcount = 0;
            foreach (AsciiMapItem item in this.AsciiMapList)
            {
                str += string.Format("{0,1}", item.data.character);
                tempcount++;
                if ((tempcount % MainWindow.PixellatedWidth) == 0)
                {
                    str += "\n";
                }
            }
            CurrentMap = str;
            return str;
        }
    }
}
