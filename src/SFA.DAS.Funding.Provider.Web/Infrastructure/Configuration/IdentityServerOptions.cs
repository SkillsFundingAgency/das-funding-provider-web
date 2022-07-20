namespace SFA.DAS.Funding.Provider.Web.Infrastructure.Configuration
{
    public class IdentityServerOptions
    {
        public const string IdentityServerConfiguration = "Identity";
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
        public string? BaseAddress { get; set; }
        public string? Scopes { get; set; }
        public bool UsePkce { get; set; }
    }
}
