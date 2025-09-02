namespace OF.ConsentManagement.Model.Common;

public class CoreBankApis
{
    public string? BaseUrl { get; set; }
    public string? TokenUrl { get; set; }
    public ConsentEventUrl? ConsentEventUrl { get; set; }
    public ConsentManagementUrl? ConsentManagementUrl { get; set; }
}
public class ConsentEventUrl
{
    public string? AugementConsentUrl { get; set; }
    public string? ValidateConsentUrl { get; set; }
    public string? ConsentOperationsUrl { get; set; }
    
}
public class ConsentManagementUrl
{
    public string? PostConsent { get; set; }
    public string? UpdateConsent { get; set; }
    public string? GetConsents { get; set; }
    public string? GetConsentById { get; set; }

}
