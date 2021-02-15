using Microsoft.EntityFrameworkCore;
using RBC_GAM.Model;

namespace RBC_GAM.Data
{
    public partial class FinInstContext : DbContext
    {
        public FinInstContext(DbContextOptions<FinInstContext> options) : base(options)
        {}
        public DbSet<FinancialInstrument> FinancialInstrument { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<FinancialInstrumentUser> FinancialInstrumentUser { get; set; }
        public DbSet<Trigger> Trigger {get; set;}
        public DbSet<Threshold> Threshold { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //   => options.UseSqlite("Data Source=finanicalInstrument.db");
    }
}
