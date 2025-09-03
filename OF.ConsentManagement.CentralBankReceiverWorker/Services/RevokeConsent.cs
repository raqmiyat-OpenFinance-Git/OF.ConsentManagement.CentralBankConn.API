using ConsentMangerModel.CoreBank;

namespace ConsentManagerBackendReceiverWorker.Services
{
    public class RevokeConsent:IRevokeConsent
    {
        private readonly PostConsentDbContext _context;
        private readonly IDbConnection _dbConnection;
        public RevokeConsent(PostConsentDbContext context, IDbConnection dbConnection)
        {
            _context = context;
            _dbConnection = dbConnection;
        }

        public async Task RevokeConsentAsync (CbsConsentRevokedDto consentRequest, Logger logger)
        {
            if (consentRequest == null)
            {
                logger.Warn("RevokeConsent: Received null consentRequest.");
                return;
            }

            try
            {
                var update = await _context.ConsentRequest
                    .FirstOrDefaultAsync(x => x.BaseConsentId == consentRequest.ConsentGroupId);

                if (update == null)
                {
                    logger.Warn($"RevokeConsent: No matching ConsentRequest found for ConsentGroupId: {consentRequest.ConsentGroupId}");
                    return;
                }

                // Update entity
                update.Status = "Revoked";
                update.Revokedby = consentRequest.Revokedby ?? string.Empty;
                update.RevokedPsuUserId = consentRequest.PsuUserId ?? string.Empty;

                // EF will track changes automatically — no need to call Update() explicitly
                await _context.SaveChangesAsync();

                logger.Info(
                    $"RevokeConsent successful. CorrelationId: {consentRequest.CorrelationId}, ConsentGroupId: {consentRequest.ConsentGroupId}"
                );
            }
            catch (DbUpdateException dbEx)
            {
                logger.Error(dbEx, $"Database update error while revoking consent. CorrelationId: {consentRequest?.CorrelationId}");
                throw;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Unexpected error in RevokeConsent. CorrelationId: {consentRequest?.CorrelationId}");
                throw;
            }
        }

        public async Task RevokeConsentbyIdAsync(CbsConsentRevokedDto consentRequest, Logger logger)
        {
            if (consentRequest == null)
            {
                logger.Warn("RevokeConsent: Received null consentRequest.");
                return;
            }

            try
            {
                var update = await _context.ConsentRequest
                    .FirstOrDefaultAsync(x => x.ConsentId == consentRequest.ConsentGroupId);

                if (update == null)
                {
                    logger.Warn($"RevokeConsent: No matching ConsentRequest found for ConsentGroupId: {consentRequest.ConsentGroupId}");
                    return;
                }

                // Update entity
                update.Status = "Revoked";
                update.Revokedby = consentRequest.Revokedby ?? string.Empty;
                update.RevokedPsuUserId = consentRequest.PsuUserId ?? string.Empty;

                // EF will track changes automatically — no need to call Update() explicitly
                await _context.SaveChangesAsync();

                logger.Info(
                    $"RevokeConsent successful. CorrelationId: {consentRequest.CorrelationId}, ConsentGroupId: {consentRequest.ConsentGroupId}"
                );
            }
            catch (DbUpdateException dbEx)
            {
                logger.Error(dbEx, $"Database update error while revoking consent. CorrelationId: {consentRequest?.CorrelationId}");
                throw;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Unexpected error in RevokeConsent. CorrelationId: {consentRequest?.CorrelationId}");
                throw;
            }
        }

    }
}
