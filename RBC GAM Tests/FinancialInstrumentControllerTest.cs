using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using RBC_GAM.ModelDTO;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RBC_GAM_Tests
{
    public class FinancialInstrumentControllerTest : TestWithSqlLite, IClassFixture<WebApplicationFactory<RBC_GAM.Startup>>
    {
        //protected BaseWebApplicationFactory<TestStartup> Factory { get; }

        //protected FinancialInstrumentControllerTest(BaseWebApplicationFactory<TestStartup> factory) =>
        //Factory = factory;

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
                Price = 14.30,
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

            //Assert
            Assert.True(result > 0);
        }

        [Fact]
        public async Task Get_FinancialInstrument_Should_Return_FinInstrument()
        {
            //Arrange
            var finInstDTO = new FinInstrumentDTO
            {
                Id = 0,
                Price = 14.25,
                UserId = 0
            };
            var json = JsonConvert.SerializeObject(finInstDTO);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var uri = new StringBuilder(_httpClient.BaseAddress.ToString())
                .Append("api/")
                .Append("financialInstrument/")
                .Append("NewInstrument")
                .ToString();

            var getUri = new StringBuilder(_httpClient.BaseAddress.ToString())
                .Append("api/")
                .Append("financialInstrument/")
                .Append("1")
                .ToString();

            //Action
            var response = await _httpClient.PostAsync(uri, data);
            var result = JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());

            var getResponse = await _httpClient.GetAsync(getUri);
            var getResult = JsonConvert.DeserializeObject<FinInstrumentDTO>(await getResponse.Content.ReadAsStringAsync());

            //Assert
            Assert.True(result > 0);
            Assert.Equal(1, getResult.Id);
            Assert.Equal(finInstDTO.Price, getResult.Price);
        }

        [Fact]
        public async Task EndToEnd_Test()
        {
            var finInstId = await NewInstrument();
            var userId = await NewUser();
            var buyInstResult = await BuyInstrument(finInstId, userId);

            var prices = new double[]
            {
                14.30,
                14.27,
                14.26,
                14.25,
                14.26,
                14.25,
                14.26,
                14.24,
                14.25,
                14.35,
                14.40
            };

            foreach (var p in prices)
                await SetPrice(p);
        }

        private async Task<int> NewInstrument()
        {
            var finInstDTO = new FinInstrumentDTO
            {
                Id = 0,
                Price = 14.25,
                UserId = 0
            };
            var json = JsonConvert.SerializeObject(finInstDTO);
            var finInstJS = new StringContent(json, Encoding.UTF8, "application/json");

            var addFinInstUri = new StringBuilder(_httpClient.BaseAddress.ToString())
                .Append("api/")
                .Append("financialInstrument/")
                .Append("NewInstrument")
                .ToString();

            //Action
            var response = await _httpClient.PostAsync(addFinInstUri, finInstJS);
            var result = JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());

            return result;
        }

        private async Task<int> NewUser()
        {
            var userDTO = new UserDTO
            {
                Id = 0,
                Name = "aruna"
            };
            var json = JsonConvert.SerializeObject(userDTO);
            var userJS = new StringContent(json, Encoding.UTF8, "application/json");

            var addUserUri = new StringBuilder(_httpClient.BaseAddress.ToString())
                .Append("api/")
                .Append("user")
                .ToString();

            //Action

            var createUserResponse = await _httpClient.PostAsync(addUserUri, userJS);
            var createUserResult = JsonConvert.DeserializeObject<int>(await createUserResponse.Content.ReadAsStringAsync());

            return createUserResult;
        }

        
        private async Task<bool> BuyInstrument(int finInstId, int userId)
        {
            var userDTO = new UserDTO
            {
                Id = userId,
                FinInstrumentId = finInstId,
                Thresholds = new List<ThresholdDTO> ()
                {
                    new ThresholdDTO
                    {
                        Id = 0,
                        FinInstrumentId = finInstId,
                        UserId = userId,
                        Triggers = new List<TriggerDTO>()
                        {
                            new TriggerDTO
                            {
                                Id = 0,
                                Action = "Buy",
                                ThresholdId = 0,
                                Price = 14.25,
                                Direction = "Above",
                                Fluctuation = 0.02,
                                HasBeenHit = false
                            },
                            new TriggerDTO
                            {
                                Id = 0,
                                Action = "Buy",
                                ThresholdId = 0,
                                Price = 14.00,
                                Direction = "Below",
                                Fluctuation = 0.02,
                                HasBeenHit = false
                            },
                            new TriggerDTO
                            {
                                Id = 0,
                                Action = "Sell",
                                ThresholdId = 0,
                                Price = 14.30,
                                Direction = "Below",
                                Fluctuation = 0.02,
                                HasBeenHit = false
                            },
                            new TriggerDTO
                            {
                                Id = 0,
                                Action = "Sell",
                                ThresholdId = 0,
                                Price = 15.30,
                                Direction = "Above",
                                Fluctuation = 0.02,
                                HasBeenHit = false
                            }
                        }
                    }
                }

            };
            var json = JsonConvert.SerializeObject(userDTO);
            var userJS = new StringContent(json, Encoding.UTF8, "application/json");

            var buyFinInstUri = new StringBuilder(_httpClient.BaseAddress.ToString())
                .Append("api/")
                .Append("financialInstrument/")
                .Append("buyinstrument")
                .ToString();

            var response = await _httpClient.PostAsync(buyFinInstUri, userJS);
            var result = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
            return result;
        }

        private async Task SetPrice(double price)
        {
            var finInstDTO = new FinInstrumentDTO
            {
                Id = 1,
                Price = price
            };

            var json = JsonConvert.SerializeObject(finInstDTO);
            var finInstJS = new StringContent(json, Encoding.UTF8, "application/json");

            var setPriceUri = new StringBuilder(_httpClient.BaseAddress.ToString())
                .Append("api/")
                .Append("financialInstrument/")
                .Append("setprice")
                .ToString();

            var response = await _httpClient.PutAsync(setPriceUri, finInstJS);
            //var result = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());            
        }
    }
}
