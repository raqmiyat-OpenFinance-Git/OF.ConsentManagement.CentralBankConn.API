namespace OF.ConsentManagement.Model.CentralBank.ResourceLog;

public class CbPatchInsuranceQuoteLogRequestDto
{
    public Guid CorrelationId { get; set; }
    public string? LogId { get; set; }
    public CbPatchInsuranceQuoteLogRequest? cbPatchInsuranceQuoteLogRequest { get; set; }

}
