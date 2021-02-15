using RBC_GAM.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RBC_GAM.Repositories
{
    public class TriggerRepository : ITriggerRepository
    {
        private readonly FinInstContext _dbContext;

        public TriggerRepository(FinInstContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> UpdateTriggersThatHaveBeenHit(int[] ids)
        {
            var dbTriggers = _dbContext.Trigger.Where(x => ids.Contains(x.Id)).Select(x => x).ToList();
            dbTriggers.ForEach(t => t.HasBeenHit = true);
            _dbContext.UpdateRange(dbTriggers);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
