using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Funding.Provider.Web.Infrastructure.Authentication
{
    [ExcludeFromCodeCoverage]
    public class ProviderIdams
    {
        public string MetadataAddress { get; set; }

        public string Wtrealm { get; set; }
    }
}
