using System.Data.Linq;

namespace OperatorOfAAirport
{
    public class MyDataContext : DataContext
    {
        public Table<Operator> operators;
        public Table<Airline> airlines;
        public Table<Aircraft> aircrafts;
        public Table<Flight> flights;

        public MyDataContext(string connectionString): base(connectionString) { }
    }
}
