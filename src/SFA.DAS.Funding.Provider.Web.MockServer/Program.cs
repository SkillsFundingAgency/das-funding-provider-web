//using SFA.DAS.Funding.Provider.Web.MockServer;
//using SFA.DAS.Funding.Provider.Web.MockServer.CosmosDb;
//using SFA.DAS.Funding.Provider.Web.MockServer.FundingProviderApi;
//using SFA.DAS.Funding.Provider.Web.SystemAcceptanceTests;

//var fundingProviderApi = FundingProviderApiBuilder
//    .Create(8083)
//    // .WithAccountWithNoLegalEntities() -- example
//    .Build();

////var readStore = await AccountsReadStoreBuilder.Create(8082);
////await readStore.WithAccountForAccountOwnerUserId(22222);
////readStore.Build();

//var webSite = new LocalWebSite(fundingProviderApi.Claims)
//    .Build()
//    .Run();

//Console.WriteLine("Press any key to stop the servers");
//Console.ReadKey();

//fundingProviderApi.Dispose();
//webSite.Dispose();