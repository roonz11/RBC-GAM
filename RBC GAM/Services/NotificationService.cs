using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RBC_GAM.Data;
using RBC_GAM.Model;
using RBC_GAM.ModelDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBC_GAM.Services
{
    public class NotificationService : INotificationService
    {
        private readonly FinInstContext _dbContext;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(FinInstContext dbContext,
                                   ILogger<NotificationService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task NotifyUsers(double prevPrice, int finIstId)
        {
            var userBuyAbove = await UsersBuyAbove(prevPrice, finIstId);
            foreach (var u in userBuyAbove)
                DisplayNotification(u);

            var userBuyBelow = await UsersBuyBelow(prevPrice, finIstId);
            foreach (var u in userBuyBelow)
                DisplayNotification(u);

            var userSellAbove = await UsersSellAbove(prevPrice, finIstId);
            foreach (var u in userSellAbove)
                DisplayNotification(u);

            var userSellBelow = await UsersSellBelow(prevPrice, finIstId);
            foreach (var u in userSellBelow)
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
            _logger.LogInformation($"        Action: {user.TriggerInfo.Action}");
            _logger.LogInformation($"        Price: {user.TriggerInfo.Price}");
            _logger.LogInformation($"        Direction: {user.TriggerInfo.Direction}");
            _logger.LogInformation($"        Fluctuation: {user.TriggerInfo.Fluctuation}");
            _logger.LogInformation("--------------------------");
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
                            && tr.Fluctuation < Math.Abs(prevPrice - f.CurrentPrice)
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
                             Action = tr.Action == Model.Action.Buy ? "Buy" : "Sell", //Enum.GetName(typeof(Model.Action), tr.Action),
                             Direction = tr.Direction == Direction.Above ? "Above" : "Below", //Enum.GetName(typeof(Direction), tr.Direction),
                             Fluctuation = tr.Fluctuation,
                             Price = tr.Price
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
                            && tr.Fluctuation < Math.Abs(f.CurrentPrice - prevPrice)
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
                             Action = tr.Action == Model.Action.Buy ? "Buy" : "Sell",
                             Direction = tr.Direction == Direction.Above ? "Above" : "Below",
                             Fluctuation = tr.Fluctuation,
                             Price = tr.Price
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
                            && tr.Fluctuation < Math.Abs(f.CurrentPrice - prevPrice)
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
                             Action = tr.Action == Model.Action.Buy ? "Buy" : "Sell",
                             Direction = tr.Direction == Direction.Above ? "Above" : "Below",
                             Fluctuation = tr.Fluctuation,
                             Price = tr.Price
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
                            && tr.Fluctuation < Math.Abs(f.CurrentPrice - prevPrice)
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
                             Action = tr.Action == Model.Action.Buy ? "Buy" : "Sell",
                             Direction = tr.Direction == Direction.Above ? "Above" : "Below",
                             Fluctuation = tr.Fluctuation,
                             Price = tr.Price
                         }
                     };

            return await qq.ToListAsync();
        }
    }
}
