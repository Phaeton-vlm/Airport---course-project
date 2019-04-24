using System;
using System.Data;
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
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        string connectionString = "Data Source=DESKTOP-989RPMD;Initial Catalog=AirportDB;Integrated Security=True";

        public Page1()
        {
            InitializeComponent();

            MyDataContext dboperator = new MyDataContext(connectionString);

            var flights = from fl in dboperator.flights
                          join airc in dboperator.airlines on fl.AirlineID equals airc.AirlineID
                          select new
                          {
                              fl.ArrivalCity,
                              fl.ArrivalTime,
                              fl.DepartureTime,
                              fl.DepatureCity,
                              fl.FlightNumber,
                              airc.AirlineName,
                              fl.FlightID,
                          };

            foreach (var item in flights)
            {
                DataGridFlights.Items.Add(item);
            }

            _ComboBoxHours = CurrentUser.LoadComboBoxHours(_ComboBoxHours);
            _ComboBoxMinutes = CurrentUser.LoadComboBoxMinutes(_ComboBoxMinutes);
            CurrentUser.LoadComboBoxSideNumberandAirline(ref _ComboBoxAirlineName, ref _ComboBoxAirlineNameINV, ref _ComboBoxSideNumber, ref _ComboBoxSideNumberINV);

                     
        }

        private void ButtonClick_AddFlight(object sender, RoutedEventArgs e)
        {
            CurrentUser.LoadComboBoxSideNumberandAirline(ref _ComboBoxAirlineName, ref _ComboBoxAirlineNameINV, ref _ComboBoxSideNumber, ref _ComboBoxSideNumberINV);
        }

        private void ButtonClick_OK(object sender, RoutedEventArgs e)
        {
            try
            {

                MyDataContext dbFlight = new MyDataContext(connectionString);

                DateTime date1;
                date1 = Convert.ToDateTime(DatePiker.Text);
                date1 = date1.AddHours(TimePiker.SelectedTime.Value.Hour);
                date1 = date1.AddMinutes(TimePiker.SelectedTime.Value.Minute);

                DateTime date2 = date1;
                date2 = date2.AddHours(double.Parse(_ComboBoxHours.Text));
                date2 = date2.AddMinutes(double.Parse(_ComboBoxMinutes.Text));



                Flight newflight = new Flight
                {
                    ArrivalCity = _TextBoxArrivalCity.Text,
                    DepatureCity = _TextBoxDepatureCity.Text,
                    FlightNumber = _TextBoxFlightNumber.Text,
                    OperatorID = CurrentUser.CurrentID,
                    AircraftID = short.Parse(_ComboBoxSideNumberINV.Items[_ComboBoxSideNumber.SelectedIndex].ToString()),
                    AirlineID = short.Parse(_ComboBoxAirlineNameINV.Items[_ComboBoxAirlineName.SelectedIndex].ToString()),
                    DepartureTime = date1,
                    ArrivalTime = date2,
                };

                dbFlight.flights.InsertOnSubmit(newflight);
                dbFlight.SubmitChanges();

                TextBlockMessgeAddFlight = CurrentUser.ResetColor(TextBlockMessgeAddFlight);
              
                TextBlockMessgeAddFlight.Text = "Рейс добавлен";
                TextBlockMessgeAddFlight.Visibility = Visibility.Visible;


                var flights = from fl in dbFlight.flights
                              join airc in dbFlight.airlines on fl.AirlineID equals airc.AirlineID
                              where fl.FlightNumber == _TextBoxFlightNumber.Text
                              select new
                              { fl.ArrivalCity, fl.ArrivalTime, fl.DepartureTime, fl.DepatureCity, fl.FlightNumber, airc.AirlineName,fl.FlightID };

       
                DataGridFlights.Items.Add(flights);
                             
            }
            catch (System.Data.SqlClient.SqlException)
            {
                TextBlockMessgeAddFlight.Foreground = Brushes.Red;
                TextBlockMessgeAddFlight.Visibility = Visibility.Visible;
                TextBlockMessgeAddFlight.Text = $"Рейс с номером {_TextBoxFlightNumber.Text} уже существует";
            }
            catch (Exception)
            {
                TextBlockMessgeAddFlight.Foreground = Brushes.Red;
                TextBlockMessgeAddFlight.Visibility = Visibility.Visible;
                TextBlockMessgeAddFlight.Text = "Заполнены не все поля";

            }
        }

        private void ButtonClick_DelFlight(object sender, RoutedEventArgs e)
        {
            try
            {
                MyDataContext dboperator = new MyDataContext(connectionString);
                int index = DataGridFlights.SelectedIndex;
              
                dynamic std = DataGridFlights.SelectedItems;
                short ind = std[0].FlightID;

                dboperator.ExecuteCommand("DELETE FROM Flight where FlightID = {0}", ind);
                DataGridFlights.Items.RemoveAt(index);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
