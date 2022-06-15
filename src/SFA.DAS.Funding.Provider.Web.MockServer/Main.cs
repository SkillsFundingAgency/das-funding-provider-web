using SFA.DAS.Funding.Provider.Web.MockServer.CosmosDb;
using SFA.DAS.Funding.Provider.Web.MockServer.EmployerIncentivesApi;

public static class MockServerProgram
{
    public static async Task Main()
    {
        var fundingProviderApi = FundingProviderApiBuilder
            .Create(8083)
            // .WithAccountWithNoLegalEntities() -- example
            .Build();

        var readStore = await AccountsReadStoreBuilder.Create(8082);
        await readStore.WithAccountForAccountOwnerUserId(22222);
        readStore.Build();

        Console.WriteLine("Press any key to stop the servers");
        Console.ReadKey();

        fundingProviderApi.Dispose();
    }
}
