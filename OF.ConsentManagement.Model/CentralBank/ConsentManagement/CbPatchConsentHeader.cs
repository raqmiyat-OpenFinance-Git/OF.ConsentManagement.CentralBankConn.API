namespace OF.ConsentManagement.Model.CentralBank.Consent.PatchHeader;

public class CbPatchConsentHeader
{
    public Guid CorrelationId { get; set; }

    public string ConsentId { get; set; }

    public string? RequestJson { get; set; }
}
