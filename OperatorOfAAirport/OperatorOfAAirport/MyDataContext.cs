using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
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
