namespace OF.ConsentManagement.Model.CoreBank;

public class CbsPatchConsentResponse
{
    
    public string? OurReferenceNumber { get; set; }
    public string? ConsentId { get; set; }
    public string? PaymentId { get; set; }
    public Guid CorrelationId { get; set; }
    //CreditorAgent
    public decimal? Amount { get; set; }
    public string? ReturnCode { get; set; }
    public string? ReturnDescription { get; set; }

}