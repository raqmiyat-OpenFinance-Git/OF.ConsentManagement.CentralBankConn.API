using ConsentMangerModel.Consent;
using OF.ConsentManagement.Model.CoreBank;

namespace ConsentManagerBackEndAPI.IServices
{
    public interface IPaymentsService
    {
        Task<ApiResult<GetAuditConsentsByConsentIdResponse>> GetPaymentLogDetails(CbGetAuditConsentByConsentIdRequestDto cbGetAuditConsentByConsentIdRequestDto);
        Task<ApiResult<string>> PatchPaymentLogDetails(CbPatchPaymentRecordRequestDto patchPaymentLogDetailsRequest);
    }
}
