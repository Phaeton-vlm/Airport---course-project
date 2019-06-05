using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OperatorOfAAirport
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {

        public Page1()
        {
            InitializeComponent();

            MyDataContext dboperator = new MyDataContext(CurrentUser.connectionString);
            

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

        //Добавить рейс
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
            _ComboBoxHours.SelectedIndex = -1;
            _ComboBoxMinutes.SelectedIndex = -1;
            _ComboBoxAirlineName.SelectedIndex = -1;
            _ComboBoxSideNumber.SelectedIndex = -1;
            DatePiker.Text = "";
            TimePiker.Text="";
        }

        private void ButtonClick_OK(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_TextBoxArrivalCity.Text.Length > 0 && _TextBoxDepatureCity.Text.Length > 0 && _TextBoxFlightNumber.Text.Length > 0)
                {
                    MyDataContext dbFlight = new MyDataContext(CurrentUser.connectionString);

                    DateTime date1;
                    date1 = Convert.ToDateTime(DatePiker.Text);
                    date1 = date1.AddHours(TimePiker.SelectedTime.Value.Hour);
                    date1 = date1.AddMinutes(TimePiker.SelectedTime.Value.Minute);

                    if(date1 < DateTime.Now)
                    {
                        TextBlockMessgeAddFlight.Visibility = Visibility.Visible;
                        TextBlockMessgeAddFlight.Text = "Неверная дата";
                        return;
                    }

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
                    ClearValue();
                }
                else
                {
                    TextBlockMessgeAddFlight.Visibility = Visibility.Visible;
                    TextBlockMessgeAddFlight.Text = "Заполнены не все поля";
                }

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
        
        //Удалить рейс
        private void ButtonClick_DelFlight(object sender, RoutedEventArgs e)
        {
            try
            {
                MyDataContext dboperator = new MyDataContext(CurrentUser.connectionString);
                
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
            catch 
            {
                ErrorTextBlock.Text = "Выделите строку для удаления";
                ErrorTextBlock.Visibility = Visibility.Visible;
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
            MyDataContext dboperator = new MyDataContext(CurrentUser.connectionString);

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

        private void DataGridFlights_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ErrorTextBlock.Visibility == Visibility.Visible)
            {
                ErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void _TextBoxFlightNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (TextBlockMessgeAddFlight.Visibility == Visibility.Visible)
            {
                TextBlockMessgeAddFlight.Visibility = Visibility.Hidden;
            }
        }

        private void DatePiker_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (TextBlockMessgeAddFlight.Visibility == Visibility.Visible)
            {
                TextBlockMessgeAddFlight.Visibility = Visibility.Hidden;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            QueryFlightNumber();
        }
    }
}
