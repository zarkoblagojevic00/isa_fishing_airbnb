using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcceptanceTests.Parameters
{
    public class ReservationDTOParameter
    {
        public string UserMail { get; set; }
        public double Price { get; set; }
        public string AdditionalEquipment { get; set; }
        public int From { get; set; }
        public int To { get; set; }
    }
}
