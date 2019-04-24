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
        static string connectionString = "Data Source=DESKTOP-989RPMD;Initial Catalog=AirportDB;Integrated Security=True";


        public static TextBlock ResetColor(TextBlock textBlock)
        {
            var palette = new MaterialDesignThemes.Wpf.PaletteHelper().QueryPalette();
            var hue = palette.PrimarySwatch.PrimaryHues.ToArray()[palette.PrimaryDarkHueIndex];
            textBlock.Foreground = new SolidColorBrush(hue.Color);

            return textBlock;
        }

        public static ComboBox LoadComboBoxHours(ComboBox comboBox)
        {

            for (short i = 0; i < 25; i++)
            {
                comboBox.Items.Add(i);
            }

            return comboBox;
        }

        public static ComboBox LoadComboBoxMinutes(ComboBox comboBox)
        {
          
            for (short i = 0; i < 60; i++)
            {
                comboBox.Items.Add(i);
            }

            return comboBox;
        }

        public static ComboBox LoadComboBoxSideNumberandAirline(ref ComboBox Airl, ref ComboBox AirlINV, ref ComboBox Aircr, ref ComboBox AircrINV )
        {
            Airl.Items.Clear();
            AirlINV.Items.Clear();
            Aircr.Items.Clear();
            AircrINV.Items.Clear();

            MyDataContext db = new MyDataContext(connectionString);

            var aircrafts = from airc in db.aircrafts
                       select new { airc.AircraftID, airc.AircraftModel };

            var airlines = from airln in db.airlines
                           select new { airln.AirlineID, airln.AirlineName };


            foreach (var item in airlines)
            {
                Airl.Items.Add(item.AirlineName);
                AirlINV.Items.Add(item.AirlineID);
            }

            foreach (var item in aircrafts)
            {
                Aircr.Items.Add(item.AircraftModel);
                AircrINV.Items.Add(item.AircraftID);
            }

            return null;
        }


    }
}
