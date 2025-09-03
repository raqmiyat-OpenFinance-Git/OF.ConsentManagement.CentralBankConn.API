using ConsentMangerModel.CoreBank;

namespace ConsentManagerBackEndAPI.IServices
{
    public interface IConsentRevoke
    {
        Task<IActionResult> PostConsentRevokebyConsentgrpId(CbsConsentRevokedDto request, Logger logger);
        Task<IActionResult> PostConsentRevokebyConsentId(CbsConsentRevokedDto request, Logger logger);
    }
}
