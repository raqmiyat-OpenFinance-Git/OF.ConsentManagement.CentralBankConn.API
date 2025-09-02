using OF.ConsentManagement.Common.NLog;
using OF.ConsentManagement.Model.EFModel;

namespace OF.ConsentManagement.CentralBankReceiverWorker.Consumer;

[ExcludeFromConfigureEndpoints]
public class CbsEnquiryConsumer : IConsumer<CoreBankEnquiry>
{
    private readonly ConsentLogger _logger;
    private readonly CbsDbContext _cbsDbContext;
    public CbsEnquiryConsumer(ConsentLogger logger, CbsDbContext cbsDbContext)
    {
        _logger = logger;
        _cbsDbContext = cbsDbContext;
    }

    public async Task Consume(ConsumeContext<CoreBankEnquiry> context)
    {
        try
        {
            if (context?.Message == null)
            {
                _logger.Warn("CbsEnquiryConsumer: Received null message.");
                return;
            }

            _logger.Info($"CbsEnquiryConsumer: Received message - CorrelationId: {context.Message}");

           
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Unhandled exception in CbsEnquiryConsumer.Consume()");
            throw; // Rethrow to enable MassTransit retry/error policies
        }
    }
    public async Task<Guid> SaveCbsEnquiryAsync(CoreBankEnquiry enquiry, Logger logger)
    {
        try
        {
            if (!enquiry.CreatedDate.HasValue)
                enquiry.CreatedDate = DateTime.UtcNow;

            _cbsDbContext.coreBankEnquiries.Add(enquiry);
            await _cbsDbContext.SaveChangesAsync();


            return enquiry.EnquiryId; // Return the generated ID
        }
        catch (DbUpdateException dbEx)
        {
            logger.Error(dbEx, "Database update error occurred while saving CoreBankEnquiry.");
            throw; // Rethrow or return a special value depending on your error handling strategy
        }
        catch (Exception ex)
        {
            logger.Error(ex, $"OurReferenceNumber: {enquiry.OurReferenceNumber} || An error occurred while saving CoreBankEnquiry.");
            throw;
        }
    }



}
