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
        public string? ChangeEmailUrl { get; set; }
        public string ChangeEmailLinkFormatted()
        {
            return BaseAddress?.Replace("/identity", "") + string.Format(ChangeEmailUrl, ClientId);
        }
        public string? ChangePasswordUrl { get; set; }
        public string ChangePasswordLinkFormatted()
        {
            return BaseAddress?.Replace("/identity", "") + string.Format(ChangePasswordUrl, ClientId);
        }
    }
}
