    using ConsentMangerModel.Consent;

namespace ConsentManagerService.Services
{
    public interface IPaymentsService
    {
        Task<GetAuditConsentsByConsentIdResponse> GetAuditConsentResponse(string ConsentId, string AccountId, Logger logger);
        Task<string> PatchPaymentLogResponse(string Id, PatchPaymentRecordBody patchPaymentRecordBody, Logger logger);

    }
}
