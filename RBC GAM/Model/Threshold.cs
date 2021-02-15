using System.Collections.Generic;

namespace RBC_GAM.Model
{
    public class Threshold
    {
        public int Id { get; set; }
        public int FinInstrumentId { get; set; }
        public FinancialInstrument FinancialInstrument { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public IEnumerable<Trigger> Triggers { get; set; }                
    }

    public class Trigger
    {
        public int Id { get; set; }
        public Action Action { get; set; }
        public Threshold Threshold { get; set; }
        public int ThresholdId { get; set; }
        public double Price { get; set; }
        public Direction Direction { get; set; }
        public double Fluctuation { get; set; }
    }

    public enum Action
    {
        Buy = 0,
        Sell = 1
    }
    public enum Direction
    {
        Above = 0,
        Below = 1
    }
}
