using ConsentManagerBackEndAPI.IServices;
using ConsentManagerCommon.NLog;
using ConsentMangerModel.Consent;
using Microsoft.IdentityModel.Tokens;
using OF.ConsentManagement.Common.Helpers;
using OF.ConsentManagement.Common.NLog;
using OF.ConsentManagement.Model.Common;
using OF.ConsentManagement.Model.CoreBank;

namespace ConsentManagerBackEndAPI.Controllers
{
    [ApiController] 
    public class PaymentsApiController : Controller
    {
        private readonly IPaymentsService _paymentsService;
        private readonly PaymentsApiLogger _paymentsLogger;
        private readonly WarmUpLogger _warmUpLogger;
        private readonly IOptions<ServiceParams> _serviceParams;

        public PaymentsApiController(IPaymentsService paymentsService, IOptions<ServiceParams> serviceParams, PaymentsApiLogger paymentsLogger, WarmUpLogger warmUpLogger)
        {
            _paymentsService = paymentsService;
            _serviceParams = serviceParams;
            _paymentsLogger = paymentsLogger;
            _warmUpLogger = warmUpLogger;
        }

        [HttpPost]
        [Route("/GetPaymentLog")]
        public async Task<IActionResult> GetPaymentLog(CbGetAuditConsentByConsentIdRequestDto cbsRequest)
        {
            try
            {
                ApiResult<GetAuditConsentsByConsentIdResponse>? result = null;
                if (!ModelState.IsValid)
                {
                    var Message = "Invalid request model.";
                    return BadRequest(Message);
                }

                if (string.IsNullOrWhiteSpace(cbsRequest.ConsentId))
                {
                    return BadRequest(ApiResultFactory.Failure<GetAuditConsentsByConsentIdResponse>("ConsentId is required."));
                }

                if (string.IsNullOrWhiteSpace(cbsRequest.AccountId))
                {
                    return BadRequest(ApiResultFactory.Failure<GetAuditConsentsByConsentIdResponse>("AccountId is required."));
                }

                if (cbsRequest != null)
                {

                    result = await _paymentsService.GetPaymentLogDetails(cbsRequest!);

                    if (!result.Success)
                    {
                        if (string.IsNullOrEmpty(result.Code))
                        {
                            return StatusCode(502, result); // or 504 depending on scenario
                        }

                        if (int.TryParse(result.Code, out int statusCode))
                        {
                            return StatusCode(statusCode, result);
                        }

                        return StatusCode(500, result); // General failure
                    }
                }

                return Ok(ApiResultFactory.Success(result!.Data!, "Payment fetched successfully"));

            }
            catch (Exception ex)
            {
                _paymentsLogger.Error(ex);
                return BadRequest(ApiResultFactory.Failure<GetAuditConsentsByConsentIdResponse>("An unexpected error occurred."));
            }
        }

        [HttpPost]
        [Route("/PatchPaymentLog")]
        public async Task<IActionResult> PatchPaymentLog(CbPatchPaymentRecordRequestDto cbsRequest)
        {
            var cbsResponse = new ApiResult<string>
            {
                Success = false,
                Code = "400",
                Message = string.Empty,
                Data = null
            };
            try
            {
                ApiResult<string>? result = null;
                if (!ModelState.IsValid)
                {
                    cbsResponse.Message = "Invalid request model.";
                    return BadRequest(cbsResponse);
                }

                if (string.IsNullOrWhiteSpace(cbsRequest.Id))
                {
                    return BadRequest(ApiResultFactory.Failure<string>("PaymentId is required."));
                }

                if (cbsRequest != null)
                {
                    // cbsRequest.OurReferenceNumber = await _cbsFxQuotesRepository
                    //.GetNextSeqAsync("GetPayment", cbsRequest.PaymentId!, _paymentsLogger.Log);

                    result = await _paymentsService.PatchPaymentLogDetails(cbsRequest!);

                    if (result!.Success)
                    {
                        if (string.IsNullOrEmpty(result.Message))
                        {
                            return StatusCode(502, result); // or 504 depending on scenario
                        }

                        if (int.TryParse(result.Code, out int statusCode))
                        {
                            return StatusCode(statusCode, result);
                        }

                        return StatusCode(500, result); // General failure
                    }
                }

                return Ok(ApiResultFactory.Success(result!.Message!, "Payment status updated successfully"));

            }
            catch (Exception ex)
            {
                _paymentsLogger.Error(ex);
                return BadRequest(ApiResultFactory.Failure<string>("An unexpected error occurred."));
            }
        }

    }
}
