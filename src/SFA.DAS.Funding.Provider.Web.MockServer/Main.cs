using SFA.DAS.Funding.Provider.Web.MockServer.EmployerIncentivesApi;

public static class MockServerProgram
{
    public static async Task Main()
    {
        var fundingProviderApi = FundingProviderApiBuilder
            .Create(8083)
            // .WithAccountWithNoLegalEntities() -- example
            .Build();

        Console.WriteLine("Press any key to stop the servers");
        Console.ReadKey();

        fundingProviderApi.Dispose();
    }
}
