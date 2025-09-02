namespace OF.ConsentManagement.Model.EFModel
{
    [Table("AuditLog")]
    public class AuditLog
    {
        public long Id { get; set; }
        public Guid CorrelationId { get; set; }
        public string? SourceSystem { get; set; }
        public string? TargetSystem { get; set; }
        public string? Endpoint { get; set; }
        public string? RequestPayload { get; set; }
        public string? ResponsePayload { get; set; }
        public string? StatusCode { get; set; }
        public string? RequestType { get; set; }
        public int? ExecutionTimeMs { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
