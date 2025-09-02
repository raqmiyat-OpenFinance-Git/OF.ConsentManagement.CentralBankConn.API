using Dapper;

namespace OF.ConsentManagement.CoreBankConn.API.Repositories;

public class CbsRepository : ICbsRepository
{
    private readonly CbsDbContext _context;
    private readonly IDbConnection _dbConnection;

    public CbsRepository(CbsDbContext context, IDbConnection dbConnection)
    {
        _context = context;
        _dbConnection = dbConnection;
    }
    public async Task<string> GetNextSeqAsync(string serviceName, string ourReferenceNumber, Logger logger)
    {
        try
        {
            long sequenceNo = 0;
            int retryCount = 2;

            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    sequenceNo = await GetNextSeqNoAsync(serviceName, ourReferenceNumber, logger);
                    logger.Trace($"OurReferenceNumber: {ourReferenceNumber} || Sequence number retrieved: {sequenceNo}");
                    if (sequenceNo > 0) break;
                }
                catch (SqlException ex)
                {
                    logger.Error(ex, $"OurReferenceNumber: {ourReferenceNumber} || Attempt {i + 1} failed.");
                    if (i == retryCount - 1) throw;
                }
            }

            string formattedSeqNo = sequenceNo.ToString().PadLeft(7, '0');
            string sequenceNoString = $"OF{DateTime.Now:yyMMdd}{formattedSeqNo}";

            logger.Trace($"OurReferenceNumber: {ourReferenceNumber} || Formatted sequence number: {sequenceNoString}");
            return sequenceNoString;
        }
        catch (Exception ex)
        {
            logger.Error(ex, $"OurReferenceNumber: {ourReferenceNumber} || Error in GetNextSequenceNoAsync");
            throw;
        }
    }
    private async Task<long> GetNextSeqNoAsync(string serviceName, string ourReferenceNumber, Logger logger)
    {
        logger.Trace($"OurReferenceNumber: {ourReferenceNumber} || GetNextSeqNo started for service: {serviceName}");
        long result = 0;

        try
        {

            var parameters = new DynamicParameters();
            parameters.Add("@InstrName", serviceName, DbType.String);

            var sequenceNumber = await _dbConnection.ExecuteScalarAsync<long?>(
                "OF_sp_get_next_ENQ_SEQ_No",
                parameters,
                commandType: CommandType.StoredProcedure
                );
            if (sequenceNumber.HasValue)
            {
                result = sequenceNumber.Value;
            }
            else
            {
                logger.Error($"OurReferenceNumber: {ourReferenceNumber} || Invalid or null sequence number returned.");
            }

            logger.Trace($"OurReferenceNumber: {ourReferenceNumber} || Sequence number result: {result}");
        }
        catch (Exception ex)
        {
            logger.Error(ex, $"OurReferenceNumber: {ourReferenceNumber} || Error in GetNextSeqNo");
        }

        return result;
    }

}
