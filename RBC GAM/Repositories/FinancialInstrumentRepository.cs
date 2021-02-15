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

namespace RBC_GAM.Repositories
{
    public class FinancialInstrumentRepository : IFinancialInstrumentRepository
    {
        private readonly FinInstContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<FinancialInstrumentRepository> _logger;

        public FinancialInstrumentRepository(FinInstContext dbContext,
                                             IMapper mapper,
                                             ILogger<FinancialInstrumentRepository> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<FinancialInstrument> GetFinancialInstrument(int id)
        {
            return await _dbContext.FinancialInstrument.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<FinancialInstrument>> GetFinancialInstruments()
        {
            return await _dbContext.FinancialInstrument.ToListAsync();
        }

        public Task<double> GetPrice(int id)
        {
            throw new NotImplementedException();
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

        private void DisplayNotification(UserNotification user)
        {
            _logger.LogInformation("--------------------------");
            _logger.LogInformation($"user: {user.UserId}");
            _logger.LogInformation($"fin Instrument: {user.FinInstrumentId}");
            _logger.LogInformation($"action: {user.Action}");
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
        private async Task<List<UserNotification>> UsersBuyAbove(double prevPrice, int finIstId)
        {
            var qq = from u in _dbContext.User
                     join fu in _dbContext.FinancialInstrumentUser on u.Id equals fu.UserId
                     join f in _dbContext.FinancialInstrument on fu.FinInstrumentId equals f.Id
                     join th in _dbContext.Threshold on u.Id equals th.UserId
                     join tr in _dbContext.Trigger on th.Id equals tr.ThresholdId
                     where f.Id == finIstId
                            && tr.Price > f.CurrentPrice
                            && tr.Fluctuation < Math.Abs(prevPrice - f.CurrentPrice)
                            && tr.Direction == Direction.Above
                            && tr.Action == Model.Action.Buy
                     select new UserNotification
                     {
                         UserId = u.Id,
                         Action = tr.Action,
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
        private async Task<List<UserNotification>> UsersBuyBelow(double prevPrice, int finIstId)
        {
            var qq = from u in _dbContext.User
                     join fu in _dbContext.FinancialInstrumentUser on u.Id equals fu.UserId
                     join f in _dbContext.FinancialInstrument on fu.FinInstrumentId equals f.Id
                     join th in _dbContext.Threshold on u.Id equals th.UserId
                     join tr in _dbContext.Trigger on th.Id equals tr.ThresholdId
                     where f.Id == finIstId
                            && tr.Price < f.CurrentPrice
                            && tr.Fluctuation < Math.Abs(f.CurrentPrice - prevPrice)
                            && tr.Direction == Direction.Below
                            && tr.Action == Model.Action.Buy
                     select new UserNotification
                     {
                         UserId = u.Id,
                         Name = u.Name,
                         Action = tr.Action,
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
        private async Task<List<UserNotification>> UsersSellAbove(double prevPrice, int finIstId)
        {
            var qq = from u in _dbContext.User
                     join fu in _dbContext.FinancialInstrumentUser on u.Id equals fu.UserId
                     join f in _dbContext.FinancialInstrument on fu.FinInstrumentId equals f.Id
                     join th in _dbContext.Threshold on u.Id equals th.UserId
                     join tr in _dbContext.Trigger on th.Id equals tr.ThresholdId
                     where f.Id == finIstId
                            && tr.Price > f.CurrentPrice
                            && tr.Fluctuation < Math.Abs(f.CurrentPrice - prevPrice)
                            && tr.Direction == Direction.Above
                            && tr.Action == Model.Action.Sell
                     select new UserNotification
                     {
                         UserId = u.Id,
                         Name = u.Name,
                         Action = tr.Action,
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
        private async Task<List<UserNotification>> UsersSellBelow(double prevPrice, int finIstId)
        {
            var qq = from u in _dbContext.User
                     join fu in _dbContext.FinancialInstrumentUser on u.Id equals fu.UserId
                     join f in _dbContext.FinancialInstrument on fu.FinInstrumentId equals f.Id
                     join th in _dbContext.Threshold on u.Id equals th.UserId
                     join tr in _dbContext.Trigger on th.Id equals tr.ThresholdId
                     where f.Id == finIstId
                            && tr.Price < f.CurrentPrice
                            && tr.Fluctuation < Math.Abs(f.CurrentPrice - prevPrice)
                            && tr.Direction == Direction.Below
                            && tr.Action == Model.Action.Sell
                     select new UserNotification
                     {
                         UserId = u.Id,
                         Name = u.Name,
                         Action = tr.Action,
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

        public async Task<bool> UpdatePrice(FinInstrumentDTO finInst)
        {
            var dbFinInst = await _dbContext.FinancialInstrument
                .SingleOrDefaultAsync(x => x.Id == finInst.Id);

            if (dbFinInst == null)
                throw new Exception("Financial Instrument Not Found");

            var prevPrice = dbFinInst.CurrentPrice;
            dbFinInst.CurrentPrice = finInst.Price;
            _dbContext.Update(dbFinInst);
            var result = await _dbContext.SaveChangesAsync();
            if (result > 0)
            {
                await NotifyUsers(prevPrice, dbFinInst.Id);
                return true;
            }

            return false;
        }

        public async Task<bool> BuyFinancialInstrument(UserDTO user)
        {
            var addUserToFinInst = await AddUserToFinancialInstrumentAsync(user.FinInstrumentId, user.Id);
            if (addUserToFinInst)
            {
                await CreateNewThresholds(user);
                return true;
            }

            return false;
        }

        public async Task<bool> SellFinancialInstrument(UserDTO user)
        {
            var finUser = await _dbContext.FinancialInstrumentUser
                        .SingleOrDefaultAsync(x => x.UserId == user.Id && x.FinInstrumentId == user.FinInstrumentId);

            if (finUser != null)
            {
                _dbContext.Remove(finUser);

                var dbthresholds = _dbContext.Threshold.Where(x => x.UserId == user.Id && x.FinInstrumentId == user.FinInstrumentId);
                _dbContext.RemoveRange(dbthresholds);

                var result = await _dbContext.SaveChangesAsync();
                return result > 0;
            }

            return false;
        }

        public async Task<bool> NewFinancialInstrument(FinInstrumentDTO finInst)
        {
            var dbFinInst = await _dbContext.FinancialInstrument
                                .SingleOrDefaultAsync(x => x.Id == finInst.Id);

            if (dbFinInst == null)
            {
                return await AddFinancialInstrument(finInst);
            }

            return false;
        }

        private async Task<bool> AddFinancialInstrument(FinInstrumentDTO price)
        {
            var dbFinInst = new FinancialInstrument
            {
                CurrentPrice = price.Price,
            };

            await _dbContext.AddAsync(dbFinInst);
            var result = await _dbContext.SaveChangesAsync();
            if (result > 0)
                return true;

            return false;
        }

        private async Task<bool> AddUserToFinancialInstrumentAsync(int finId, int userId)
        {
            var dbUser = await _dbContext.User.SingleOrDefaultAsync(x => x.Id == userId);
            var finInst = await _dbContext.FinancialInstrument.SingleOrDefaultAsync(x => x.Id == finId);

            if (dbUser == null || finInst == null)
                return false;

            var exists = await _dbContext.FinancialInstrumentUser
                .SingleOrDefaultAsync(x => x.UserId == dbUser.Id && x.FinInstrumentId == finInst.Id);
            if (exists != null)
                return false;

            var finUser = new FinancialInstrumentUser
            {
                FinInstrumentId = finId,
                UserId = userId
            };
            await _dbContext.AddAsync(finUser);
            var result = await _dbContext.SaveChangesAsync();

            return result > 0;
        }

        private async Task CreateNewThresholds(UserDTO userDTO)
        {
            var dbThresholds = new List<Threshold>();
            foreach (var thresholdDTO in userDTO.Thresholds)
            {
                var dbThreshold = _mapper.Map<Threshold>(thresholdDTO);
                dbThreshold.UserId = userDTO.Id;
                dbThresholds.Add(dbThreshold);        
            }

            await _dbContext.AddRangeAsync(dbThresholds);
            await _dbContext.SaveChangesAsync();
        }
    }
}
