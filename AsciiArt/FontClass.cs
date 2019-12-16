using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace AsciiArt
{
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
        // OurArrayMap -- '~' indicates an empty field.  Array is 4 rows of 26 different characters
        private const string letterArrayRow = "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                                              "abcdefghijklmnopqrstuvwxyz" +
                                              "1234567890-=[]~~',./~~~~~~" +
                                              "!@#$%^&*()_+{}|:~<>?~~~~~~";

        //Attributes
        public List<FontRatioData> DataList;

        //constructor
        public FontRatio()
        {
            DataList = new List<FontRatioData>();
        }

        public void GenerateFontList()
        {
            string line;
            //int counter = 0;

            string path = @"../../defaultfontratio.txt";

            if (File.Exists(path))
            {
                // Read the file and display it line by line.  
                System.IO.StreamReader file =
                    new System.IO.StreamReader(path);
                while ((line = file.ReadLine()) != null)
                {
                    string[] words;
                    words = line.Split(' ');

                    FontRatioData frd = new FontRatioData();
                    frd.character = words[0];
                    frd.ratio = Convert.ToDouble(words[1]);

                    MainWindow.FontRatioList.DataList.Add(frd);
                }
                file.Close();

                // now clean up the list, purge tildes, scale from 0 - 1, and sort the list
                this.ProcessList();

                //now invert the ratios 
                foreach (FontRatioData tempData in MainWindow.FontRatioList.DataList)
                {
                    tempData.ratio = 1 - tempData.ratio;
                }

                // now sort the list
                this.SortAscending();
            }
            this.SortAscending();
        }

        public void SortAscending()
        {
            DataList.Sort(delegate (FontRatioData x, FontRatioData y)
            {
                return (x.ratio.CompareTo(y.ratio));
            });
        }

        public string DisplayAll()
        {
            // Display our ratio data (For Testing)
            string strFont = "";
            int tempcount = 0;
            foreach (FontRatioData data in this.DataList)
            {
                strFont += data.DisplayDataItem();
                tempcount++;
                if ((tempcount % 5) == 0)
                {
                    strFont += "\n";
                }
            }
            strFont += "\n-----------------------------------------------------------------------------------\n";
            return strFont;
        }

        // removes tildes and scales our icons
        public void ProcessList()
        {
            this.PurgeTildes();
            this.ScaleRatios();
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

        public void PurgeTildes()
        {
            //List<FontRatioData> tempList = this.DataList;
            for (int i = MainWindow.FontRatioList.DataList.Count - 1; i >= 0; i--)
            {
                if (MainWindow.FontRatioList.DataList[i].character == "~")
                {
                    DataList.RemoveAt(i);
                }
            }
        }

        public async void WriteTextAsync()
        {
            string filename = "defaultfontratio.txt";
            string path = @"../..";

            string text = "";
            foreach (FontRatioData data in this.DataList)
            {
                text += data.character + " " + data.ratio + "\r\n";
            }

            // Write the text asynchronously to a new file named "WriteTextAsync.txt".
            using (StreamWriter outputFile = new StreamWriter(System.IO.Path.Combine(path, filename)))
            {
                await outputFile.WriteAsync(text);
            }
        }


    }
}
