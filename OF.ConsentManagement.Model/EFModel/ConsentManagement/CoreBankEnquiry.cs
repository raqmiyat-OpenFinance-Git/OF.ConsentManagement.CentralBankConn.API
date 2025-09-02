namespace OF.ConsentManagement.Model.EFModel;

[Table("CoreBankEnquiry")]
public class CoreBankEnquiry
{
    [Key]
    public Guid EnquiryId { get; set; }
    public string? EnquiryType { get; set; }

    public Guid? CorrelationId { get; set; }
    public string? OurReferenceNumber { get; set; }
    public string? OfReferenceId { get; set; }
    public string? CoreBankReferenceId { get; set; }

    public decimal? Amount { get; set; }
    public string? Currency { get; set; }
    public string? Status { get; set; }

    public DateTime? TransactionDate { get; set; }
    public DateTime? ValueDate { get; set; }

    public string? PayerAccountNumber { get; set; }
    public string? PayerName { get; set; }
    public string? PayeeAccountNumber { get; set; }
    public string? PayeeName { get; set; }

    public string? BankResponseCode { get; set; }
    public string? BankResponseMessage { get; set; }

    public DateTime? RequestTimestamp { get; set; }
    public DateTime? ResponseTimestamp { get; set; }

    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }

    public string? Module { get; set; }
    public int RetryCount { get; set; }
    public DateTime? RetryOn { get; set; }

    public string? RequestPayload { get; set; }
    public string? ResponsePayload { get; set; }
    public string? Comments { get; set; }

    public string? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public DateTime? LastUpdatedOn { get; set; }

}
