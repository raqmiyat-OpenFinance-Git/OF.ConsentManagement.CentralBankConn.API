using OF.ConsentManagement.CentralBankConn.API.Repositories;
using OF.ConsentManagement.Common.Custom;
using OF.ConsentManagement.Common.Helpers;
using OF.ConsentManagement.Model.Common;
using OF.ConsentManagement.Model.CoreBank;

namespace OF.ConsentManagement.CentralBankConn.API.Services;
public class ConsentManagementService : IConsentManagementService
{
    private readonly HttpClient _httpClient;
    private readonly IOptions<CoreBankApis> _coreBankApis;
    private readonly IMasterRepository _masterRepository;
    public ConsentManagementService(HttpClient httpClient, IOptions<CoreBankApis> coreBankApis, IMasterRepository masterRepository)
    {
        _httpClient = httpClient;
        _coreBankApis = coreBankApis;
        _masterRepository = masterRepository;
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

    public async Task<ApiResult<CbsPostConsentResponse>> SendConsentToCoreBankAsync(CbsPostConsentRequest cbsRequest, Logger logger)
    {
        ApiResult<CbsPostConsentResponse>? apiResult = null;

        try
        {
            string jsonData = JsonConvert.SerializeObject(cbsRequest, Formatting.Indented);

            logger.Debug($"CorrelationId: {cbsRequest.CorrelationId} || JsonRequestBody: {PciDssSecurity.MaskCardInDynamicJson(jsonData, logger)}");

            var url = UrlHelper.CombineUrl(_coreBankApis.Value.BaseUrl!, _coreBankApis.Value.ConsentManagementUrl!.PostConsent!);

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("FxQuotesServiceUrl.PostFxQuotes is not configured.");
            }

            var content = GetStringContent(jsonData, cbsRequest.CorrelationId.ToString(), logger);

            //_httpClient.Timeout = TimeSpan.FromSeconds(30); // or pull from config
            logger.Debug($"CorrelationId: {cbsRequest.CorrelationId} || Calling CoreBank API URL: {url}");

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };
            request.Headers.Add("X-Correlation-ID", cbsRequest.CorrelationId.ToString());

            HttpResponseMessage apiResponse = await _httpClient.SendAsync(request);

            string apiResponseBody = await apiResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(apiResponseBody))
            {
                throw new InvalidOperationException("CoreBank API returned empty response body.");
            }

            logger.Debug($"CorrelationId: {cbsRequest.CorrelationId} || Receiving Response data: {PciDssSecurity.MaskCardInDynamicJson(apiResponseBody, logger)}");

            if (apiResponse.IsSuccessStatusCode)
            {
                logger.Debug($"CorrelationId: {cbsRequest.CorrelationId} || API Response: SUCCESS");

                var coreBankPostFxQuotesResponse = JsonConvert.DeserializeObject<CbsPostConsentResponse>(apiResponseBody) ?? throw new InvalidOperationException("Deserialized CoreBankCustomerResponse is null.");
                apiResult = ApiResultFactory.Success(data: coreBankPostFxQuotesResponse!, apiResponseBody, "200");
            }
            else
            {
                logger.Warn($"CorrelationId: {cbsRequest.CorrelationId}|| OurReferenceNumber: {cbsRequest.OurReferenceNumber} || API FAILED with status {(int)apiResponse.StatusCode} - {apiResponse.ReasonPhrase}");

                apiResult = ApiResultFactory.Failure<CbsPostConsentResponse>(apiResponseBody, ((int)apiResponse.StatusCode).ToString());
            }

        }
        catch (HttpRequestException ex) when (ex.StatusCode.HasValue)
        {
            logger.Error(ex,
            $"Method: MakePaymentToCoreBankAsync || CorrelationId: {cbsRequest.CorrelationId.ToString() ?? "N/A"} || " +
            $"HTTP error. StatusCode: {(int)(ex.StatusCode ?? 0)} - {ex.StatusCode} || Message: {ex.Message}");

            return ApiResultFactory.Failure<CbsPostConsentResponse>("Internal server error", "502");
        }

        catch (Exception ex)
        {
            logger.Error(ex, $"Unhandled exception: {ex.Message}");
            return ApiResultFactory.Failure<CbsPostConsentResponse>("Internal server error", "500");
        }

        return apiResult!;
    }
    public async Task<ApiResult<CbsGetConsentResponse>> GetConsentsFromCoreBankAsync(CbsGetConsentRequest cbsRequest, Logger logger)
    {
        ApiResult<CbsGetConsentResponse>? apiResult = null;

        try
        {
            string jsonData = JsonConvert.SerializeObject(cbsRequest, Formatting.Indented);

            logger.Debug($"CorrelationId: {cbsRequest.CorrelationId} || JsonRequestBody: {PciDssSecurity.MaskCardInDynamicJson(jsonData, logger)}");

            var url = UrlHelper.CombineUrl(_coreBankApis.Value.BaseUrl!, _coreBankApis.Value.ConsentManagementUrl!.GetConsents!);

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("FxQuotesServiceUrl.GetFxQuote is not configured.");
            }

            var content = GetStringContent(jsonData, cbsRequest.CorrelationId.ToString(), logger);

            //_httpClient.Timeout = TimeSpan.FromSeconds(30); // or pull from config
            logger.Debug($"CorrelationId: {cbsRequest.CorrelationId} || Calling CoreBank API URL: {url}");

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };
            request.Headers.Add("X-Correlation-ID", cbsRequest.CorrelationId.ToString());

            HttpResponseMessage apiResponse = await _httpClient.SendAsync(request);

            string apiResponseBody = await apiResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(apiResponseBody))
            {
                throw new InvalidOperationException("CoreBank API returned empty response body.");
            }

            logger.Debug($"CorrelationId: {cbsRequest.CorrelationId} || Receiving Response data: {PciDssSecurity.MaskCardInDynamicJson(apiResponseBody, logger)}");

            if (apiResponse.IsSuccessStatusCode)
            {
                logger.Debug($"CorrelationId: {cbsRequest.CorrelationId} || API Response: SUCCESS");

                var coreBankGetFxQuoteResponse = JsonConvert.DeserializeObject<CbsGetConsentResponse>(apiResponseBody) ?? throw new InvalidOperationException("Deserialized CoreBankCustomerResponse is null.");
                apiResult = ApiResultFactory.Success(data: coreBankGetFxQuoteResponse!, apiResponseBody, "200");
            }
            else
            {
                logger.Warn($"CorrelationId: {cbsRequest.CorrelationId}|| OurReferenceNumber: {cbsRequest.OurReferenceNumber} || API FAILED with status {(int)apiResponse.StatusCode} - {apiResponse.ReasonPhrase}");

                apiResult = ApiResultFactory.Failure<CbsGetConsentResponse>(apiResponseBody, ((int)apiResponse.StatusCode).ToString());
            }

        }
        catch (HttpRequestException ex) when (ex.StatusCode.HasValue)
        {
            logger.Error(ex,
            $"Method: GetFxQuoteFromCoreBankAsync || CorrelationId: {cbsRequest.CorrelationId.ToString() ?? "N/A"} || " +
            $"HTTP error. StatusCode: {(int)(ex.StatusCode ?? 0)} - {ex.StatusCode} || Message: {ex.Message}");

            return ApiResultFactory.Failure<CbsGetConsentResponse>("Internal server error", "502");
        }
        catch (Exception ex)
        {
            logger.Error(ex, $"Unhandled exception: {ex.Message}");
            return ApiResultFactory.Failure<CbsGetConsentResponse>("Internal server error", "500");
        }

        return apiResult!;
    }
    public async Task<ApiResult<CbsGetConsentResponse>> GetConsentByIdFromCoreBankAsync(CbsGetConsentRequest cbsRequest, Logger logger)
    {
        ApiResult<CbsGetConsentResponse>? apiResult = null;

        try
        {
            string jsonData = JsonConvert.SerializeObject(cbsRequest, Formatting.Indented);

            logger.Debug($"CorrelationId: {cbsRequest.CorrelationId} || JsonRequestBody: {PciDssSecurity.MaskCardInDynamicJson(jsonData, logger)}");

            var url = UrlHelper.CombineUrl(_coreBankApis.Value.BaseUrl!, _coreBankApis.Value.ConsentManagementUrl!.GetConsentById!);

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("FxQuotesServiceUrl.GetFxQuote is not configured.");
            }

            var content = GetStringContent(jsonData, cbsRequest.CorrelationId.ToString(), logger);

            //_httpClient.Timeout = TimeSpan.FromSeconds(30); // or pull from config
            logger.Debug($"CorrelationId: {cbsRequest.CorrelationId} || Calling CoreBank API URL: {url}");

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };
            request.Headers.Add("X-Correlation-ID", cbsRequest.CorrelationId.ToString());

            HttpResponseMessage apiResponse = await _httpClient.SendAsync(request);

            string apiResponseBody = await apiResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(apiResponseBody))
            {
                throw new InvalidOperationException("CoreBank API returned empty response body.");
            }

            logger.Debug($"CorrelationId: {cbsRequest.CorrelationId} || Receiving Response data: {PciDssSecurity.MaskCardInDynamicJson(apiResponseBody, logger)}");

            if (apiResponse.IsSuccessStatusCode)
            {
                logger.Debug($"CorrelationId: {cbsRequest.CorrelationId} || API Response: SUCCESS");

                var coreBankGetFxQuoteResponse = JsonConvert.DeserializeObject<CbsGetConsentResponse>(apiResponseBody) ?? throw new InvalidOperationException("Deserialized CoreBankCustomerResponse is null.");
                apiResult = ApiResultFactory.Success(data: coreBankGetFxQuoteResponse!, apiResponseBody, "200");
            }
            else
            {
                logger.Warn($"CorrelationId: {cbsRequest.CorrelationId}|| OurReferenceNumber: {cbsRequest.OurReferenceNumber} || API FAILED with status {(int)apiResponse.StatusCode} - {apiResponse.ReasonPhrase}");

                apiResult = ApiResultFactory.Failure<CbsGetConsentResponse>(apiResponseBody, ((int)apiResponse.StatusCode).ToString());
            }

        }
        catch (HttpRequestException ex) when (ex.StatusCode.HasValue)
        {
            logger.Error(ex,
            $"Method: GetFxQuoteFromCoreBankAsync || CorrelationId: {cbsRequest.CorrelationId.ToString() ?? "N/A"} || " +
            $"HTTP error. StatusCode: {(int)(ex.StatusCode ?? 0)} - {ex.StatusCode} || Message: {ex.Message}");

            return ApiResultFactory.Failure<CbsGetConsentResponse>("Internal server error", "502");
        }
        catch (Exception ex)
        {
            logger.Error(ex, $"Unhandled exception: {ex.Message}");
            return ApiResultFactory.Failure<CbsGetConsentResponse>("Internal server error", "500");
        }

        return apiResult!;
    }

    public async Task<ApiResult<CbsPatchConsentResponse>> SendConsentUpdateToCoreBankAsync(CbsPatchConsentRequest cbsRequest, Logger logger)
    {
        ApiResult<CbsPatchConsentResponse>? apiResult = null;

        try
        {
            string jsonData = JsonConvert.SerializeObject(cbsRequest, Formatting.Indented);

            logger.Debug($"CorrelationId: {cbsRequest.CorrelationId} || JsonRequestBody: {PciDssSecurity.MaskCardInDynamicJson(jsonData, logger)}");

            var url = UrlHelper.CombineUrl(_coreBankApis.Value.BaseUrl!, _coreBankApis.Value.ConsentManagementUrl!.UpdateConsent!);

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("FxQuotesServiceUrl.UpdateFxQuotes is not configured.");
            }

            var content = GetStringContent(jsonData, cbsRequest.CorrelationId.ToString(), logger);

            //_httpClient.Timeout = TimeSpan.FromSeconds(30); // or pull from config
            logger.Debug($"CorrelationId: {cbsRequest.CorrelationId} || Calling CoreBank API URL: {url}");

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };
            request.Headers.Add("X-Correlation-ID", cbsRequest.CorrelationId.ToString());

            HttpResponseMessage apiResponse = await _httpClient.SendAsync(request);

            string apiResponseBody = await apiResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(apiResponseBody))
            {
                throw new InvalidOperationException("CoreBank API returned empty response body.");
            }

            logger.Debug($"CorrelationId: {cbsRequest.CorrelationId} || Receiving Response data: {PciDssSecurity.MaskCardInDynamicJson(apiResponseBody, logger)}");

            if (apiResponse.IsSuccessStatusCode)
            {
                logger.Debug($"CorrelationId: {cbsRequest.CorrelationId} || API Response: SUCCESS");

                var coreBankPatchFxQuotesResponse = JsonConvert.DeserializeObject<CbsPatchConsentResponse>(apiResponseBody) ?? throw new InvalidOperationException("Deserialized CoreBankCustomerResponse is null.");
                apiResult = ApiResultFactory.Success(data: coreBankPatchFxQuotesResponse!, apiResponseBody, "200");
            }
            else
            {
                logger.Warn($"CorrelationId: {cbsRequest.CorrelationId}|| OurReferenceNumber: {cbsRequest.OurReferenceNumber} || API FAILED with status {(int)apiResponse.StatusCode} - {apiResponse.ReasonPhrase}");

                apiResult = ApiResultFactory.Failure<CbsPatchConsentResponse>(apiResponseBody, ((int)apiResponse.StatusCode).ToString());
            }

        }
        catch (HttpRequestException ex) when (ex.StatusCode.HasValue)
        {
            logger.Error(ex,
            $"Method: MakePaymentToCoreBankAsync || CorrelationId: {cbsRequest.CorrelationId.ToString() ?? "N/A"} || " +
            $"HTTP error. StatusCode: {(int)(ex.StatusCode ?? 0)} - {ex.StatusCode} || Message: {ex.Message}");

            return ApiResultFactory.Failure<CbsPatchConsentResponse>("Internal server error", "502");
        }

        catch (Exception ex)
        {
            logger.Error(ex, $"Unhandled exception: {ex.Message}");
            return ApiResultFactory.Failure<CbsPatchConsentResponse>("Internal server error", "500");
        }

        return apiResult!;
    }
}
