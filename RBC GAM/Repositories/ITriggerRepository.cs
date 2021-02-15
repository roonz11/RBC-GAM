using System.Threading.Tasks;

namespace RBC_GAM.Repositories
{
    public interface ITriggerRepository
    {
        Task<int> UpdateTriggersThatHaveBeenHit(int[] ids);
    }
}