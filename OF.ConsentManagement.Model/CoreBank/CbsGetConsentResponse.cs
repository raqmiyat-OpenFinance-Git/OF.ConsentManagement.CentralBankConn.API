namespace OF.ConsentManagement.Model.CoreBank;

public class CbsGetConsentResponse
{
    public Guid CorrelationId { get; set; }
    public string? ConsentId { get; set; }
    public string? PaymentId { get; set; }

    public string? OurReferenceNumber { get; set; }
    public string? CoreBankReferenceId { get; set; }

    public decimal? Amount { get; set; }
    public string? Currency { get; set; }
    public string? PaymentStatus { get; set; }

    public DateTime? TransactionDate { get; set; }
    public DateTime? ValueDate { get; set; }

    public string? PayerAccountNumber { get; set; }
    public string? PayerName { get; set; }
    public string? PayeeAccountNumber { get; set; }
    public string? PayeeName { get; set; }
    public string? BankResponseCode { get; set; }
    public string? BankResponseMessage { get; set; }

}