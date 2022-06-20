using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Funding.Provider.Web.Infrastructure.Authorisation
{
    [ExcludeFromCodeCoverage]
    public static class PolicyNames
    {
        public static string AllowAnonymous => nameof(AllowAnonymous);
        public static string HasProviderAccount => nameof(HasProviderAccount);
    }
}
