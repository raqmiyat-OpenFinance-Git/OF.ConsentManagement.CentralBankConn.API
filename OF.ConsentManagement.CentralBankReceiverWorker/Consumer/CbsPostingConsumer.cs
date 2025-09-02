using OF.ConsentManagement.Common.NLog;
using OF.ConsentManagement.Model.EFModel;

namespace OF.ConsentManagement.CentralBankReceiverWorker.Consumer;

[ExcludeFromConfigureEndpoints]
public class CbsPostingConsumer : IConsumer<CoreBankPosting>
{
    private readonly ConsentLogger _logger;
    private readonly CbsDbContext _cbsDbContext;
    public CbsPostingConsumer(ConsentLogger logger, CbsDbContext cbsDbContext)
    {
        _logger = logger;
        _cbsDbContext = cbsDbContext;
    }

    public async Task Consume(ConsumeContext<CoreBankPosting> context)
    {
        try
        {
            if (context?.Message == null)
            {
                _logger.Warn("CbsPostingConsumer: Received null message.");
                return;
            }
            await SaveCbsPostingAsync(context?.Message!, _logger.Log);
            _logger.Info($"CbsPostingConsumer: Received message - CorrelationId: {context!.Message}");

           
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Unhandled exception in CbsPostingConsumer.Consume()");
            throw; // Rethrow to enable MassTransit retry/error policies
        }
    }
    public async Task<long> SaveCbsPostingAsync(CoreBankPosting posting, Logger logger)
    {
        try
        {
            if (!posting.CreatedOn.HasValue)
                posting.CreatedOn = DateTime.UtcNow;

            _cbsDbContext.coreBankPostings.Add(posting);
            await _cbsDbContext.SaveChangesAsync();


            return posting.Id; // Return the generated ID
        }
        catch (DbUpdateException dbEx)
        {
            logger.Error(dbEx, "Database update error occurred while saving CoreBankEnquiry.");
            throw; // Rethrow or return a special value depending on your error handling strategy
        }
        catch (Exception ex)
        {
            logger.Error(ex, $"OurReferenceNumber: {posting.OurReferenceNumber} || An error occurred while saving CoreBankEnquiry.");
            throw;
        }
    }



}
