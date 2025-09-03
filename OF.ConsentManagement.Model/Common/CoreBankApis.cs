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
    public string? RevokeConsentByConsentGroupId { get; set; }
    public string? RevokeConsentById { get; set; }
    public string? GetPaymentLog { get; set; }
    public string? UpdatePaymentLogById { get; set; }
    public string? RevokeConsentbyConsentGroupId { get; set; }
    public string? RevokeConsentbyConsentId { get; set; }


}
