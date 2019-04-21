using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace OperatorOfAAirport
{
    static class CurrentUser
    {
        public static short CurrentID { get; set; }

        public static TextBlock ResetColor(TextBlock textBlock)
        {
            var palette = new MaterialDesignThemes.Wpf.PaletteHelper().QueryPalette();
            var hue = palette.PrimarySwatch.PrimaryHues.ToArray()[palette.PrimaryDarkHueIndex];
            textBlock.Foreground = new SolidColorBrush(hue.Color);

            return textBlock;
        }

        public static short[] LoadComboBoxHours()
        {
            short[] Hours= new short[25];

            for (short i = 0; i < 25; i++)
            {
                Hours[i] = i;
            }

            return Hours;
        }

        public static short[] LoadComboBoxMinutes()
        {
            short[] Min = new short[60];

            for (short i = 0; i < 60; i++)
            {
                Min[i] = i;
            }

            return Min;
        }



    }
}
