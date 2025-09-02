using OF.ConsentManagement.Model.CentralBank.Consent.GetResponse;

namespace OF.ConsentManagement.Model.CentralBank.Consent.GetResponseDto;

public class CbGetConsentResponseDto
{
    public Guid CorrelationId { get; set; }
    public string Status { get; set; }
    public CbGetConsentResponse? cbGetConsentResponse { get; set; }
    public string ConsentId { get; set; }
}
