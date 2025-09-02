using OF.ConsentManagement.Model.CentralBank.Consent.GetQuery;

namespace OF.ConsentManagement.Model.CentralBank.Consent.GetRequestDto;

public class CbGetConsentRequestDto
{
    public Guid CorrelationId { get; set; }
    public CbGetConsentQueryParameters? cbGetConsentQueryParameters { get; set; }
    public string ConsentId { get; set; }
}
