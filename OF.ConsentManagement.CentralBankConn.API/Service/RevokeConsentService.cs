using ConsentMangerModel.CoreBank;
using OF.ConsentManagement.CentralBankConn.API.Repositories;
using OF.ConsentManagement.Common.Custom;
using OF.ConsentManagement.Common.Helpers;
using OF.ConsentManagement.Model.Common;

namespace ConsentManagerService.Services
{
    public class RevokeConsentService : IRevokeConsentService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<CoreBankApis> _backEndApis;
        private readonly IMasterRepository _masterRepository;
        private readonly IOptions<ServiceParams> _serviceparams;
        public RevokeConsentService(HttpClient httpClient, IOptions<CoreBankApis> backEndApis, IMasterRepository masterRepository, IOptions<ServiceParams> serviceparams)
        {
            _httpClient = httpClient;
            _backEndApis = backEndApis;
            _masterRepository = masterRepository;
        }
        public async Task<IActionResult> RevokeConsentbyConsentGroupid(CbsConsentRevokedDto cbsrequest, Logger logger)
        {
            try
            {
                logger.Info("RevokeConsentbyConsentGroupid is Invoked.");

                if (_serviceparams.Value.Online)
                {
                    string jsonData = JsonConvert.SerializeObject(cbsrequest, Formatting.Indented);

                    logger.Debug($"CorrelationId: {cbsrequest.CorrelationId} || JsonRequestBody: {PciDssSecurity.MaskCardInDynamicJson(jsonData, logger)}");

                    var url = UrlHelper.CombineUrl(
                        _backEndApis.Value.BaseUrl!,
                        _backEndApis.Value.ConsentManagementUrl!.RevokeConsentByConsentGroupId!
                    );

                    if (string.IsNullOrWhiteSpace(url))
                    {
                        throw new InvalidOperationException("PaymentsServiceUrl.PostPayment is not configured.");
                    }

                    var content = GetStringContent(jsonData, cbsrequest.CorrelationId.ToString(), logger);

                    logger.Debug($"CorrelationId: {cbsrequest.CorrelationId} || Calling CoreBank API URL: {url}");

                    var request = new HttpRequestMessage(HttpMethod.Post, url)
                    {
                        Content = content
                    };
                    request.Headers.Add("X-Correlation-ID", cbsrequest.CorrelationId.ToString());

                    HttpResponseMessage apiResponse = await _httpClient.SendAsync(request);

                    string apiResponseBody = await apiResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                    if (string.IsNullOrWhiteSpace(apiResponseBody))
                    {
                        throw new InvalidOperationException("CoreBank API returned empty response body.");
                    }

                    logger.Debug($"CorrelationId: {cbsrequest.CorrelationId} || Receiving Response data: {PciDssSecurity.MaskCardInDynamicJson(apiResponseBody, logger)}");

                    if (apiResponse.IsSuccessStatusCode)
                    {
                        logger.Debug($"CorrelationId: {cbsrequest.CorrelationId} || API Response: SUCCESS");
                        return new NoContentResult(); // ✅ works here
                    }
                    else
                    {
                        var message = "Returned Failure Response";
                        return new BadRequestObjectResult(message); // ✅ works here
                    }
                }
                else
                {
                    return GetOfflineResponse(); // ✅ make sure this returns IActionResult
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                var message= ex.Message;
                return new BadRequestObjectResult(message);
            }
        }
        public async Task<IActionResult> RevokeConsentbyConsentId(CbsConsentRevokedDto cbsrequest, Logger logger)
        {
            try
            {
                logger.Info("RevokeConsentbyConsentId is Invoked.");

                if (_serviceparams.Value.Online)
                {
                    string jsonData = JsonConvert.SerializeObject(cbsrequest, Formatting.Indented);

                    logger.Debug($"CorrelationId: {cbsrequest.CorrelationId} || JsonRequestBody: {PciDssSecurity.MaskCardInDynamicJson(jsonData, logger)}");

                    var url = UrlHelper.CombineUrl(
                        _backEndApis.Value.BaseUrl!,
                        _backEndApis.Value.ConsentManagementUrl.RevokeConsentById!
                    );

                    if (string.IsNullOrWhiteSpace(url))
                    {
                        throw new InvalidOperationException("RevokeConsentbyConsentId.RevokeConsentbyConsentId is not configured.");
                    }

                    var content = GetStringContent(jsonData, cbsrequest.CorrelationId.ToString(), logger);

                    logger.Debug($"CorrelationId: {cbsrequest.CorrelationId} || Calling CoreBank API URL: {url}");

                    var request = new HttpRequestMessage(HttpMethod.Post, url)
                    {
                        Content = content
                    };
                    request.Headers.Add("X-Correlation-ID", cbsrequest.CorrelationId.ToString());

                    HttpResponseMessage apiResponse = await _httpClient.SendAsync(request);

                    string apiResponseBody = await apiResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                    if (string.IsNullOrWhiteSpace(apiResponseBody))
                    {
                        throw new InvalidOperationException("CoreBank API returned empty response body.");
                    }

                    logger.Debug($"CorrelationId: {cbsrequest.CorrelationId} || Receiving Response data: {PciDssSecurity.MaskCardInDynamicJson(apiResponseBody, logger)}");

                    if (apiResponse.IsSuccessStatusCode)
                    {
                        logger.Debug($"CorrelationId: {cbsrequest.CorrelationId} || API Response: SUCCESS");
                        return new NoContentResult();
                    }
                    else
                    {
                        var message = "Returned Failure Response";
                        return new BadRequestObjectResult(message); 
                    }
                }
                else
                {
                    return GetOfflineResponse();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                var message = ex.Message;
                return new BadRequestObjectResult(message);
            }
        }
        private IActionResult GetOfflineResponse()
        {
            return new NoContentResult();
        }
        private StringContent GetStringContent(string jsonContent, string correlationId, Logger logger)
        {
            try
            {
                logger.Debug($"CorrelationId: {correlationId} || Preparing StringContent for HTTP request.");
                return new StringContent(jsonContent, Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"CorrelationId: {correlationId} || Failed to create StringContent.");
                throw new InvalidOperationException("Failed to create HTTP request content.", ex);
            }
        }
    }
}
