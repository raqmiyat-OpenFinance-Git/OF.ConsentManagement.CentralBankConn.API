using OF.ConsentManagement.Model.Common;
using OF.ConsentManagement.Model.EFModel;

namespace OF.ConsentManagement.CentralBankConn.API.Repositories;

public class MasterRepository : IMasterRepository
{
    private readonly IDistributedCache _cache;
    private readonly IOptions<RedisCacheSettings> _redisCacheSettings;
    private readonly IDbConnection _dbConnection;

    public MasterRepository(IDistributedCache distributedCache, IOptions<RedisCacheSettings> redisCacheSettings, IDbConnection dbConnection)
    {
        _cache = distributedCache;
        _redisCacheSettings = redisCacheSettings;
        _dbConnection = dbConnection;
    }

    public async Task<MasterTableList> GetCachedMasterAsync(Logger logger)
    {
        var masterTableList = new MasterTableList();
        try
        {
            if (!_redisCacheSettings.Value.EnableCache)
            {
                masterTableList = await GetMasterAsync(logger);
                return masterTableList;
            }

            string cacheKey = "MasterTableList";
            var cachedData = await _cache.GetAsync(cacheKey);

            if (cachedData != null)
            {
                var json = Encoding.UTF8.GetString(cachedData);
                masterTableList = System.Text.Json.JsonSerializer.Deserialize<MasterTableList>(json)!;
            }
            else
            {
                masterTableList = await GetMasterAsync(logger);

                var jsonData = System.Text.Json.JsonSerializer.Serialize(masterTableList);
                var dataToCache = Encoding.UTF8.GetBytes(jsonData);

                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_redisCacheSettings.Value.SetAbsoluteExpirationAddMinutes),
                    SlidingExpiration = TimeSpan.FromMinutes(_redisCacheSettings.Value.SetSlidingExpirationFromMinutes)
                };

                await _cache.SetAsync(cacheKey, dataToCache, options);
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Error occurred in GetCachedMasterAsync()");
        }

        return masterTableList;
    }

    private async Task<MasterTableList> GetMasterAsync(Logger logger)
    {
        var masterTableList = new MasterTableList();

        try
        {
            var parameters = new DynamicParameters();

            using var reader = await _dbConnection.QueryMultipleAsync(
                "OF_sp_get_CbsMappingCodes",
                parameters,
                commandType: CommandType.StoredProcedure);

            masterTableList.ofCbsMappingCode = reader.Read<OfCbsMappingCode>().ToList();
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Error occurred in GetMasterAsync()");
        }

        return masterTableList;
    }

    public async Task<bool> IsTransactionIdDuplicateAsync(string transactionId, string requestType, Logger logger)
    {
        try
        {
            return await CheckAndSetTransactionCacheAsync(transactionId, requestType, logger);
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Error occurred in IsTransactionIdDuplicateAsync()");
            return false;
        }
    }

    private async Task<bool> CheckAndSetTransactionCacheAsync(string refNbr, string requestType, Logger logger)
    {
        try
        {
            var cacheKey = $"{requestType}:{refNbr}";
            var existingValue = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(existingValue))
            {
                return true; // Duplicate found
            }

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_redisCacheSettings.Value.SetAbsoluteExpirationAddMinutes),
                SlidingExpiration = TimeSpan.FromMinutes(_redisCacheSettings.Value.SetSlidingExpirationFromMinutes)
            };

            await _cache.SetStringAsync(cacheKey, "exists", options);
            return false; // New entry
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Error occurred in CheckAndSetTransactionCacheAsync()");
            return false;
        }
    }

    Task<MasterTableList> IMasterRepository.GetCachedMasterAsync(Logger logger)
    {
        throw new NotImplementedException();
    }
}
