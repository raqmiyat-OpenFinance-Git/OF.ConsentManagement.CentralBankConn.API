using OF.ConsentManagement.Model.CentralBank.Consent.PatchHeader;
using OF.ConsentManagement.Model.CentralBank.Consent.PatchRequest;

namespace OF.ConsentManagement.Model.CentralBank.Consent.PostRequestDto;

public class CbPatchConsentRequestDto
{
    public Guid CorrelationId { get; set; }
    public CbPatchConsentHeader? cbPatchConsentHeader { get; set; }
    public CbPatchConsentRequest? cbPatchConsentRequest { get; set; }
}
