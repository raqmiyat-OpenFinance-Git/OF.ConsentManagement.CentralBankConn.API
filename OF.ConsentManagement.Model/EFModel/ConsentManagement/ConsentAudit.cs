namespace OF.ConsentManagement.Model.EFModel;

[Table("LfiConsentAudit")]
public class ConsentAudit
{
    [Key]
    public long ConsentAuditId { get; set; }
    public Guid? CorrelationId { get; set; }
    public string Status { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public string RequestPayload { get; set; }
    public string RequestUpdatePayload { get; set; }
    public string CurrentStatus { get; set; }

    // 🔗 Navigation Property
    public ICollection<ConsentAuditResponse> ConsentAuditResponses { get; set; }
}