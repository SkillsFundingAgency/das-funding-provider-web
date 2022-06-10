﻿namespace SFA.DAS.Funding.Provider.Web.SystemAcceptanceTests.Bindings
{
    [Binding]
    public class TestCleanUp
    {
        private readonly TestContext _context;

        public TestCleanUp(TestContext context)
        {
            _context = context;
        }

        [AfterScenario]
        public void CleanUp()
        {
            Directory.Delete(_context.TestDirectory.FullName, true);
            _context.FundingProviderApi?.Dispose();
            _context.Website.Dispose();
        }
    }
}
