using OF.ConsentManagement.Model.CentralBank.ResourceLog;

namespace OF.ConsentManagement.CentralBankConn.API.Services;

public interface IResourceLogService
{
    Task<bool> UpdateAccountOpeningLog(CbPatchAccountOpeningLogRequestDto cbPatchAccountOpeningLogRequestDto, Logger logger);
    Task<bool> UpdateFxQuoteLog(CbPatchFxQuoteLogRequestDto cbPatchFxQuoteLogRequestDto, Logger logger);
    Task<bool> UpdateInsuranceQuoteLog(CbPatchInsuranceQuoteLogRequestDto cbPatchInsuranceQuoteLogRequestDto, Logger logger);
}
