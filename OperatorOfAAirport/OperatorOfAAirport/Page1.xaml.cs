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
                          { fl.ArrivalCity , fl.ArrivalTime, fl.DepartureTime, fl.DepatureCity, fl.FlightNumber, airc.AirlineName };

            foreach (var item in flights)
            {
                DataGridFlights.Items.Add(item);
            }

            _ComboBoxHours.ItemsSource = CurrentUser.LoadComboBoxHours();
            _ComboBoxMinutes.ItemsSource = CurrentUser.LoadComboBoxMinutes();
        }

       
    }
}
