using System;
using System.Data.Linq.Mapping;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatorOfAAirport
{
    [Table]
    public class Operator
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public short OperatorID { get; set; }
        [Column]
        public string FirstName{ get; set; }
        [Column]
        public string SecondName{ get; set; }
        [Column(CanBeNull = true)]
        public string MiddleName{ get; set; }
        [Column(Name = "OperatorLogin")]
        public string Login{ get; set; }
        [Column(Name = "OperatorPassword")]
        public string Password{ get; set; }

    }
}
