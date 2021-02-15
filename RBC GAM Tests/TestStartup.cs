using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RBC_GAM;

namespace RBC_GAM_Tests
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        { }

        protected override void ConfigureDatabaseServices(IServiceCollection services)
        {
            services.AddTransient<TestDataSeeder>();            
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            base.Configure(app, env);
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var seeder = serviceScope.ServiceProvider.GetService<TestDataSeeder>();
            seeder.SeedData();
        }

    }
}
