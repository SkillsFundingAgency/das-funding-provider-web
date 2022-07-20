using System.Diagnostics;
using Newtonsoft.Json;
using WireMock.Logging;
using WireMock.Server;

namespace SFA.DAS.Funding.Provider.Web.AcceptanceTests.Services.FundingProviderApi
{
    public class TestFundingProviderApi : IDisposable
    {
        private bool _isDisposed;

        public string BaseAddress { get; }

        public WireMockServer MockServer { get; }

        public TestFundingProviderApi()
        {
            MockServer = WireMockServer.Start();
            BaseAddress = MockServer.Urls[0];
            MockServer.LogEntriesChanged += MockServer_LogEntriesChanged!;
        }

        private void MockServer_LogEntriesChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (LogEntry newItem in e.NewItems!)
            {
                Debug.WriteLine("============================= TestFundingProviderApi MockServer called ================================");
                Debug.WriteLine(JsonConvert.SerializeObject(TestHelper.Map(newItem), Formatting.Indented));
                Debug.WriteLine("==========================================================================================================");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                if (MockServer.IsStarted)
                {
                    MockServer.Stop();
                }
                MockServer.Dispose();
            }

            _isDisposed = true;
        }
    }
}
