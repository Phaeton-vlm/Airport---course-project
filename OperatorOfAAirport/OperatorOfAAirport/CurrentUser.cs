using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Data;

namespace OperatorOfAAirport
{
    static class CurrentUser
    {
        public static short CurrentID { get; set; }
        public static string FirstName { get; set; }
        public static string SecondName { get; set; }
        public static string connectionString = @"Data Source=(local);Initial Catalog=AirportDB;Integrated Security=True";
        //public static string connectionString = sqlcl;
        //AttachDbFilename=DBFiles\AirportDB.mdf; Initial Catalog=AirportDB;

        public static void ChangeConStr(string ConSt)
        {
            connectionString = ConSt;
        }
    
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
                       select new { airc.AircraftID, airc.AircraftModel,airc.IsFree };

            var airlines = from airln in db.airlines
                           select new { airln.AirlineID, airln.AirlineName };


            foreach (var item in airlines)
            {
                Airl.Items.Add(item.AirlineName);
                AirlINV.Items.Add(item.AirlineID);
            }

            foreach (var item in aircrafts)
            {
                if (item.IsFree)
                {
                    Aircr.Items.Add(item.AircraftModel);
                    AircrINV.Items.Add(item.AircraftID);
                }
            }

            return null;
        }

        public static ComboBox GetOperators(ref ComboBox _Operators)
        {
            MyDataContext db = new MyDataContext(connectionString);
            var Qoperators = from op in db.operators
                            select new {op.Login };

            foreach (var item in Qoperators)
            {
                _Operators.Items.Add($"{item.Login}");
            }
            _Operators.SelectedIndex = 0;
            return null;
        }

    }
}
