using ConsentMangerModel.Consent;
using ConsentMangerModel.EFModel.Payments;

namespace ConsentManagerBackendReceiverWorker.IServices
{
    public interface IPaymentsService
    {
        Task SaveGetPaymentLogRequestAsync(Guid correlationId, GetPaymentLog getPaymentLog, Logger logger);
        Task UpdateGetPaymentLogRequestStatusAsync(Guid correlationId, string status, GetAuditConsentsByConsentIdResponse auditConsentsByConsentIdResponse, Logger logger);
        Task SavePatchPaymentLogRequestAsync(Guid correlationId, PatchPaymentLog patchPaymentLog, Logger logger);
        Task UpdatePatchPaymentLogRequestStatusAsync(Guid correlationId, string status, string response, Logger logger);
    }
}
