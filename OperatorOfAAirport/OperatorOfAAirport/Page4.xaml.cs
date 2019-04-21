using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Linq;

namespace OperatorOfAAirport
{
    /// <summary>
    /// Логика взаимодействия для Page4.xaml
    /// </summary>
    public partial class Page4 : Page
    {
        string connectionString = "Data Source=DESKTOP-989RPMD;Initial Catalog=AirportDB;Integrated Security=True";

        public Page4()
        {
            InitializeComponent();

            MyDataContext dboperator = new MyDataContext(connectionString);

            var report = from fl in dboperator.flights
                         join airc in dboperator.airlines on fl.AirlineID equals airc.AirlineID
                         join aircrf in dboperator.aircrafts on fl.AircraftID equals aircrf.AircraftID
                         join op in dboperator.operators on fl.OperatorID equals op.OperatorID
                         select new
                         {
                             fl.ArrivalCity,
                             fl.ArrivalTime,
                             fl.DepartureTime,
                             fl.DepatureCity,
                             fl.FlightNumber,
                             airc.AirlineName,
                             airc.Country,
                             aircrf.AircraftModel,
                             aircrf.BusinessClass,
                             aircrf.EconomyClass,
                             aircrf.FirstClass,
                             aircrf.SideNumber,
                             aircrf.VIPClass,
                             op.FirstName,
                             op.SecondName
                         };

            foreach (var item in report)
            {
                DataGridAircraft.Items.Add(item);
            }
           
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
