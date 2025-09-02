namespace OF.ConsentManagement.Model.EFModel;


[Table("ConsentResponseHistory")]
public class ConsentResponseHistory
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ConsentResponseId { get; set; }

    [ForeignKey("ConsentStatusHistory")]
    public long? ConsentStatusHistoryId { get; set; }
    public virtual ConsentStatusHistory ConsentStatusHistory { get; set; }

    [ForeignKey("ConsentRequest")]
    public long ConsentRequestId { get; set; }
    public virtual ConsentRequest ConsentRequest { get; set; }

    [MaxLength(100)]
    public string ConsentId { get; set; }

    [MaxLength(100)]
    public string PsuUserId { get; set; }

    // Stored as JSON/string
    public string AccountIds { get; set; }

    // Stored as JSON/string
    public string InsurancePolicyIds { get; set; }

    // JSON data
    public string SupplementaryInformation { get; set; }

    // JSON data
    public string PaymentContext { get; set; }

    [MaxLength(200)]
    public string ConnectToken { get; set; }

    public DateTime? LastDataShared { get; set; }

    public DateTime? LastServiceInitiationAttempt { get; set; }

    [MaxLength(50)]
    public string AuthorizationChannel { get; set; }

    [MaxLength(50)]
    public string CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }
}