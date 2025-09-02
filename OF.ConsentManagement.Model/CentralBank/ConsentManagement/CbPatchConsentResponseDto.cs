namespace OF.ConsentManagement.Model.CentralBank.Consent.PostRequestDto;

public class CbPatchConsentResponseDto
{
    public Guid CorrelationId { get; set; }
    public string Status { get; set; }
    public string ConsentId { get; set; }

}
