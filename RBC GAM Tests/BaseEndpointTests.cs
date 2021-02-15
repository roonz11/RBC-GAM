using Newtonsoft.Json;
using RBC_GAM.ModelDTO;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RBC_GAM_Tests
{
    public abstract class BaseEndpointTests
    {
        protected BaseWebApplicationFactory<TestStartup> Factory { get; }

        protected BaseEndpointTests(BaseWebApplicationFactory<TestStartup> factory) =>
            Factory = factory;

        public static readonly IEnumerable<object[]> Endpoints = new List<object[]>()
    {
        //new object[] {"/FinancialInstrument"},
        new object[] {"api/financialInstrument/NewInstrument"},
        
    };

        [Theory]
        [MemberData(nameof(Endpoints))]
        public async Task GetEndpointsReturnSuccessAndCorrectContentType(string url)
        {
            const string expectedContentType = "text/html; charset=utf-8";
            var client = Factory.CreateClient();



            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            Assert.Equal(expectedContentType,
                response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [MemberData(nameof(Endpoints))]
        public async Task Create_New_FinancialInstrument_Should_Return_Ok(string uri)
        {
            //Arrange
            var _httpClient = Factory.CreateClient();

            var finInst = new FinInstrumentDTO
            {
                Id = 0,
                Price = 14.25,
                UserId = 0
            };
            var json = JsonConvert.SerializeObject(finInst);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            uri = new StringBuilder(_httpClient.BaseAddress.ToString())
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
