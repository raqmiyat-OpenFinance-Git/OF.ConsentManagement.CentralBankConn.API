namespace OF.ConsentManagement.Model.EFModel;

[Table("ConsentAuditResponse")]
public class ConsentAuditResponse
{
    [Key]
    public long ConsentAuditResponseId { get; set; }
    public long ConsentAuditId { get; set; }   // FK to ConsentAudit
    public long? ConsentRequestId { get; set; }
    public string ConsentId { get; set; }
    public string ProviderId { get; set; }
    public string Operation { get; set; }
    public long? Timestamp { get; set; }
    public string FkMongoId { get; set; }
    public string FkId { get; set; }
    public string OzoneInteractionId { get; set; }
    public string CallerOrgId { get; set; }
    public string CallerClientId { get; set; }
    public string CallerSoftwareStatementId { get; set; }
    public string PatchFilter { get; set; }
    public string Patch { get; set; }
    public string Status { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }

    // 🔗 Navigation Property
    public ConsentAudit ConsentAudit { get; set; }
}