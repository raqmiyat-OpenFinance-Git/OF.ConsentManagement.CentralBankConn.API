namespace OF.ConsentManagement.Model.CentralBank.ResourceLog;

public class CbPatchFxQuoteLogRequestDto
{
    public Guid CorrelationId { get; set; }
    public string? LogId { get; set; }
    public CbPatchFxQuoteLogRequest? cbPatchFxQuoteLogRequest { get; set; }

}
