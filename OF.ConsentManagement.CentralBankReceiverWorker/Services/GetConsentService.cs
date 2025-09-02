using Dapper;
using OF.ConsentManagement.CentralBankReceiverWorker.IServices;
using OF.ConsentManagement.Model.EFModel;

namespace OF.ConsentManagement.CentralBankReceiverWorker.Services;

public class GetConsentService : IGetConsentService
{

    private readonly GetConsentDbContext _context;
    private readonly IDbConnection _dbConnection;
    public GetConsentService(GetConsentDbContext context, IDbConnection dbConnection)
    {
        _context = context;
        _dbConnection = dbConnection;
    }

    public async Task<long> GetConsentStatusHistoryIdAsync(Guid correlationId, Logger logger)
    {
        long result = 0; // Default return value

        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("CorrelationId", correlationId, DbType.Guid);

            var dbResult = await _dbConnection.ExecuteScalarAsync<long?>(
                "OF_GetConsentStatusHistoryId",
                parameters,
                commandTimeout: 1200,
                commandType: CommandType.StoredProcedure,
                transaction: null
            );
            result = dbResult ?? 0; // If null, set to 0 or any default you want
            logger.Info($"GetConsentStatusHistoryIdAsync is done. CorrelationId: {correlationId}, Result: {result}");
        }
        catch (Exception ex)
        {
            logger.Error(ex, $"CorrelationId: {correlationId} || An error occurred while executing GetConsentStatusHistoryIdAsync.");
            throw;
        }

        return result;
    }

    public async Task SaveConsentStatusHistoryAsync(ConsentStatusHistory consentStatusHistory, Logger logger)
    {
        try
        {
            _context.ConsentStatusHistory.Add(consentStatusHistory);
            await _context.SaveChangesAsync();
            logger.Info($"SaveConsentStatusHistoryAsync is done. CorrelationId: {consentStatusHistory.CorrelationId},  Id:{consentStatusHistory.ConsentStatusHistoryId})");
        }
        catch (DbUpdateException dbEx)
        {
            logger.Error(dbEx, "Database update error occurred while saving SaveConsentStatusHistoryAsync.");
            throw; // Rethrow or return a special value depending on your error handling strategy
        }
        catch (Exception ex)
        {
            logger.Error(ex, $"CorrelationId: {consentStatusHistory.CorrelationId} || An error occurred while saving SaveConsentStatusHistoryAsync.");
            throw;
        }
    }

    public async Task<IEnumerable<ConsentIdentifier>> GetConsentRequestIdsAsync(Guid correlationId,
    IEnumerable<string> consentIds, Logger logger)
    {
        List<ConsentIdentifier> consentIdentifiers = new List<ConsentIdentifier>(); // Default return value
        try
        {
            // Build DataTable for TVP
            var tvp = new DataTable();
            tvp.Columns.Add("ConsentId", typeof(string));
            foreach (var id in consentIds)
            {
                tvp.Rows.Add(id);
            }

            var parameters = new DynamicParameters();
            parameters.Add("@ConsentIds", tvp.AsTableValuedParameter("dbo.ConsentIdList"));

            var result = await _dbConnection.QueryAsync<ConsentIdentifier>(
                "Usp_GetConsentRequestIds",
                parameters,
                commandType: CommandType.StoredProcedure);

            logger.Info($"GetConsentRequestIdsAsync is done. CorrelationId: {correlationId}, Result: {result}");
        }
        catch (Exception ex)
        {
            logger.Error(ex, $"CorrelationId: {correlationId} || An error occurred while executing GetConsentRequestIdsAsync.");
            throw;
        }

        return consentIdentifiers;


    }

    public async Task SaveConsentResponseHistoryAsync(Guid correlationId, List<ConsentResponseHistory> consentResponseHistories, Logger logger)
    {
        try
        {
            _context.ConsentResponseHistory.AddRange(consentResponseHistories);
            await _context.SaveChangesAsync();

            foreach (var consentStatusHistory in consentResponseHistories)
            {
                logger.Info($"SaveConsentResponseHistoryAsync is done. " +
                            $"CorrelationId: {correlationId},  " +
                            $"Id:{consentStatusHistory.ConsentStatusHistoryId})");
            }
        }
        catch (DbUpdateException dbEx)
        {
            logger.Error(dbEx, "Database update error occurred while saving SaveConsentResponseHistoryAsync.");
            throw; // Rethrow or return a special value depending on your error handling strategy
        }
        catch (Exception ex)
        {
            logger.Error(ex, $"CorrelationId: {correlationId} || An error occurred while saving SaveConsentResponseHistoryAsync.");
            throw;
        }
    }

    public async Task UpdateConsentStatusHistoryAsync(long id, Guid correlationId, ConsentStatusHistory consentStatusHistory, Logger logger)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@ConsentStatusHistoryId", consentStatusHistory.ConsentStatusHistoryId, DbType.Int64);
            parameters.Add("@MetaPageNumber", consentStatusHistory.MetaPageNumber, DbType.Int32);
            parameters.Add("@MetaPageSize", consentStatusHistory.MetaPageSize, DbType.Int32);
            parameters.Add("@MetaTotalPages", consentStatusHistory.MetaTotalPages, DbType.Int32);
            parameters.Add("@MetaTotalRecords", consentStatusHistory.MetaTotalRecords, DbType.Int32);
            parameters.Add("@StatusCode", consentStatusHistory.StatusCode, DbType.String);
            parameters.Add("@StatusReason", consentStatusHistory.StatusReason, DbType.String);

            logger.Info($"Calling OF_UpdatePaymentRequests with Transaction: Id={id}, Status={consentStatusHistory.StatusCode}");

            await _dbConnection.ExecuteAsync(
                "Usp_UpdateConsentStatusHistory",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 1200,
            transaction: null);

            logger.Info($"Payment request updated successfully with Transaction. CorrelationId: {correlationId}, Id={id}, Status={consentStatusHistory.StatusCode}");

            logger.Info($"UpdateConsentStatusHistoryAsync is done. ConsentStatusHistoryId: {id}, CorrelationId: {correlationId}, Id:{consentStatusHistory.ConsentStatusHistoryId})");
        }
        catch (DbUpdateException dbEx)
        {
            logger.Error(dbEx, "Database update error occurred while saving UpdateConsentStatusHistoryAsync.");
            throw;
        }
        catch (Exception ex)
        {
            logger.Error(ex, $"CorrelationId: {correlationId} || An error occurred while saving UpdateConsentStatusHistoryAsync.");
            throw;
        }
    }

}
