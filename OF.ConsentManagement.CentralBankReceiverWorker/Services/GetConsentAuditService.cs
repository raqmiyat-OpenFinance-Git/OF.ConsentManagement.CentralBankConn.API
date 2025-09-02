using Dapper;
using OF.ConsentManagement.CentralBankReceiverWorker.IServices;
using OF.ConsentManagement.Model.EFModel;

namespace OF.ConsentManagement.CentralBankReceiverWorker.Services;

public class GetConsentAuditService : IGetConsentAuditService
{

    private readonly GetConsentAuditDbContext _context;
    private readonly IDbConnection _dbConnection;
    public GetConsentAuditService(GetConsentAuditDbContext context, IDbConnection dbConnection)
    {
        _context = context;
        _dbConnection = dbConnection;
    }

    public async Task<long> GetConsentAuditIdAsync(Guid correlationId, Logger logger)
    {
        long result = 0; // Default return value

        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("CorrelationId", correlationId, DbType.Guid);

            var dbResult = await _dbConnection.ExecuteScalarAsync<long?>(
                "OF_GetConsentAuditId",
                parameters,
                commandTimeout: 1200,
                commandType: CommandType.StoredProcedure,
                transaction: null
            );
            result = dbResult ?? 0; // If null, set to 0 or any default you want
            logger.Info($"GetConsentAuditIdAsync is done. CorrelationId: {correlationId}, Result: {result}");
        }
        catch (Exception ex)
        {
            logger.Error(ex, $"CorrelationId: {correlationId} || An error occurred while executing GetConsentAuditIdAsync.");
            throw;
        }

        return result;
    }

    public async Task SaveConsentAuditAsync(ConsentAudit consentAudit, Logger logger)
    {
        try
        {
            _context.ConsentAudits.Add(consentAudit);
            await _context.SaveChangesAsync();
            logger.Info($"SaveConsentAuditAsync is done. CorrelationId: {consentAudit.CorrelationId},  Id:{consentAudit.ConsentAuditId})");
        }
        catch (DbUpdateException dbEx)
        {
            logger.Error(dbEx, "Database update error occurred while saving SaveConsentAuditAsync.");
            throw; // Rethrow or return a special value depending on your error handling strategy
        }
        catch (Exception ex)
        {
            logger.Error(ex, $"CorrelationId: {consentAudit.CorrelationId} || An error occurred while saving SaveConsentAuditAsync.");
            throw;
        }
    }

    public async Task SaveConsentAuditResponseAsync(List<ConsentAuditResponse> consentAuditResponses, Guid correlationId, long consentAuditId, Logger logger)
    {
        try
        {
            _context.ConsentAuditResponses.AddRange(consentAuditResponses);
            await _context.SaveChangesAsync();
            logger.Info($"SaveConsentAuditResponseAsync is done. CorrelationId: {correlationId},  Id:{consentAuditId})");
        }
        catch (DbUpdateException dbEx)
        {
            logger.Error(dbEx, "Database update error occurred while saving SaveConsentAuditResponseAsync.");
            throw; // Rethrow or return a special value depending on your error handling strategy
        }
        catch (Exception ex)
        {
            logger.Error(ex, $"CorrelationId: {correlationId} || An error occurred while saving SaveConsentAuditResponseAsync.");
            throw;
        }
    }
    public async Task UpdateConsentAuditAsync(long id, string status, Guid correlationId, Logger logger)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@ConsentAuditId", id, DbType.Int64);
            parameters.Add("@Status", status, DbType.String);
            

            logger.Info($"Calling Usp_UpdateConsentAudit with Transaction: Id={id}, Status={status}");

            await _dbConnection.ExecuteAsync(
                "Usp_UpdateConsentAudit",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 1200,
            transaction: null);

            logger.Info($"Payment request updated successfully with Transaction. CorrelationId: {correlationId}, Id={id}, Status={status}");

            logger.Info($"UpdateConsentAuditAsync is done. ConsentStatusHistoryId: {id}, CorrelationId: {correlationId}, Id:{id})");
        }
        catch (DbUpdateException dbEx)
        {
            logger.Error(dbEx, "Database update error occurred while saving UpdateConsentAuditAsync.");
            throw;
        }
        catch (Exception ex)
        {
            logger.Error(ex, $"CorrelationId: {correlationId} || An error occurred while saving UpdateConsentAuditAsync.");
            throw;
        }
    }
}
