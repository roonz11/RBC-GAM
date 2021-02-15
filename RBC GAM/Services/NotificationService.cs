using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RBC_GAM.Data;
using RBC_GAM.Model;
using RBC_GAM.ModelDTO;
using RBC_GAM.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RBC_GAM.Services
{
    public class NotificationService : INotificationService
    {
        private readonly FinInstContext _dbContext;
        private readonly ITriggerRepository _triggerRepository;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(FinInstContext dbContext,
                                   ITriggerRepository triggerRepository,
                                   ILogger<NotificationService> logger)
        {
            _dbContext = dbContext;
            _triggerRepository = triggerRepository;
            _logger = logger;
        }
        public async Task NotifyUsers(double prevPrice, int finIstId)
        {
            var userBuyAbove = await UsersBuyAbove(prevPrice, finIstId);
            if(userBuyAbove.Count > 0)
            {
                DisplayNotification(userBuyAbove);
                await UpdateTriggers(userBuyAbove);
            }
            
            var userBuyBelow = await UsersBuyBelow(prevPrice, finIstId);
            if(userBuyBelow.Count > 0)
            {
                DisplayNotification(userBuyBelow);
                await UpdateTriggers(userBuyBelow);
            }

            var userSellAbove = await UsersSellAbove(prevPrice, finIstId);
            if(userSellAbove.Count > 0)
            {
                DisplayNotification(userSellAbove);
                await UpdateTriggers(userSellAbove);
            }

            var userSellBelow = await UsersSellBelow(prevPrice, finIstId);
            if(userSellBelow.Count > 0)
            {
                DisplayNotification(userSellBelow);
                await UpdateTriggers(userSellBelow);
            }
        }

        private void DisplayNotification(List<UserNotification> users)
        {
            foreach (var u in users)
                DisplayNotification(u);
        }

        public void DisplayNotification(UserNotification user)
        {
            _logger.LogInformation("--------------------------");
            _logger.LogInformation($"user: {user.UserId}");
            _logger.LogInformation($"fin Instrument: {user.FinInstrumentId}");            
            _logger.LogInformation($"prev Price: {user.PrevPrice}");
            _logger.LogInformation($"current Price: {user.CurrentPrice}");
            _logger.LogInformation("Trigger:");
            _logger.LogInformation($"        Id: {user.TriggerInfo.Id}");
            _logger.LogInformation($"        Action: {user.TriggerInfo.Action}");
            _logger.LogInformation($"        Price: {user.TriggerInfo.Price}");
            _logger.LogInformation($"        Direction: {user.TriggerInfo.Direction}");
            _logger.LogInformation($"        Fluctuation: {user.TriggerInfo.Fluctuation}");
            _logger.LogInformation($"        HasBeenHitAlready: {(user.TriggerInfo.HasBeenHit ? "Yes" : "No")}");
            _logger.LogInformation("--------------------------");
        }

        private async Task UpdateTriggers(List<UserNotification> users)
        {
            var triggersToUpdate = users.Where(x => !x.TriggerInfo.HasBeenHit).Select(x => x.TriggerInfo.Id).ToArray();
            await _triggerRepository.UpdateTriggersThatHaveBeenHit(triggersToUpdate);
        }

        /// <summary>
        /// Buy when price less than threshold and price going down
        /// </summary>
        /// <param name="prevPrice"></param>
        /// <param name="finIstId"></param>
        /// <returns></returns>
        public async Task<List<UserNotification>> UsersBuyAbove(double prevPrice, int finIstId)
        {
            var qq = from u in _dbContext.User
                     join fu in _dbContext.FinancialInstrumentUser on u.Id equals fu.UserId
                     join f in _dbContext.FinancialInstrument on fu.FinInstrumentId equals f.Id
                     join th in _dbContext.Threshold on u.Id equals th.UserId
                     join tr in _dbContext.Trigger on th.Id equals tr.ThresholdId
                     where f.Id == finIstId
                            && tr.Price >= f.CurrentPrice
                            && (!tr.HasBeenHit || tr.Fluctuation < Math.Abs(prevPrice - f.CurrentPrice))
                            && tr.Direction == Direction.Above
                            && tr.Action == Model.Action.Buy
                     select new UserNotification
                     {
                         UserId = u.Id,                         
                         FinInstrumentId = f.Id,
                         PrevPrice = prevPrice,
                         CurrentPrice = f.CurrentPrice,
                         TriggerInfo = new TriggerDTO
                         {
                             Id = tr.Id,
                             Action = tr.Action == Model.Action.Buy ? "Buy" : "Sell", //Enum.GetName(typeof(Model.Action), tr.Action),
                             Direction = tr.Direction == Direction.Above ? "Above" : "Below", //Enum.GetName(typeof(Direction), tr.Direction),
                             Fluctuation = tr.Fluctuation,
                             Price = tr.Price,
                             HasBeenHit = tr.HasBeenHit
                         }
                     };

            return await qq.ToListAsync();
        }

        /// <summary>
        /// Buy when price is higher than threshold and price going up
        /// </summary>
        /// <param name="prevPrice"></param>
        /// <param name="finIstId"></param>
        /// <returns></returns>
        public async Task<List<UserNotification>> UsersBuyBelow(double prevPrice, int finIstId)
        {
            var qq = from u in _dbContext.User
                     join fu in _dbContext.FinancialInstrumentUser on u.Id equals fu.UserId
                     join f in _dbContext.FinancialInstrument on fu.FinInstrumentId equals f.Id
                     join th in _dbContext.Threshold on u.Id equals th.UserId
                     join tr in _dbContext.Trigger on th.Id equals tr.ThresholdId
                     where f.Id == finIstId
                            && tr.Price <= f.CurrentPrice
                            && (!tr.HasBeenHit || tr.Fluctuation < Math.Abs(f.CurrentPrice - prevPrice))
                            && tr.Direction == Direction.Below
                            && tr.Action == Model.Action.Buy
                     select new UserNotification
                     {
                         UserId = u.Id,
                         Name = u.Name,                         
                         FinInstrumentId = f.Id,
                         PrevPrice = prevPrice,
                         CurrentPrice = f.CurrentPrice,
                         TriggerInfo = new TriggerDTO
                         {
                             Id = tr.Id,
                             Action = tr.Action == Model.Action.Buy ? "Buy" : "Sell",
                             Direction = tr.Direction == Direction.Above ? "Above" : "Below",
                             Fluctuation = tr.Fluctuation,
                             Price = tr.Price,
                             HasBeenHit = tr.HasBeenHit
                         }
                     };

            return await qq.ToListAsync();
        }

        /// <summary>
        /// Sell when lower than threshold and price going down
        /// </summary>
        /// <param name="prevPrice"></param>
        /// <param name="finIstId"></param>
        /// <returns></returns>
        public async Task<List<UserNotification>> UsersSellAbove(double prevPrice, int finIstId)
        {
            var qq = from u in _dbContext.User
                     join fu in _dbContext.FinancialInstrumentUser on u.Id equals fu.UserId
                     join f in _dbContext.FinancialInstrument on fu.FinInstrumentId equals f.Id
                     join th in _dbContext.Threshold on u.Id equals th.UserId
                     join tr in _dbContext.Trigger on th.Id equals tr.ThresholdId
                     where f.Id == finIstId
                            && tr.Price >= f.CurrentPrice
                            && (!tr.HasBeenHit || tr.Fluctuation < Math.Abs(f.CurrentPrice - prevPrice))
                            && tr.Direction == Direction.Above
                            && tr.Action == Model.Action.Sell
                     select new UserNotification
                     {
                         UserId = u.Id,
                         Name = u.Name,                         
                         FinInstrumentId = f.Id,
                         PrevPrice = prevPrice,
                         CurrentPrice = f.CurrentPrice,
                         TriggerInfo = new TriggerDTO
                         {
                             Id = tr.Id,
                             Action = tr.Action == Model.Action.Buy ? "Buy" : "Sell",
                             Direction = tr.Direction == Direction.Above ? "Above" : "Below",
                             Fluctuation = tr.Fluctuation,
                             Price = tr.Price,
                             HasBeenHit = tr.HasBeenHit
                         }
                     };

            return await qq.ToListAsync();
        }

        /// <summary>
        /// Sell when higher than threshold and price going up
        /// </summary>
        /// <param name="prevPrice"></param>
        /// <param name="finIstId"></param>
        /// <returns></returns>
        public async Task<List<UserNotification>> UsersSellBelow(double prevPrice, int finIstId)
        {
            var qq = from u in _dbContext.User
                     join fu in _dbContext.FinancialInstrumentUser on u.Id equals fu.UserId
                     join f in _dbContext.FinancialInstrument on fu.FinInstrumentId equals f.Id
                     join th in _dbContext.Threshold on u.Id equals th.UserId
                     join tr in _dbContext.Trigger on th.Id equals tr.ThresholdId
                     where f.Id == finIstId
                            && tr.Price <= f.CurrentPrice
                            && (!tr.HasBeenHit || tr.Fluctuation < Math.Abs(f.CurrentPrice - prevPrice))
                            && tr.Direction == Direction.Below
                            && tr.Action == Model.Action.Sell
                     select new UserNotification
                     {
                         UserId = u.Id,
                         Name = u.Name,                         
                         FinInstrumentId = f.Id,
                         PrevPrice = prevPrice,
                         CurrentPrice = f.CurrentPrice,
                         TriggerInfo = new TriggerDTO
                         {
                             Id = tr.Id,
                             Action = tr.Action == Model.Action.Buy ? "Buy" : "Sell",
                             Direction = tr.Direction == Direction.Above ? "Above" : "Below",
                             Fluctuation = tr.Fluctuation,
                             Price = tr.Price,
                             HasBeenHit = tr.HasBeenHit
                         }
                     };

            return await qq.ToListAsync();
        }
    }
}
