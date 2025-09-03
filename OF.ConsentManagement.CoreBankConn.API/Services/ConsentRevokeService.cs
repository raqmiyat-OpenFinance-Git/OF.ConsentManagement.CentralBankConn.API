using ConsentManagerBackEndAPI.IServices;
using ConsentMangerModel.CoreBank;

namespace ConsentManagerBackEndAPI.Services
{
    public class ConsentRevokeService : IConsentRevoke
    {
        private readonly IDbConnection _dbConnection;

        public ConsentRevokeService(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IActionResult> PostConsentRevokebyConsentgrpId(CbsConsentRevokedDto request, Logger logger)
        {
            try
            {
                logger.Info("PostConsentRevokebyConsentgrpId is Invoked.");
                return new NoContentResult(); 
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return new BadRequestObjectResult(ex.Message); 
            }
        }

        public async Task<IActionResult> PostConsentRevokebyConsentId(CbsConsentRevokedDto request, Logger logger)
        {
            try
            {
                logger.Info("PostConsentRevokebyConsentId is Invoked.");
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
