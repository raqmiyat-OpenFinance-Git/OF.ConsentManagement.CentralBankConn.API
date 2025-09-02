using OF.ConsentManagement.Model.CentralBank.Consent.GetAuditResponse;

namespace OF.ConsentManagement.Model.CentralBank.Consent.GetResponseDto;

public class CbGetConsentAuditResponseDto
{
    public Guid CorrelationId { get; set; }
    public string Status { get; set; }
    public CbGetConsentAuditResponse? cbGetConsentAuditResponse { get; set; }
    public string ConsentId { get; set; }
}
