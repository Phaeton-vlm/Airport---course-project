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
                              join air in dboperator.aircrafts on fl.AircraftID equals air.AircraftID
                              select new
                              {
                                  fl.ArrivalCity,
                                  fl.ArrivalTime,
                                  fl.DepartureTime,
                                  fl.DepatureCity,
                                  fl.FlightNumber,
                                  airc.AirlineName,
                                  fl.FlightID,
                                  air.AircraftModel,
                                  air.SideNumber
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
            TextBlockMessgeAddFlight.Visibility = Visibility.Hidden;
            ClearValue();
            CurrentUser.LoadComboBoxSideNumberandAirline(ref _ComboBoxAirlineName, ref _ComboBoxAirlineNameINV, ref _ComboBoxSideNumber, ref _ComboBoxSideNumberINV);
        }

        private void ClearValue()
        {
            _TextBoxFlightNumber.Clear();
            _TextBoxDepatureCity.Clear();
            _TextBoxArrivalCity.Clear();
            DatePiker.Text = "";
            TimePiker.Text="";
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
              
                TextBlockMessgeAddFlight.Text = "Рейс добавлен";
                TextBlockMessgeAddFlight.Visibility = Visibility.Visible;
              
               var flights = from fl in dbFlight.flights
                             join airc in dbFlight.airlines on fl.AirlineID equals airc.AirlineID
                             join air in dbFlight.aircrafts on fl.AircraftID equals air.AircraftID
                             where fl.FlightNumber == _TextBoxFlightNumber.Text
                             select new
                             {
                                 fl.ArrivalCity,
                                 fl.ArrivalTime,
                                 fl.DepartureTime,
                                 fl.DepatureCity,
                                 fl.FlightNumber,
                                 airc.AirlineName,
                                 fl.FlightID,
                                 air.AircraftModel,
                                 air.SideNumber
                             };

                foreach (var item in flights)
                {
                    DataGridFlights.Items.Add(item);
                }


                Aircraft Aircr = dbFlight.aircrafts.Where(airc => airc.AircraftID == short.Parse(_ComboBoxSideNumberINV.Items[_ComboBoxSideNumber.SelectedIndex].ToString())).FirstOrDefault();
                Aircr.IsFree = false;

                dbFlight.SubmitChanges();

            }
            catch (System.Data.SqlClient.SqlException)
            {
                TextBlockMessgeAddFlight.Visibility = Visibility.Visible;
                TextBlockMessgeAddFlight.Text = $"Рейс с номером {_TextBoxFlightNumber.Text} уже существует";
            }
            catch (Exception)
            {
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
                    string ind2 = std[0].SideNumber;

                    dboperator.ExecuteCommand("DELETE FROM Flight where FlightID = {0}", ind);
                    DataGridFlights.Items.RemoveAt(index);

                    Aircraft Aircr = dboperator.aircrafts.Where(airc => airc.SideNumber == ind2).FirstOrDefault();
                    Aircr.IsFree = true;

                    dboperator.SubmitChanges();
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButtonClick_Cancel(object sender, RoutedEventArgs e)
        {
            TextBlockMessgeAddFlight.Visibility = Visibility.Hidden;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            QueryFlightNumber();
        }

        void QueryFlightNumber()
        {
            DataGridFlights.Items.Clear();
            MyDataContext dboperator = new MyDataContext(connectionString);

            var flights = from fl in dboperator.flights
                          join airc in dboperator.airlines on fl.AirlineID equals airc.AirlineID
                          join air in dboperator.aircrafts on fl.AircraftID equals air.AircraftID
                          where fl.FlightNumber.Contains(SortBox.Text) || air.SideNumber.Contains(SortBox.Text) || air.AircraftModel.Contains(SortBox.Text) || airc.AirlineName.Contains(SortBox.Text) || fl.DepatureCity.Contains(SortBox.Text)|| fl.ArrivalCity.Contains(SortBox.Text)
                          select new
                          {
                              fl.ArrivalCity,
                              fl.ArrivalTime,
                              fl.DepartureTime,
                              fl.DepatureCity,
                              fl.FlightNumber,
                              airc.AirlineName,
                              fl.FlightID,
                              air.AircraftModel,
                              air.SideNumber
                          };

            foreach (var item in flights)
            {
                DataGridFlights.Items.Add(item);
            }
        }

      /*  void QuerySideNumber()
        {

            DataGridFlights.Items.Clear();
            MyDataContext dboperator = new MyDataContext(connectionString);

            var flights = from fl in dboperator.flights
                          join airc in dboperator.airlines on fl.AirlineID equals airc.AirlineID
                          join air in dboperator.aircrafts on fl.AircraftID equals air.AircraftID
                          where air.SideNumber.Contains(SortBox.Text)
                          select new
                          {
                              fl.ArrivalCity,
                              fl.ArrivalTime,
                              fl.DepartureTime,
                              fl.DepatureCity,
                              fl.FlightNumber,
                              airc.AirlineName,
                              fl.FlightID,
                              air.AircraftModel,
                              air.SideNumber
                          };

            foreach (var item in flights)
            {
                DataGridFlights.Items.Add(item);
            }
        }

        void QueryAircraftModel()
        {
            DataGridFlights.Items.Clear();
            MyDataContext dboperator = new MyDataContext(connectionString);

            var flights = from fl in dboperator.flights
                          join airc in dboperator.airlines on fl.AirlineID equals airc.AirlineID
                          join air in dboperator.aircrafts on fl.AircraftID equals air.AircraftID
                          where air.AircraftModel.Contains(SortBox.Text)
                          select new
                          {
                              fl.ArrivalCity,
                              fl.ArrivalTime,
                              fl.DepartureTime,
                              fl.DepatureCity,
                              fl.FlightNumber,
                              airc.AirlineName,
                              fl.FlightID,
                              air.AircraftModel,
                              air.SideNumber
                          };

            foreach (var item in flights)
            {
                DataGridFlights.Items.Add(item);
            }
        }

        void QueryAirlineName()
        {
            DataGridFlights.Items.Clear();
            MyDataContext dboperator = new MyDataContext(connectionString);

            var flights = from fl in dboperator.flights
                          join airc in dboperator.airlines on fl.AirlineID equals airc.AirlineID
                          join air in dboperator.aircrafts on fl.AircraftID equals air.AircraftID
                          where airc.AirlineName.Contains(SortBox.Text)
                          select new
                          {
                              fl.ArrivalCity,
                              fl.ArrivalTime,
                              fl.DepartureTime,
                              fl.DepatureCity,
                              fl.FlightNumber,
                              airc.AirlineName,
                              fl.FlightID,
                              air.AircraftModel,
                              air.SideNumber
                          };

            foreach (var item in flights)
            {
                DataGridFlights.Items.Add(item);
            }
        }

        void QueryDepatureCity()
        {       
            DataGridFlights.Items.Clear();
            MyDataContext dboperator = new MyDataContext(connectionString);

            var flights = from fl in dboperator.flights
                          join airc in dboperator.airlines on fl.AirlineID equals airc.AirlineID
                          join air in dboperator.aircrafts on fl.AircraftID equals air.AircraftID
                          where fl.DepatureCity.Contains(SortBox.Text)
                          select new
                          {
                              fl.ArrivalCity,
                              fl.ArrivalTime,
                              fl.DepartureTime,
                              fl.DepatureCity,
                              fl.FlightNumber,
                              airc.AirlineName,
                              fl.FlightID,
                              air.AircraftModel,
                              air.SideNumber
                          };

            foreach (var item in flights)
            {
                DataGridFlights.Items.Add(item);
            }
        }

        void QueryArrivalCity()
        {
            DataGridFlights.Items.Clear();
            MyDataContext dboperator = new MyDataContext(connectionString);

            var flights = from fl in dboperator.flights
                          join airc in dboperator.airlines on fl.AirlineID equals airc.AirlineID
                          join air in dboperator.aircrafts on fl.AircraftID equals air.AircraftID
                          where fl.ArrivalCity.Contains(SortBox.Text)
                          select new
                          {
                              fl.ArrivalCity,
                              fl.ArrivalTime,
                              fl.DepartureTime,
                              fl.DepatureCity,
                              fl.FlightNumber,
                              airc.AirlineName,
                              fl.FlightID,
                              air.AircraftModel,
                              air.SideNumber
                          };

            foreach (var item in flights)
            {
                DataGridFlights.Items.Add(item);
            }
        }
       */
    }
}
