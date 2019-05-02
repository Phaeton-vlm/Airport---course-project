using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;
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
        }

        private void ButtonClick_AddAircraft(object sender, RoutedEventArgs e)
        {
            if (_TextBlockBusinessClass.Text.Length < 1 || _TextBlockEconomyClass.Text.Length < 1 || _TextBlockFirstClass.Text.Length < 1 || _TextBlockModel.Text.Length < 1 || _TextBlockSideNumber.Text.Length < 1 || _TextBlockVipClass.Text.Length < 1)
            {
                TextBlockMessgeAddAircraft.Visibility = Visibility.Visible;
                TextBlockMessgeAddAircraft.Text = "Заполнены не все поля";
            }
            else
            {
                try
                {
                    MyDataContext dboperator = new MyDataContext(connectionString);

                    Aircraft aircraftnew = new Aircraft
                    {
                        AircraftModel = _TextBlockModel.Text,
                        SideNumber = _TextBlockSideNumber.Text,
                        EconomyClass = short.Parse(_TextBlockEconomyClass.Text),
                        BusinessClass = short.Parse(_TextBlockBusinessClass.Text),
                        FirstClass = short.Parse(_TextBlockFirstClass.Text),
                        VIPClass = short.Parse(_TextBlockVipClass.Text),
                        IsFree = true
                    };

                    dboperator.aircrafts.InsertOnSubmit(aircraftnew);
                    dboperator.SubmitChanges();

                    TextBlockMessgeAddAircraft.Text = "Самолет добавлен";
                    TextBlockMessgeAddAircraft.Visibility = Visibility.Visible;

                    ClearTextBoxes();

                    DataGridAircraft.Items.Add(aircraftnew);

                    if (All.IsChecked == true) { Sort(true, 1); return; }
                    if (Free.IsChecked == true) { Sort(true, 0); return; }
                    if (Off.IsChecked == true) { Sort(false, 0); return; }
                }
                catch (Exception)
                {
                    TextBlockMessgeAddAircraft.Visibility = Visibility.Visible;
                    TextBlockMessgeAddAircraft.Text = $"Номер {_TextBlockSideNumber.Text} занят";
                }
            }
        }

        private void _TextBlockEconomyClass_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(char.Parse(e.Text))) { e.Handled = true;  return; }
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            TextBlockMessgeAddAircraft.Visibility = Visibility.Hidden;
            ClearTextBoxes();
        }

        private void ClearTextBoxes()
        {
            _TextBlockSideNumber.Clear();
            _TextBlockModel.Clear();
            _TextBlockEconomyClass.Clear();
            _TextBlockBusinessClass.Clear();
            _TextBlockFirstClass.Clear();
            _TextBlockVipClass.Clear();
        }

        private void ButtonClick_DeleteAircraft(object sender, RoutedEventArgs e)
        {
            try
            {
                ErrorTextBlock.Visibility = Visibility.Collapsed;

                MyDataContext dboperator = new MyDataContext(connectionString);
                int index = DataGridAircraft.SelectedIndex;
                IList delaircraft = DataGridAircraft.SelectedItems;

                if(!(delaircraft[0] as Aircraft).IsFree)
                {
                    ErrorTextBlock.Text = "Операция запрещена пока самолет находится в расписании";
                    ErrorTextBlock.Visibility = Visibility.Visible;                   
                }
                else
                {
                    dboperator.ExecuteCommand("DELETE FROM Aircraft where AircraftID = {0}", (delaircraft[0] as Aircraft).AircraftID);
                    DataGridAircraft.Items.RemoveAt(index);
                }
           
            }
            catch(Exception)
            {
                ErrorTextBlock.Text = "Выделите строку для удаления";
                ErrorTextBlock.Visibility = Visibility.Visible;
            }

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ErrorTextBlock.Visibility = Visibility.Collapsed;

            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
               if(All.IsChecked == true) { Sort(true, 1); return; }
               if(Free.IsChecked == true) { Sort(true, 0); return; }
               if(Off.IsChecked == true) { Sort(false, 0); return; }
            });
        }


        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {           
            switch ((sender as RadioButton).Name)
            {
                case "All":
                    Sort(true,1);
                    break;
                case "Free":
                    Sort(true, 0);
                    break;
                case "Off":
                    Sort(false, 0);
                    break;
            }
        }

        public void Sort(bool par, byte addpar)
        {
            DataGridAircraft.Items.Clear();
            MyDataContext dboperator = new MyDataContext(connectionString);

            var aircrafts = from airc in dboperator.aircrafts
                            select airc;
            if (addpar == 1)
            {
                foreach (var item in aircrafts)
                {
                    DataGridAircraft.Items.Add(item);
                }
                return;
            }
            else
            {
                foreach (var item in aircrafts)
                {
                    if (item.IsFree == par)
                    {
                        DataGridAircraft.Items.Add(item);
                    }
                }
                return;
            }
        }

        private void _TextBlockSideNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (TextBlockMessgeAddAircraft.Visibility == Visibility.Visible)
            {
                TextBlockMessgeAddAircraft.Visibility = Visibility.Hidden;
            }
        }

        private void DataGridAircraft_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ErrorTextBlock.Visibility == Visibility.Visible)
            {
                ErrorTextBlock.Visibility = Visibility.Collapsed;
            }

        }
    }
}
