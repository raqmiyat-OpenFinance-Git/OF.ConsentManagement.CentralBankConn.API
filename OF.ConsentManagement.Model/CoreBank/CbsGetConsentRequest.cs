namespace OF.ConsentManagement.Model.CoreBank;

public class CbsGetConsentRequest
{
    
    public string? OurReferenceNumber { get; set; }
    public string? ConsentId { get; set; }
    public string? PaymentId { get; set; }
    public Guid CorrelationId { get; set; }

}