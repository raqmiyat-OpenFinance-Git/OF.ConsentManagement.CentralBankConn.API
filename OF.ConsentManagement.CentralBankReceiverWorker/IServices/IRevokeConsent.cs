using ConsentMangerModel.CoreBank;

namespace ConsentManagerBackendReceiverWorker.IServices
{
    public interface IRevokeConsent
    {
        Task RevokeConsentAsync (CbsConsentRevokedDto request,Logger logger);
        Task RevokeConsentbyIdAsync(CbsConsentRevokedDto request,Logger logger);
    }
}
