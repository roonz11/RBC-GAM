using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RBC_GAM.Data;
using RBC_GAM.Model;
using RBC_GAM.ModelDTO;
using RBC_GAM.Services;
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
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        
        public FinancialInstrumentRepository(FinInstContext dbContext,
                                             INotificationService notificationService,
                                             IMapper mapper)
        {
            _dbContext = dbContext;
            _notificationService = notificationService;
            _mapper = mapper;
        }

        public async Task<FinancialInstrument> GetFinancialInstrument(int id)
        {
            return await _dbContext.FinancialInstrument.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<FinancialInstrument>> GetFinancialInstruments()
        {
            return await _dbContext.FinancialInstrument.ToListAsync();
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
                await _notificationService.NotifyUsers(prevPrice, dbFinInst.Id);
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
