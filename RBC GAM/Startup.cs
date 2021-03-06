using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RBC_GAM.Data;
using RBC_GAM.Repositories;
using RBC_GAM.Services;

namespace RBC_GAM
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureDatabaseServices(services);
            services.AddScoped<IFinancialInstrumentRepository, FinancialInstrumentRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITriggerRepository, TriggerRepository>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddControllers();
            services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapping>(), typeof(Startup));
        }

        protected virtual void ConfigureDatabaseServices(IServiceCollection services)
        {
            services.AddDbContext<FinInstContext>(options => options.UseSqlite("Data Source=finanicalInstrument.db"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
