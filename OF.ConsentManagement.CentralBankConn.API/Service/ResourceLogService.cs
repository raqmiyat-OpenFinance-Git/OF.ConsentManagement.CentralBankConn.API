using OF.ConsentManagement.Model.CentralBank.ResourceLog;
namespace OF.ConsentManagement.CentralBankConn.API.Services;
public class ResourceLogService : IResourceLogService
{
    private readonly IDbConnection _dbConnection;
    public ResourceLogService(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<bool> UpdateAccountOpeningLog(CbPatchAccountOpeningLogRequestDto cbPatchAccountOpeningLogRequestDto, Logger logger)
    {
        try
        {
            var parameters = new DynamicParameters();
            //parameters.Add("@Id", id, DbType.Int64);
            //parameters.Add("@ConsentStatus", consentStatus, DbType.String);

            //logger.Info($"Calling UpdateAccountOpeningLog with Id={id}, QuoteStatus={consentStatus}");

            await _dbConnection.ExecuteAsync(
                "OF_UpdateConsentRequestForPatch",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 1200
            );

            //logger.Info($"Consent request updated successfully. Id={id}, QuoteStatus={consentStatus}");
            return true;
        }
        catch (Exception ex)
        {
            //logger.Error(ex, $"Error in UpdateConsentRequestAsync(). Id={id}, QuoteStatus={consentStatus}");
            throw;
        }
    }

    public Task<bool> UpdateFxQuoteLog(CbPatchFxQuoteLogRequestDto cbPatchFxQuoteLogRequestDto, Logger logger)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateInsuranceQuoteLog(CbPatchInsuranceQuoteLogRequestDto cbPatchInsuranceQuoteLogRequestDto, Logger logger)
    {
        throw new NotImplementedException();
    }
}
