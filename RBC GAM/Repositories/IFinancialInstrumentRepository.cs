using RBC_GAM.Model;
using RBC_GAM.ModelDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RBC_GAM.Repositories
{
    public interface IFinancialInstrumentRepository
    {
        Task<int> NewFinancialInstrument(FinInstrumentDTO price);
        Task<FinInstrumentDTO> GetFinancialInstrument(int id);
        Task<List<FinInstrumentDTO>> GetFinancialInstruments();
        Task<bool> BuyFinancialInstrument(UserDTO user);
        Task<bool> SellFinancialInstrument(UserDTO user);
        Task<bool> UpdatePrice(FinInstrumentDTO finInst);                
    }
}
