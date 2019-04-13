using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace OperatorOfAAirport
{
    [Table]
    class Flight
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public short FlightID { get; set; }
        [Column]
        public string FlightNumber { get; set; }
        [Column]
        public DateTime DepartureTime { get; set; }
        [Column]
        public DateTime ArrivalTime { get; set; }
        [Column]
        public string DepatureCity { get; set; }
        [Column]
        public string ArrivalCity { get; set; }
        [Column]
        public short AircaftID { get; set; }
        [Column]
        public short AirlineID { get; set; }
        [Column]
        public short OperatorID { get; set; }
    }
}
