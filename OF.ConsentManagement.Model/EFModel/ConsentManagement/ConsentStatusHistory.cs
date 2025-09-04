namespace OF.ConsentManagement.Model.EFModel;


[Table("LfiConsentStatusHistory")]
public class ConsentStatusHistory
{
    [Key]
    public long ConsentStatusHistoryId { get; set; }   // PK

    public Guid? CorrelationId { get; set; }

    public long? QueryParamUpdatedAt { get; set; }

    public string QueryParamConsentType { get; set; }

    public string QueryParamStatus { get; set; }

    public int QueryParamPage { get; set; }

    public int QueryParamPageSize { get; set; }

    public int? MetaPageNumber { get; set; }

    public int? MetaPageSize { get; set; }

    public int? MetaTotalPages { get; set; }

    public long? MetaTotalRecords { get; set; }

    public string StatusCode { get; set; }

    public string StatusReason { get; set; }

    public string StatusSource { get; set; }

    public DateTime ChangedOn { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string RequestPayload { get; set; }

    public string ResponsePayload { get; set; }

   

    // Navigation collections
    public ICollection<ConsentResponseHistory> ConsentResponseHistories { get; set; }
}