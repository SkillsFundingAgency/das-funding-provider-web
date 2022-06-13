using System.Diagnostics;
using System.Security.Claims;
using Newtonsoft.Json;
using SFA.DAS.Funding.Provider.Web.SystemAcceptanceTests.Services;
using WireMock.Logging;
using WireMock.Server;

namespace SFA.DAS.Funding.Provider.Web.MockServer.FundingProviderApi
{
    public class FundingProviderApiBuilder
    {
        private readonly WireMockServer _server;
        private readonly List<Claim> _claims;

        public static FundingProviderApiBuilder Create(int port)
        {
            return new FundingProviderApiBuilder(port);
        }

        private FundingProviderApiBuilder(int port)
        {
            _claims = new List<Claim>();
            _server = WireMockServer.StartWithAdminInterface(port);
        }
       
        public FundingProviderApi Build()
        {
            _server.LogEntriesChanged += _server_LogEntriesChanged;
            return new FundingProviderApi(_server, _claims);
        }

        private void AddClaim(string type, string value)
        {
            _claims.Add(new Claim(type, value));
        }

        private void AddOrReplaceClaim(string type, string value)
        {
            var existing = _claims.SingleOrDefault(c => c.Type == type);
            if (existing != null)
            {
                _claims.Remove(existing);
            }
            _claims.Add(new Claim(type, value));
        }

        private void _server_LogEntriesChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (LogEntry newItem in e.NewItems)
            {
                Debug.WriteLine("============================= TestFundingProviderApi MockServer called ================================");
                Debug.WriteLine(JsonConvert.SerializeObject(TestHelper.Map(newItem), Formatting.Indented));
                Debug.WriteLine("==========================================================================================================");
            }
        }
    }

#pragma warning disable S3881 // "IDisposable" should be implemented correctly
    public class FundingProviderApi : IDisposable
#pragma warning restore S3881 // "IDisposable" should be implemented correctly
    {
        private readonly WireMockServer _server;
        public List<Claim> Claims { get; }

        public FundingProviderApi(WireMockServer server, List<Claim> claims)
        {
            _server = server;
            Claims = claims;
        }

        public void Dispose()
        {
            if (_server.IsStarted)
            {
                _server.Stop();
            }
        }
    }
}
