using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RBC_GAM.Data;
using System;

namespace RBC_GAM_Tests
{
    public class WebApplicationFactoryWithSqlLite : BaseWebApplicationFactory<TestStartup>
    {
        private readonly string _connectionString = $"Datasource={Guid.NewGuid()}.db";
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddEntityFrameworkSqlite()
                .AddDbContext<FinInstContext>(options =>
                {
                    options.UseSqlite(_connectionString);
                    options.UseInternalServiceProvider(services.BuildServiceProvider());
                });
            });
        }
    }
}
