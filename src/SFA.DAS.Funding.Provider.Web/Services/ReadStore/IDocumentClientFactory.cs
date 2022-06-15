using Microsoft.Azure.Documents;

namespace SFA.DAS.Funding.Provider.Web.Services.ReadStore
{
    public interface IDocumentClientFactory
    {
        IDocumentClient CreateDocumentClient();
    }
}
