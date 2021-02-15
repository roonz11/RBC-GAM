using Xunit;

namespace RBC_GAM_Tests
{
    public class EndpointTestWithSqlLite : BaseEndpointTests, IClassFixture<WebApplicationFactoryWithSqlLite>
    {
        public EndpointTestWithSqlLite(WebApplicationFactoryWithSqlLite factory) : base(factory)
        {
        }
    }
}
