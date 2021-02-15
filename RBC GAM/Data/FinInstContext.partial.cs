using Microsoft.EntityFrameworkCore;
using RBC_GAM.Model;

namespace RBC_GAM.Data
{
    public partial class FinInstContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Threshold>()
                .HasOne(t => t.User)
                .WithMany(u => u.Thresholds)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Threshold>()
            //    .HasOne(t => t.FinancialInstrument)
            //    .WithMany(f => f.Thresholds)
            //    .HasForeignKey(t => t.FinancialInstrumentId)
            //    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Threshold>()
                .HasMany(th => th.Triggers)
                .WithOne(t => t.Threshold)
                .HasForeignKey(t => t.ThresholdId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FinancialInstrumentUser>()
                .HasKey(fu => new { fu.FinInstrumentId, fu.UserId });

            modelBuilder.Entity<FinancialInstrumentUser>()
                .HasOne(fu => fu.FinancialInstrument)
                .WithMany(f => f.FinancialInstrumentUsers)
                .HasForeignKey(fu => fu.FinInstrumentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FinancialInstrumentUser>()
                .HasOne(fu => fu.User)
                .WithMany(u => u.FinancialInstrumentUsers)
                .HasForeignKey(fu => fu.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
        }
    }
}
