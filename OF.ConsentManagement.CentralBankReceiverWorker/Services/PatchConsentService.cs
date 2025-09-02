using Dapper;
using OF.ConsentManagement.CentralBankReceiverWorker.IServices;
using OF.ConsentManagement.Model.EFModel;
using Logger = NLog.Logger;

namespace OF.ConsentManagement.CentralBankReceiverWorker.Services;

public class PatchConsentService : IPatchConsentService
{
    private readonly IDbConnection _dbConnection;

    public PatchConsentService(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<long> GetConsentRequestIdAsync(string consentId, Logger logger)
    {
        long result = 0;
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@ConsentId", consentId, DbType.String);

            var dbResult = await _dbConnection.ExecuteScalarAsync<long?>(
                "OF_GetConsentRequestsIdByConsentId",
                parameters,
                commandTimeout: 1200,
                commandType: CommandType.StoredProcedure
            );

            result = dbResult ?? 0;
            logger.Info($"GetConsentRequestIdAsync done. ConsentId={consentId}, Result={result}");
        }
        catch (Exception ex)
        {
            logger.Error(ex, $"ConsentId={consentId} || Error in GetConsentRequestIdAsync.");
            throw;
        }
        return result;
    }

    public async Task<bool> UpdateConsentRequestAsync(long id, string consentStatus, Logger logger)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Int64);
            parameters.Add("@ConsentStatus", consentStatus, DbType.String);

            logger.Info($"Calling OF_UpdateConsentRequestForPatch with Id={id}, QuoteStatus={consentStatus}");

            await _dbConnection.ExecuteAsync(
                "OF_UpdateConsentRequestForPatch", 
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 1200
            );

            logger.Info($"Consent request updated successfully. Id={id}, QuoteStatus={consentStatus}");
            return true;
        }
        catch (Exception ex)
        {
            logger.Error(ex, $"Error in UpdateConsentRequestAsync(). Id={id}, QuoteStatus={consentStatus}");
            throw;
        }
    }

    public async Task<bool> UpdateConsentResponseAsync(long id, ConsentResponse consentResponse, Logger logger)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Int64);
            parameters.Add("@PsuUserId", consentResponse.PsuUserId, DbType.String);
            parameters.Add("@AccountIds", consentResponse.AccountIds, DbType.String);
            parameters.Add("@InsurancePolicyIds", consentResponse.InsurancePolicyIds, DbType.String);
            parameters.Add("@SupplementaryInformation", consentResponse.SupplementaryInformation, DbType.String);
            parameters.Add("@PaymentContext", consentResponse.PaymentContext, DbType.String);
            parameters.Add("@ConnectToken", consentResponse.ConnectToken, DbType.String);
            parameters.Add("@LastDataShared", consentResponse.LastDataShared, DbType.DateTime2);
            parameters.Add("@LastServiceInitiationAttempt", consentResponse.LastServiceInitiationAttempt, DbType.DateTime2);
            parameters.Add("@AuthorizationChannel", consentResponse.AuthorizationChannel, DbType.String);
            parameters.Add("@UnitCurrency", consentResponse.UnitCurrency, DbType.String);
            parameters.Add("@ExchangeRate", consentResponse.ExchangeRate, DbType.Decimal);
            parameters.Add("@RateType", consentResponse.RateType, DbType.String);
            parameters.Add("@ContractIdentification", consentResponse.ContractIdentification, DbType.String);
            parameters.Add("@ExpirationDateTime", consentResponse.ExpirationDateTime, DbType.DateTime2);
            parameters.Add("@ChargeBearer", consentResponse.ChargeBearer, DbType.String);
            parameters.Add("@ChargeBearerType", consentResponse.ChargeBearerType, DbType.String);
            parameters.Add("@ChargeBearerAmount", consentResponse.ChargeBearerAmount, DbType.Decimal);
            parameters.Add("@ChargeBearerCurrency", consentResponse.ChargeBearerCurrency, DbType.String);
            parameters.Add("@ResponseUpdatePayload", consentResponse.ResponseUpdatePayload, DbType.String);

            logger.Info($"Calling OF_UpdateConsentResponsesForPatch with Id={id}");

            await _dbConnection.ExecuteAsync(
                "OF_UpdateConsentResponsesForPatch",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 1200
            );

            logger.Info($"Consent response updated successfully. Id={id}");
            return true;
        }
        catch (Exception ex)
        {
            logger.Error(ex, $"Error in UpdateConsentResponseAsync(). Id={id}");
            throw;
        }
    }
}