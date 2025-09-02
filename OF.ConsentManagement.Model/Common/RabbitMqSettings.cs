namespace OF.ConsentManagement.Model.Common;

public class RabbitMqSettings
{
    public string? Url { get; set; }
    public string? UserName { get; set; }
    public string? Rabitphrase { get; set; }
    public bool IsEncrypted { get; set; }
    public int IdleTimeoutMilliSeconds { get; set; }
    public string? AugmentConsentRequest { get; set; }
    public string? AugmentConsentResponse { get; set; }
    public string? ValidateConsentRequest { get; set; }
    public string? ValidateConsentResponse { get; set; }
    public string? ConsentOperationRequest { get; set; }
    public string? ConsentOperationResponse { get; set; }
    public string? AuditLog { get; set; }
    public string? CbsPostingRequest { get; set; }
    public string? CbsPostingResponse { get; set; }
    public string? CbsEnquiryRequest { get; set; }
    public string? CbsEnquiryResponse { get; set; }
    public string? PostConsentRequest { get; set; }
    public string? PostConsentResponse { get; set; }
    public string? GetConsentRequest { get; set; }
    public string? GetConsentResponse { get; set; }
    public string? PatchConsentRequest { get; set; }
    public string? PatchConsentResponse { get; set; }
    public string? GetConsentAuditRequest { get; set; }
    public string? GetConsentAuditResponse { get; set; }


    
    
    
}

