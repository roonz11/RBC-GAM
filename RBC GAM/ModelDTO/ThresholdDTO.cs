using System.Collections.Generic;

namespace RBC_GAM.ModelDTO
{
    public class ThresholdDTO
    {
        public int Id { get; set; }
        public int FinInstrumentId { get; set; }        
        public int UserId { get; set; }        
        public List<TriggerDTO> Triggers { get; set; }
    }

    public class TriggerDTO
    {
        public int Id { get; set; }
        public string Action { get; set; }        
        public int ThresholdId { get; set; }
        public double Price { get; set; }
        public string Direction { get; set; }
        public double Fluctuation { get; set; }
        public bool HasBeenHit { get; set; }
    }
}
