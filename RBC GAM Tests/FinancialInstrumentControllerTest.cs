using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using RBC_GAM.ModelDTO;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RBC_GAM_Tests
{
    public class FinancialInstrumentControllerTest : TestWithSqlLite, IClassFixture<WebApplicationFactory<RBC_GAM.Startup>>
    {
        private readonly HttpClient _httpClient;
        public FinancialInstrumentControllerTest(WebApplicationFactory<RBC_GAM.Startup> appFactory)
        {
            _httpClient = appFactory.CreateClient();
        }

        [Fact]
        public async Task Database_Is_Available_And_Can_Be_Connected_To()
        {
            Assert.True(await _dbContext.Database.CanConnectAsync());
        }

        [Fact]
        public async Task Create_New_FinancialInstrument_Should_Return_Ok()
        {
            //Arrange
            var finInst = new FinInstrumentDTO
            {
                Id = 0,
                Price = 14.25,
                UserId = 0
            };
            var json = JsonConvert.SerializeObject(finInst);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var uri = new StringBuilder(_httpClient.BaseAddress.ToString())
                .Append("api/")
                .Append("financialInstrument/")
                .Append("NewInstrument")
                .ToString();

            //Action
            var response = await _httpClient.PostAsync(uri, data);
            var result = JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());

            Assert.True(result > 0);
        }
    }
}
