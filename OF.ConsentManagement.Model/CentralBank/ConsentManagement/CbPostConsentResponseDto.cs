using OF.ConsentManagement.Model.CentralBank.Consent.PostResponse;

namespace OF.ConsentManagement.Model.CentralBank.Consent.PostResponseDto;

public class CbPostConsentResponseDto
{
    public Guid CorrelationId { get; set; }
    public string status { get; set; }
    public CbPostConsentResponse? cbPostConsentResponse { get; set; }
}
