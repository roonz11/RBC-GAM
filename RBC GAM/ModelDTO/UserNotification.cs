namespace RBC_GAM.ModelDTO
{
    public class UserNotification
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public int FinInstrumentId { get; set; }        
        public double PrevPrice { get; set; }
        public double CurrentPrice { get; set; }
        public TriggerDTO TriggerInfo { get; set; }
        
    }
}
