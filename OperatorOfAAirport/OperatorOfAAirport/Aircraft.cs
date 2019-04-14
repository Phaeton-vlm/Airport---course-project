using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace OperatorOfAAirport
{
    [Table]
    public class Aircraft
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public short AircraftID { get; set; }
        [Column]
        public string SideNumber { get; set; }
        [Column]
        public string AircraftModel { get; set; }
        [Column]
        public short EconomyClass { get; set; }
        [Column]
        public short BusinessClass { get; set; }
        [Column]
        public short FirstClass { get; set; }
        [Column]
        public short VIPClass { get; set; }
        [Column]
        public bool IsFree { get; set; }
    }
}
