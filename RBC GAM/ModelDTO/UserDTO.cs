using RBC_GAM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBC_GAM.ModelDTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FinInstrumentId { get; set; }
        public List<ThresholdDTO> Thresholds { get; set; }
    }
}
