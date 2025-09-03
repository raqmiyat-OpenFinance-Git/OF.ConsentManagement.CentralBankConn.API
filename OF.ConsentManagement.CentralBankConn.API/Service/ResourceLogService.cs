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
            parameters.Add("@Id", cbPatchAccountOpeningLogRequestDto.LogId, DbType.Int64);
            parameters.Add("@OpeningStatus", cbPatchAccountOpeningLogRequestDto.cbPatchAccountOpeningLogRequest!.OpeningStatus, DbType.String);

            logger.Info($"Calling UpdateAccountOpeningLog with Id={cbPatchAccountOpeningLogRequestDto.LogId}, QuoteStatus={cbPatchAccountOpeningLogRequestDto.cbPatchAccountOpeningLogRequest.OpeningStatus}");

            await _dbConnection.ExecuteAsync(
                "OF_UpdateResourceLogAccountOpening",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 1200
            );

            logger.Info($"AccountOpening request updated successfully. Id={cbPatchAccountOpeningLogRequestDto.LogId}, QuoteStatus={cbPatchAccountOpeningLogRequestDto.cbPatchAccountOpeningLogRequest.OpeningStatus}");
            return true;
        }
        catch (Exception ex)
        {
            logger.Error(ex, $"Error in UpdateAccountOpeningLog(). Id={cbPatchAccountOpeningLogRequestDto.LogId}, QuoteStatus={cbPatchAccountOpeningLogRequestDto.cbPatchAccountOpeningLogRequest!.OpeningStatus}");
            throw;
        }
    }

    public async Task<bool> UpdateFxQuoteLog(CbPatchFxQuoteLogRequestDto cbPatchFxQuoteLogRequestDto, Logger logger)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", cbPatchFxQuoteLogRequestDto.LogId, DbType.Int64);
            parameters.Add("@QuoteStatus", cbPatchFxQuoteLogRequestDto.cbPatchFxQuoteLogRequest!.QuoteStatus, DbType.String);

            logger.Info($"Calling UpdateFxQuoteLog with Id={cbPatchFxQuoteLogRequestDto.LogId}, QuoteStatus={cbPatchFxQuoteLogRequestDto.cbPatchFxQuoteLogRequest!.QuoteStatus}");

            await _dbConnection.ExecuteAsync(
                "OF_UpdateResourceLogFxQuote",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 1200
            );

            logger.Info($"FxQuote request updated successfully. Id={cbPatchFxQuoteLogRequestDto.LogId}, QuoteStatus={cbPatchFxQuoteLogRequestDto.cbPatchFxQuoteLogRequest!.QuoteStatus}");
            return true;
        }
        catch (Exception ex)
        {
            logger.Error(ex, $"Error in UpdateFxQuoteLog(). Id={cbPatchFxQuoteLogRequestDto.LogId}, QuoteStatus={cbPatchFxQuoteLogRequestDto.cbPatchFxQuoteLogRequest!.QuoteStatus}");
            throw;
        }

    }

    public async Task<bool> UpdateInsuranceQuoteLog(CbPatchInsuranceQuoteLogRequestDto cbPatchInsuranceQuoteLogRequestDto, Logger logger)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", cbPatchInsuranceQuoteLogRequestDto.LogId, DbType.Int64);
            parameters.Add("@QuoteStatus", cbPatchInsuranceQuoteLogRequestDto.cbPatchInsuranceQuoteLogRequest!.QuoteStatus, DbType.String);

            logger.Info($"Calling UpdateInsuranceQuoteLog with Id={cbPatchInsuranceQuoteLogRequestDto.LogId}, QuoteStatus={cbPatchInsuranceQuoteLogRequestDto.cbPatchInsuranceQuoteLogRequest.QuoteStatus}");

            await _dbConnection.ExecuteAsync(
                "OF_UpdateResourceLogInsuranceQuote",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 1200
            );

            logger.Info($"InsuranceQuote request updated successfully. Id={cbPatchInsuranceQuoteLogRequestDto.LogId}, QuoteStatus={cbPatchInsuranceQuoteLogRequestDto.cbPatchInsuranceQuoteLogRequest.QuoteStatus}");
            return true;
        }
        catch (Exception ex)
        {
            logger.Error(ex, $"Error in UpdateInsuranceQuoteLog(). Id={cbPatchInsuranceQuoteLogRequestDto.LogId}, QuoteStatus={cbPatchInsuranceQuoteLogRequestDto.cbPatchInsuranceQuoteLogRequest!.QuoteStatus}");
            throw;
        }
    }


}
