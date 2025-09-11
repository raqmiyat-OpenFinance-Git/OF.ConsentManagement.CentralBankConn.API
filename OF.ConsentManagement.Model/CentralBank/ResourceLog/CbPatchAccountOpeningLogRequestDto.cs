namespace OF.ConsentManagement.Model.CentralBank.ResourceLog;

public class CbPatchAccountOpeningLogRequestDto
{
    public Guid CorrelationId { get; set; }
    public string? LogId { get; set; }
    public CbPatchAccountOpeningLogRequest? cbPatchAccountOpeningLogRequest { get; set; }

}
