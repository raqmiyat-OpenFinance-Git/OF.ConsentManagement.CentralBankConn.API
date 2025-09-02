using OF.ConsentManagement.Model.CoreBank;

namespace OF.ConsentManagement.CoreBankConn.API.IServices;
public interface IConsentManagementService
{
    Task<ApiResult<CbsPostConsentResponse>> SendConsentToCoreBankAsync(CbsPostConsentRequest cbsRequest, Logger logger);
    Task<ApiResult<CbsPatchConsentResponse>> SendConsentUpdateToCoreBankAsync(CbsPatchConsentRequest cbsRequest, Logger logger);
    Task<ApiResult<CbsGetConsentResponse>> GetConsentsFromCoreBankAsync(CbsGetConsentRequest cbsRequest, Logger logger);
    Task<ApiResult<CbsGetConsentResponse>> GetConsentByIdFromCoreBankAsync(CbsGetConsentRequest cbsRequest, Logger logger);
}
