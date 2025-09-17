using Dapper;
using OF.ConsentManagement.CentralBankReceiverWorker.IServices;
using OF.ConsentManagement.Model.EFModel;

namespace OF.ConsentManagement.CentralBankReceiverWorker.Services;

public class PostConsentService : IPostConsentService
{

    private readonly PostConsentDbContext _context;
    private readonly IDbConnection _dbConnection;
    public PostConsentService(PostConsentDbContext context, IDbConnection dbConnection)
    {
        _context = context;
        _dbConnection = dbConnection;
    }

    public async Task SaveConsentRequestAsync(ConsentRequest consentRequest, Logger logger)
    {
        try
        {
            _context.ConsentRequest.Add(consentRequest);
            await _context.SaveChangesAsync();
            logger.Info($"SaveConsentRequestAsync is done. CorrelationId: {consentRequest.CorrelationId},  Id:{consentRequest.ConsentRequestId})");
        }
        catch (DbUpdateException dbEx)
        {
            logger.Error(dbEx, "Database update error occurred while saving SaveConsentRequestAsync.");
            throw; // Rethrow or return a special value depending on your error handling strategy
        }
        catch (Exception ex)
        {
            logger.Error(ex, $"CorrelationId: {consentRequest.CorrelationId} || An error occurred while saving SaveConsentRequestAsync.");
            throw;
        }
    }

    public async Task<long> GetConsentRequestIdAsync(Guid correlationId, Logger logger)
    {
        long result = 0; // Default return value

        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("correlationId", correlationId, DbType.Guid);

            var dbResult = await _dbConnection.ExecuteScalarAsync<long?>(
                "OF_GetLfiConsentRequestsId",
                parameters,
                commandTimeout: 1200,
                commandType: CommandType.StoredProcedure,
                transaction: null
            );
            result = dbResult ?? 0; // If null, set to 0 or any default you want
            logger.Info($"GetConsentRequestIdAsync is done. CorrelationId: {correlationId}, Result: {result}");
        }
        catch (Exception ex)
        {
            logger.Error(ex, $"CorrelationId: {correlationId} || An error occurred while executing GetConsentRequestIdAsync.");
            throw;
        }

        return result;
    }


    public async Task SaveConsentResponseAsync(long id, Guid correlationId, ConsentResponse consentResponse, Logger logger)
    {
        try
        {
            consentResponse.ConsentRequestId = id;

            _context.ConsentResponse.Add(consentResponse);
            await _context.SaveChangesAsync();

            logger.Info($"SaveConsentResponseAsync is done. ConsentRequestId: {id}, CorrelationId: {correlationId}, Id:{consentResponse.ConsentResponseId})");
        }
        catch (DbUpdateException dbEx)
        {
            logger.Error(dbEx, "Database update error occurred while saving SaveConsentResponseAsync.");
            throw;
        }
        catch (Exception ex)
        {
            logger.Error(ex, $"CorrelationId: {correlationId} || An error occurred while saving SaveConsentResponseAsync.");
            throw;
        }
    }

    public async Task<bool> UpdateConsentRequestStatusAsync(long id, string status, Guid correlationId, Logger logger)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@ConsentRequestId", id, DbType.Int64);
            parameters.Add("@Status", status, DbType.String);
            parameters.Add("@ModifiedBy", "", DbType.String);
            parameters.Add("@CurrentStatus", "Authorized", DbType.String);
            parameters.Add("@ReturnValue", dbType: DbType.Boolean, direction: ParameterDirection.Output);

            logger.Info($"Calling OF_UpdateConsentRequest with Transaction: Id={id}, Status={status}");

            await _dbConnection.ExecuteAsync(
                "OF_UpdateLfiConsentRequest",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 1200,
                transaction: null);

            logger.Info($"Consent request updated successfully with Transaction. CorrelationId: {correlationId}, Id={id}, Status={status}");
            return true;
        }
        catch (Exception ex)
        {
            logger.Error(ex, $"Error updating Consent request. CorrelationId: {correlationId}, Id={id}, Status={status}");
            throw;
        }
    }
}
