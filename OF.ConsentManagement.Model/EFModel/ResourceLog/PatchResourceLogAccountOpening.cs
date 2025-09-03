namespace OF.ConsentManagement.Model.EFModel;

[Table("PatchResourceLogAccountOpening")]
public class PatchResourceLogAccountOpening
{
    [Key]
    public long Id { get; set; }  //bigint identity
    public Guid CorrelationId { get; set; } //uniqueidentifier
    public long RequestId { get; set; }
    public string? Status { get; set; }
    public DateTime? CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public string? ModifiedBy { get; set; }
    public string? RequestPayload { get; set; }
    public string? ResponsePayload { get; set; }

}
