using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBC_GAM.Model
{
    public class FinancialInstrument
    {
        public int Id { get; set; }
        public double CurrentPrice { get; set; }
        //public IEnumerable<Threshold> Thresholds { get; set; }
        public IEnumerable<FinancialInstrumentUser> FinancialInstrumentUsers { get; set; }
    }
}
