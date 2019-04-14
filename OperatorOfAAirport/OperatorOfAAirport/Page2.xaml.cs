using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Data.Linq;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OperatorOfAAirport
{
    /// <summary>
    /// Логика взаимодействия для Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        string connectionString = "Data Source=DESKTOP-989RPMD;Initial Catalog=AirportDB;Integrated Security=True";

        public Page2()
        {
            InitializeComponent();

            MyDataContext dboperator = new MyDataContext(connectionString);
            var aircrafts = from airc in dboperator.aircrafts
                            select new { airc.AircraftModel, airc.BusinessClass, airc.EconomyClass, airc.FirstClass, airc.IsFree, airc.SideNumber, airc.VIPClass };

            DataGridAircraft.ItemsSource = aircrafts.ToList();
        }

        private void ButtonClick_AddAircraft(object sender, RoutedEventArgs e)
        {         
            try
            {
                MyDataContext dboperator = new MyDataContext(connectionString);

                Aircraft aircraftnew = new Aircraft { AircraftModel = _TextBlockModel.Text, SideNumber = _TextBlockSideNumber.Text, EconomyClass = short.Parse(_TextBlockEconomyClass.Text),
                    BusinessClass = short.Parse(_TextBlockBusinessClass.Text), FirstClass = short.Parse(_TextBlockFirstClass.Text), VIPClass = short.Parse(_TextBlockVipClass.Text), IsFree = true };

                dboperator.aircrafts.InsertOnSubmit(aircraftnew);
                dboperator.SubmitChanges();

                ChangeColor();

                TextBlockMessgeAddAircraft.Text = "Самолет добавлен";
                TextBlockMessgeAddAircraft.Visibility = Visibility.Visible;
            }
            catch (FormatException)
            {
                TextBlockMessgeAddAircraft.Foreground = Brushes.Red;
                TextBlockMessgeAddAircraft.Visibility = Visibility.Visible;
                TextBlockMessgeAddAircraft.Text = "Заполнены не все поля";
            }
            catch (Exception)
            {
                TextBlockMessgeAddAircraft.Foreground = Brushes.Red;
                TextBlockMessgeAddAircraft.Visibility = Visibility.Visible;
                TextBlockMessgeAddAircraft.Text = $"Бортовой номер {_TextBlockSideNumber.Text} уже существует";
            }
        }

        private void _TextBlockEconomyClass_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(char.Parse(e.Text))) { e.Handled = true;  return; }
            //if((sender as TextBox).Text.Length == 0 && int.Parse(e.Text) == 0) { e.Handled = true; return; }
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {          
            _TextBlockSideNumber.Clear();
            _TextBlockModel.Clear();
            _TextBlockEconomyClass.Clear();
            _TextBlockBusinessClass.Clear();
            _TextBlockFirstClass.Clear();
            _TextBlockVipClass.Clear();
        }

        private void Cuncel_ButtonClick(object sender, RoutedEventArgs e)
        {
            TextBlockMessgeAddAircraft.Visibility = Visibility.Hidden;
        }

        void ChangeColor()
        {
            var palette = new MaterialDesignThemes.Wpf.PaletteHelper().QueryPalette();
            var hue = palette.PrimarySwatch.PrimaryHues.ToArray()[palette.PrimaryDarkHueIndex];
            TextBlockMessgeAddAircraft.Foreground = new SolidColorBrush(hue.Color);
        }
    }
}
