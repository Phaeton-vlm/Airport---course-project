using System;
using System.Collections.Generic;
using System.Collections;
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
    /// Логика взаимодействия для Page3.xaml
    /// </summary>
    public partial class Page3 : Page
    {
        string connectionString = "Data Source=DESKTOP-989RPMD;Initial Catalog=AirportDB;Integrated Security=True";

        public Page3()
        {
            InitializeComponent();

            MyDataContext dboperator = new MyDataContext(connectionString);
            var airlines = dboperator.airlines;

            foreach (var item in airlines)
            {
                DataGridAirline.Items.Add(item);
            }
        }

        private void ButtonClick_AddAirline(object sender, RoutedEventArgs e)
        {
            try
            {
                MyDataContext dboperator = new MyDataContext(connectionString);

                Airline airlinenew = new Airline
                {
                    AirlineName = _TextBlockAirlineName.Text.Length == 0? null: _TextBlockAirlineName.Text,
                    Country = _TextBlockCounty.Text.Length == 0 ? null : _TextBlockCounty.Text,
                };

                dboperator.airlines.InsertOnSubmit(airlinenew);
                dboperator.SubmitChanges();

                TextBlockMessgeAddAirline = CurrentUser.ResetColor(TextBlockMessgeAddAirline);

                TextBlockMessgeAddAirline.Text = "Авиакомпания добавлена";
                TextBlockMessgeAddAirline.Visibility = Visibility.Visible;

                DataGridAirline.Items.Add(airlinenew);
            }
            catch (System.Data.SqlClient.SqlException)
            {

                TextBlockMessgeAddAirline.Foreground = Brushes.Red;
                TextBlockMessgeAddAirline.Visibility = Visibility.Visible;
                TextBlockMessgeAddAirline.Text = "Заполнены не все поля";
            }
            catch (Exception)
            {
                TextBlockMessgeAddAirline.Foreground = Brushes.Red;
                TextBlockMessgeAddAirline.Visibility = Visibility.Visible;
                TextBlockMessgeAddAirline.Text = $"Авиакомпания {_TextBlockAirlineName.Text} уже существует";
            }
        }

        private void ButtonClick_OpenAddAirline(object sender, RoutedEventArgs e)
        {
            _TextBlockAirlineName.Clear();
            _TextBlockCounty.Clear();
        }

        private void Cuncel_ButtonClick(object sender, RoutedEventArgs e)
        {
            TextBlockMessgeAddAirline.Visibility = Visibility.Hidden;
        }

        private void ButtonClick_DeleteAirline(object sender, RoutedEventArgs e)
        {
            try
            {
                MyDataContext dboperator = new MyDataContext(connectionString);
                int index = DataGridAirline.SelectedIndex;
                IList delairline = DataGridAirline.SelectedItems;

                dboperator.ExecuteCommand("DELETE FROM Airline where AirlineID = {0}", (delairline[0] as Airline).AirlineID);
                DataGridAirline.Items.RemoveAt(index);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
