using OF.ConsentManagement.Model.CentralBank.Consent.PostRequest;

namespace OF.ConsentManagement.Model.CentralBank.Consent.PostRequestDto;

public class CbPostConsentRequestDto
{
    public Guid CorrelationId { get; set; }
    public CbPostConsentRequest? cbPostConsentRequest { get; set; }
}
