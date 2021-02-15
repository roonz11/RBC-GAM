using RBC_GAM.Data;
using RBC_GAM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBC_GAM_Tests
{
    //https://raaaimund.github.io/tech/2019/05/08/aspnet-core-integration-testing/
    public class TestDataSeeder
    {
        private readonly FinInstContext _dbContext;

        public TestDataSeeder(FinInstContext dbContext)
        {
            _dbContext = dbContext;
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
        }

        public void SeedData()
        {
            SeedFinancialInstrument();
            SeedUsers();
            SeedFinancialInstrumentUsers();
            SeedThresholds();
            SeedTriggers();            
        }

        private void SeedFinancialInstrument()
        {
            var dbFinInst = new FinancialInstrument
            {
                Id = 0,
                CurrentPrice = 14.25                
            };

            _dbContext.FinancialInstrument.Add(dbFinInst);
            _dbContext.SaveChanges();
        }

        private void SeedUsers()
        {
            var dbUser = new User
            {
                Id = 1,
                Name = "Aruna",
            };

            var dbUser2 = new User
            {
                Id = 2,
                Name = "Roonz",
            };

            _dbContext.User.Add(dbUser);
            _dbContext.User.Add(dbUser2);
            _dbContext.SaveChanges();
        }

        private void SeedFinancialInstrumentUsers()
        {
            var dbFinUser = new FinancialInstrumentUser
            {
                UserId = 1,
                FinInstrumentId = 1
            };

            _dbContext.FinancialInstrumentUser.Add(dbFinUser);
            _dbContext.SaveChanges();

        }

        private void SeedThresholds()
        {
            var threshold = new Threshold
            {
                Id = 0,
                FinInstrumentId = 1,
                UserId = 1,
            };

            var threshold2 = new Threshold
            {
                Id = 0,
                FinInstrumentId = 1,
                UserId = 1,
            };

            _dbContext.Threshold.Add(threshold);
            _dbContext.Threshold.Add(threshold2);
            _dbContext.SaveChanges();

        }

        private void SeedTriggers()
        {
            var trigger = new Trigger
            {
                Id = 0,
                Action = RBC_GAM.Model.Action.Buy,
                Price = 14.25,
                Direction = Direction.Above,
                Fluctuation = 0.02,
                ThresholdId = 1
            };
            var trigger2= new Trigger
            {
                Id = 0,
                Action = RBC_GAM.Model.Action.Sell,
                Price = 15.00,
                Direction = Direction.Below,
                Fluctuation = 0.02,
                ThresholdId = 1
            };

            _dbContext.Trigger.Add(trigger);
            _dbContext.Trigger.Add(trigger2);
            _dbContext.SaveChanges();
        }
    }
}
