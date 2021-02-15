using System.Collections.Generic;

namespace RBC_GAM.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<FinancialInstrumentUser> FinancialInstrumentUsers { get; set; }
        public IEnumerable<Threshold> Thresholds { get; set; }
    }
}
