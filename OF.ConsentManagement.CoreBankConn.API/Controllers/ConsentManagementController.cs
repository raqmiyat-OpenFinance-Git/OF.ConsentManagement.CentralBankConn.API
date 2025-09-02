using OF.ConsentManagement.Common.Helpers;
using OF.ConsentManagement.Common.NLog;
using OF.ConsentManagement.CoreBankConn.API.IServices;
using OF.ConsentManagement.CoreBankConn.API.Repositories;
using OF.ConsentManagement.Model.Common;
using OF.ConsentManagement.Model.CoreBank;

namespace OF.ConsentManagement.CoreBankConn.API.Controllers;
[ApiController]
public class ConsentManagementController : ControllerBase
{
    private readonly IConsentManagementService _consentService;
    private readonly ICbsRepository _cbsRepository;

    private readonly ConsentLogger _logger;
    private readonly IOptions<ServiceParams> _serviceParams;

    public ConsentManagementController(IConsentManagementService consentService, ICbsRepository cbsRepository, IOptions<ServiceParams> serviceParams, ConsentLogger logger)
    {
        _consentService = consentService;
        _cbsRepository = cbsRepository;
        _serviceParams = serviceParams;
        _logger = logger;
    }

    [HttpPost]
    [Route("/CreateConsent")]
    public async Task<IActionResult> CreateConsent(CbsPostConsentRequest cbsRequest)
    {
        var cbsResponse = new ApiResult<CbsPostConsentResponse>
        {
            Success = false,
            Code = "400",
            Message = string.Empty,
            Data = null
        };
        try
        {
            ApiResult<CbsPostConsentResponse>? result = null;
            if (!ModelState.IsValid)
            {
                cbsResponse.Message = "Invalid request model.";
                return BadRequest(cbsResponse);
            }

            if (string.IsNullOrWhiteSpace(cbsRequest.ConsentId))
            {
                return BadRequest(ApiResultFactory.Failure<CbsPostConsentResponse>("ConsentId is required."));
            }

            if (string.IsNullOrWhiteSpace(cbsRequest.PaymentId))
            {
                return BadRequest(ApiResultFactory.Failure<CbsPostConsentResponse>("PaymentId is required."));
            }
            if (cbsRequest != null)
            {

                cbsRequest.OurReferenceNumber = await _cbsRepository
               .GetNextSeqAsync("CreateFxQuote", cbsRequest.PaymentId!, _logger.Log);

                result = await _consentService.SendConsentToCoreBankAsync(cbsRequest!, _logger.Log);

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


            return Ok(ApiResultFactory.Success(result!.Data!, "Consent posted successfully"));

        }
        catch (Exception ex)
        {
            _logger.Error(ex);
            return BadRequest(ApiResultFactory.Failure<CbsPostConsentResponse>("An unexpected error occurred."));
        }
    }
    [HttpPost]
    [Route("/PatchConsent")]
    public async Task<IActionResult> PatchConsent(CbsPatchConsentRequest cbsRequest)
    {
        var cbsResponse = new ApiResult<CbsPatchConsentResponse>
        {
            Success = false,
            Code = "400",
            Message = string.Empty,
            Data = null
        };
        try
        {
            ApiResult<CbsPatchConsentResponse>? result = null;
            if (!ModelState.IsValid)
            {
                cbsResponse.Message = "Invalid request model.";
                return BadRequest(cbsResponse);
            }

            if (string.IsNullOrWhiteSpace(cbsRequest.ConsentId))
            {
                return BadRequest(ApiResultFactory.Failure<CbsPatchConsentResponse>("ConsentId is required."));
            }

            if (string.IsNullOrWhiteSpace(cbsRequest.PaymentId))
            {
                return BadRequest(ApiResultFactory.Failure<CbsPatchConsentResponse>("PaymentId is required."));
            }
            if (cbsRequest != null)
            {

                cbsRequest.OurReferenceNumber = await _cbsRepository
               .GetNextSeqAsync("PatchConsent", cbsRequest.PaymentId!, _logger.Log);

                result = await _consentService.SendConsentUpdateToCoreBankAsync(cbsRequest!, _logger.Log);

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


            return Ok(ApiResultFactory.Success(result!.Data!, "Payments posted successfully"));

        }
        catch (Exception ex)
        {
            _logger.Error(ex);
            return BadRequest(ApiResultFactory.Failure<CbsPatchConsentResponse>("An unexpected error occurred."));
        }
    }

    [HttpPost]
    [Route("/ReadConsents")]
    public async Task<IActionResult> ReadConsents(CbsGetConsentRequest cbsRequest)
    {
        var cbsResponse = new ApiResult<CbsGetConsentResponse>
        {
            Success = false,
            Code = "400",
            Message = string.Empty,
            Data = null
        };
        try
        {
            ApiResult<CbsGetConsentResponse>? result = null;
            if (!ModelState.IsValid)
            {
                cbsResponse.Message = "Invalid request model.";
                return BadRequest(cbsResponse);
            }

            if (string.IsNullOrWhiteSpace(cbsRequest.ConsentId))
            {
                return BadRequest(ApiResultFactory.Failure<CbsGetConsentResponse>("ConsentId is required."));
            }

            if (string.IsNullOrWhiteSpace(cbsRequest.PaymentId))
            {
                return BadRequest(ApiResultFactory.Failure<CbsGetConsentResponse>("PaymentId is required."));
            }

            if (cbsRequest != null)
            {
                cbsRequest.OurReferenceNumber = await _cbsRepository
               .GetNextSeqAsync("ReadConsents", cbsRequest.PaymentId!, _logger.Log);

                result = await _consentService.GetConsentsFromCoreBankAsync(cbsRequest!, _logger.Log);

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


            return Ok(ApiResultFactory.Success(result.Data!, "FxQuote readed successfully"));

        }
        catch (Exception ex)
        {
            _logger.Error(ex);
            return BadRequest(ApiResultFactory.Failure<CbsGetConsentResponse>("An unexpected error occurred."));
        }
    }

    [HttpPost]
    [Route("/ReadConsentById")]
    public async Task<IActionResult> ReadConsentById(CbsGetConsentRequest cbsRequest)
    {
        var cbsResponse = new ApiResult<CbsGetConsentResponse>
        {
            Success = false,
            Code = "400",
            Message = string.Empty,
            Data = null
        };
        try
        {
            ApiResult<CbsGetConsentResponse>? result = null;
            if (!ModelState.IsValid)
            {
                cbsResponse.Message = "Invalid request model.";
                return BadRequest(cbsResponse);
            }

            if (string.IsNullOrWhiteSpace(cbsRequest.ConsentId))
            {
                return BadRequest(ApiResultFactory.Failure<CbsGetConsentResponse>("ConsentId is required."));
            }

            if (string.IsNullOrWhiteSpace(cbsRequest.PaymentId))
            {
                return BadRequest(ApiResultFactory.Failure<CbsGetConsentResponse>("PaymentId is required."));
            }

            if (cbsRequest != null)
            {
                cbsRequest.OurReferenceNumber = await _cbsRepository
               .GetNextSeqAsync("ReadConsents", cbsRequest.PaymentId!, _logger.Log);

                result = await _consentService.GetConsentByIdFromCoreBankAsync(cbsRequest!, _logger.Log);

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


            return Ok(ApiResultFactory.Success(result.Data!, "FxQuote readed successfully"));

        }
        catch (Exception ex)
        {
            _logger.Error(ex);
            return BadRequest(ApiResultFactory.Failure<CbsGetConsentResponse>("An unexpected error occurred."));
        }
    }


}