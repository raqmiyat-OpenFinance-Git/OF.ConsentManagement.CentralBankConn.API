using ConsentMangerModel.CoreBank;

namespace ConsentManagerService.Services
{
    public interface IRevokeConsentService
    {
        Task<IActionResult> RevokeConsentbyConsentGroupid(CbsConsentRevokedDto request, Logger logger);
        Task<IActionResult> RevokeConsentbyConsentId(CbsConsentRevokedDto request, Logger logger);
    }
}
