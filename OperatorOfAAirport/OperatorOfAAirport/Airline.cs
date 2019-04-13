using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace OperatorOfAAirport
{
    [Table]
    class Airline
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public short AirlineID { get; set; }
        [Column]
        public string AirlineName { get; set; }
        [Column]
        public string Country { get; set; }
    }
}
