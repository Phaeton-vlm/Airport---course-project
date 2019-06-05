using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OperatorOfAAirport
{
    /// <summary>
    /// Логика взаимодействия для Page3.xaml
    /// </summary>
    public partial class Page3 : Page
    {
        public Page3()
        {
            InitializeComponent();

            MyDataContext dboperator = new MyDataContext(CurrentUser.connectionString);
            var airlines = dboperator.airlines;

            foreach (var item in airlines)
            {
                DataGridAirline.Items.Add(item);
            }
        }

        private void ButtonClick_AddAirline(object sender, RoutedEventArgs e)
        {
            if (_TextBlockAirlineName.Text.Length > 0 && _TextBlockCounty.Text.Length > 0)
            {
                try
                {
                    MyDataContext dboperator = new MyDataContext(CurrentUser.connectionString);

                    Airline airlinenew = new Airline
                    {
                        AirlineName = _TextBlockAirlineName.Text.Length == 0 ? null : _TextBlockAirlineName.Text,
                        Country = _TextBlockCounty.Text.Length == 0 ? null : _TextBlockCounty.Text,
                    };

                    dboperator.airlines.InsertOnSubmit(airlinenew);
                    dboperator.SubmitChanges();

                    //TextBlockMessgeAddAirline = CurrentUser.ResetColor(TextBlockMessgeAddAirline);

                    ClearTextValues();

                    TextBlockMessgeAddAirline.Text = "Авиакомпания добавлена";
                    TextBlockMessgeAddAirline.Visibility = Visibility.Visible;

                    DataGridAirline.Items.Add(airlinenew);
                }
                catch (System.Data.SqlClient.SqlException)
                {
                   // TextBlockMessgeAddAirline.Foreground = Brushes.Red;
                    TextBlockMessgeAddAirline.Visibility = Visibility.Visible;
                    TextBlockMessgeAddAirline.Text = $"Имя {_TextBlockAirlineName.Text} занято";
                }
            }
            else
            {
                //TextBlockMessgeAddAirline.Foreground = Brushes.Red;
                TextBlockMessgeAddAirline.Visibility = Visibility.Visible;
                TextBlockMessgeAddAirline.Text = "Заполнены не все поля";
            }
        }

        private void ButtonClick_OpenAddAirline(object sender, RoutedEventArgs e)
        {
            TextBlockMessgeAddAirline.Visibility = Visibility.Hidden;
            ClearTextValues();
        }

        void ClearTextValues()
        {
            _TextBlockAirlineName.Clear();
            _TextBlockCounty.Clear();
        }

        private void ButtonClick_DeleteAirline(object sender, RoutedEventArgs e)
        {
            try
            {
                ErrorTextBlock.Visibility = Visibility.Collapsed;

                MyDataContext dboperator = new MyDataContext(CurrentUser.connectionString);
                int index = DataGridAirline.SelectedIndex;
                IList delairline = DataGridAirline.SelectedItems;

                dboperator.ExecuteCommand("DELETE FROM Airline where AirlineID = {0}", (delairline[0] as Airline).AirlineID);
                DataGridAirline.Items.RemoveAt(index);
            }
            catch (Exception)
            {
                ErrorTextBlock.Visibility = Visibility.Visible;
            }

            Message.IsOpen = false;
        }

        private void _TextBlockAirlineName_KeyDown(object sender, KeyEventArgs e)
        {
            if (TextBlockMessgeAddAirline.Visibility == Visibility.Visible)
            {
                TextBlockMessgeAddAirline.Visibility = Visibility.Hidden;
            }
        }

        private void DataGridAirline_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ErrorTextBlock.Visibility == Visibility.Visible)
            {
                ErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

    }
}
