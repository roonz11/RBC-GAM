using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBC_GAM.Model
{
    public class FinancialInstrumentUser
    {
        public FinancialInstrument FinancialInstrument { get; set; }
        public int FinInstrumentId { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }
    }
}
