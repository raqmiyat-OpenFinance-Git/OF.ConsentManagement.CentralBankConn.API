namespace OF.ConsentManagement.Model.EFModel;


[Table("ConsentRequest")]
public class ConsentRequest
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ConsentRequestId { get; set; }

    public Guid? CorrelationId { get; set; }

    public string Type { get; set; }
    public string ConsentId { get; set; }
    public string BaseConsentId { get; set; }

    public DateTime? ExpirationDateTime { get; set; }
    public DateTime? TransactionFromDateTime { get; set; }
    public DateTime? TransactionToDateTime { get; set; }

    public string AccountType { get; set; }
    public string AccountSubType { get; set; }

    public string OnBehalfOfTradingName { get; set; }
    public string OnBehalfOfLegalName { get; set; }
    public string OnBehalfOfIdentifierType { get; set; }
    public string OnBehalfOfIdentifier { get; set; }

    public string Permission { get; set; }

    public bool? BillingIsLargeCorporate { get; set; }
    public string BillingUserType { get; set; }
    public string BillingPurpose { get; set; }

    public string WebhookUrl { get; set; }
    public bool WebhookIsActive { get; set; }
    public string WebhookSecret { get; set; }

    public string Status { get; set; }

    public string CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }

    public string RequestPayload { get; set; }
    public string RequestUpdatePayload { get; set; }

    public string CurrentStatus { get; set; }
    public string Revokedby { get; set; }
    public string RevokedPsuUserId { get; set; }
}
