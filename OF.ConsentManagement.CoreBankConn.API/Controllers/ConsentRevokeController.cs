using ConsentManagerBackEndAPI.IServices;
using ConsentManagerCommon.Logging;
using ConsentMangerModel.Consent;
using ConsentMangerModel.CoreBank;
using Microsoft.AspNetCore.Http.HttpResults;
using OF.ConsentManagement.Common.Helpers;

namespace ConsentManagerBackEndAPI.Controllers
{
    public class ConsentRevokeController : Controller
    {
        private readonly IConsentRevoke _consentService;
        private readonly RevokeConsentApiLogger _logger;
        private readonly WarmUpLogger _warmUpLogger;
        private readonly IOptions<ServiceParams> _serviceParams;

        public ConsentRevokeController(IConsentRevoke consentService, IOptions<ServiceParams> serviceParams, RevokeConsentApiLogger logger, WarmUpLogger warmUpLogger)
        {
            _consentService = consentService;
            _serviceParams = serviceParams;
            _logger = logger;
            _warmUpLogger = warmUpLogger;
        }

        [HttpPost]
        [Route("/PostConsentRevokebyConsentGroupId")]
        public async Task<IActionResult> RevokeConsentbyConsentGrpId(CbsConsentRevokedDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var Message = "Invalid request model.";
                    return BadRequest(Message);
                }

                if (string.IsNullOrWhiteSpace(request.ConsentGroupId))
                {
                    return BadRequest("ConsentGroupId is required.");
                }

                if (string.IsNullOrWhiteSpace(request.Revokedby))
                {
                    return BadRequest("Revokedby is required.");
                }

                if (request != null)
                {

                    var result = await _consentService.PostConsentRevokebyConsentgrpId(request!,_logger.Log);

                    if (result is NoContent)
                    {
                        return NoContent();
                    }
                    else
                    {
                        var message = "Internal Server Error";
                        return BadRequest(message);
                    }
                }
                return NoContent();


            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return BadRequest(ApiResultFactory.Failure<GetAuditConsentsByConsentIdResponse>("An unexpected error occurred."));
            }
        }


        [HttpPost]
        [Route("/PostConsentRevokebyConsentId")]
        public async Task<IActionResult> PostConsentRevokebyConsentId(CbsConsentRevokedDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var Message = "Invalid request model.";
                    return BadRequest(Message);
                }

                if (string.IsNullOrWhiteSpace(request.ConsentId))
                {
                    return BadRequest("ConsentId is required.");
                }

                if (string.IsNullOrWhiteSpace(request.Revokedby))
                {
                    return BadRequest("Revokedby is required.");
                }

                if (request != null)
                {

                    var result = await _consentService.PostConsentRevokebyConsentId(request!, _logger.Log);

                    if (result is NoContent)
                    {
                        return NoContent();
                    }
                    else
                    {
                        var message = "Internal Server Error";
                        return BadRequest(message);
                    }
                }
                return NoContent();


            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return BadRequest(ApiResultFactory.Failure<GetAuditConsentsByConsentIdResponse>("An unexpected error occurred."));
            }
        }

    }
}
