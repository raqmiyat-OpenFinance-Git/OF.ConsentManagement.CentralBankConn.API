using ConsentManagerBackendReceiverWorker.EntityFramework;
using ConsentManagerBackendReceiverWorker.IServices;
using ConsentMangerModel.Consent;
using ConsentMangerModel.EFModel.Payments;

namespace ConsentManagerBackendReceiverWorker.Services
{
    public class PaymentsService : IPaymentsService
    {

        private readonly PaymentLogDbContext _context;
        private readonly IDbConnection _dbConnection;
        public PaymentsService(PaymentLogDbContext context, IDbConnection dbConnection)
        {
            _context = context;
            _dbConnection = dbConnection;
        }

        public async Task SaveGetPaymentLogRequestAsync(Guid correlationId,GetPaymentLog getPaymentLog, Logger logger)
        {
            logger.Info($"SaveBulkPaymentRequestAsync is started.");
            try
            {
                _context.GetPaymentLog.Add(getPaymentLog);
                await _context.SaveChangesAsync();
                logger.Info($"SaveGetPaymentLogRequestAsync is done. CorrelationId: {correlationId}");
            }
            catch (DbUpdateException dbEx)
            {
                logger.Error(dbEx, "Database update error occurred while saving SaveGetPaymentLogRequestAsync.");
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"CorrelationId: {correlationId} || An error occurred while saving SaveGetPaymentLogRequestAsync.");
            }
            logger.Info($"SaveGetPaymentLogRequestAsync is completed.");
        }

        public async Task UpdateGetPaymentLogRequestStatusAsync(Guid correlationId, string status, GetAuditConsentsByConsentIdResponse auditConsentsByConsentIdResponse, Logger logger)
        {
            logger.Info($"UpdateGetPaymentLogRequestStatusAsync is started.");
            try
            {
                var GetPaymentLogConsent = await _context.GetPaymentLog.Where(x => x.CorrelationId == correlationId).FirstOrDefaultAsync();
                if (GetPaymentLogConsent == null)
                {
                    logger.Warn($"GetPaymentLogConsent not found. CorrelationId: {correlationId}");
                }

                GetPaymentLogConsent!.Status = status;
                GetPaymentLogConsent.ModifiedBy = "System";
                GetPaymentLogConsent.ModifiedOn = DateTime.UtcNow;
                GetPaymentLogConsent.ResponsePayload = JsonConvert.SerializeObject(auditConsentsByConsentIdResponse);
                await _context.SaveChangesAsync();

                logger.Info($"GetPaymentLogConsent request updated successfully with Transaction. CorrelationId: {correlationId}, Status={status}");
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error updating GetPaymentLogConsent request. CorrelationId: {correlationId}, Status={status}");
                throw;
            }
            logger.Info($"UpdateGetPaymentLogRequestStatusAsync is completed.");
        }

        public async Task SavePatchPaymentLogRequestAsync(Guid correlationId, PatchPaymentLog patchPaymentLog, Logger logger)
        {
            logger.Info($"SavePatchPaymentLogRequestAsync is started.");
            try
            {
                _context.PatchPaymentLog.Add(patchPaymentLog);
                await _context.SaveChangesAsync();
                logger.Info($"SavePatchPaymentLogRequestAsync is done. CorrelationId: {correlationId}");
            }
            catch (DbUpdateException dbEx)
            {
                logger.Error(dbEx, "Database update error occurred while saving SavePatchPaymentLogRequestAsync.");
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"CorrelationId: {correlationId} || An error occurred while saving SavePatchPaymentLogRequestAsync.");
            }
            logger.Info($"SavePatchPaymentLogRequestAsync is completed.");
        }

        public async Task UpdatePatchPaymentLogRequestStatusAsync(Guid correlationId, string status, string response, Logger logger)
        {
            logger.Info($"UpdatePatchPaymentLogRequestStatusAsync is started.");
            try
            {
                var GetPaymentLogConsent = await _context.GetPaymentLog.Where(x => x.CorrelationId == correlationId).FirstOrDefaultAsync();
                if (GetPaymentLogConsent == null)
                {
                    logger.Warn($"PatchPaymentLogConsent not found. CorrelationId: {correlationId}");
                }

                GetPaymentLogConsent!.Status = status;
                GetPaymentLogConsent.ModifiedBy = "System";
                GetPaymentLogConsent.ModifiedOn = DateTime.UtcNow;
                GetPaymentLogConsent.ResponsePayload = response;
                await _context.SaveChangesAsync();

                logger.Info($"PatchPaymentLogConsent request updated successfully with Transaction. CorrelationId: {correlationId}, Status={status}");
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error updating PatchPaymentLogConsent request. CorrelationId: {correlationId}, Status={status}");
                throw;
            }
            logger.Info($"UpdatePatchPaymentLogRequestStatusAsync is completed.");
        }


    }
}
