namespace SFA.DAS.Funding.Provider.Web.Infrastructure.Configuration
{
    public class ExternalLinksConfiguration
    {
        public const string FundingProviderExternalLinksConfiguration = "ExternalLinks";

        public virtual string ManageApprenticeshipSiteUrl { get; set; }
        public virtual string CommitmentsSiteUrl { get; set; }
        public virtual string EmployerRecruitmentSiteUrl { get; set; }
    }
}
