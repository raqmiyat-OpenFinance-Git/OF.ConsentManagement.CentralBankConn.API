using OF.ConsentManagement.Model.CentralBank.Consent.GetRequestDto;
using OF.ConsentManagement.Model.CentralBank.Consent.GetResponseDto;
using OF.ConsentManagement.Model.EFModel;

namespace CentralBankReceiverWorker.Mappers;

public static class CbGetConsentMapper
{
    public static ConsentStatusHistory MapCbGetConsentRequestToEF(CbGetConsentRequestDto requestDto)
    {
        if (requestDto == null)
            throw new ArgumentNullException(nameof(requestDto));

        if (requestDto.cbGetConsentQueryParameters == null)
            throw new ArgumentNullException(nameof(requestDto.cbGetConsentQueryParameters));

        var queryParameters = requestDto.cbGetConsentQueryParameters;

        var consentStatusHistory = new ConsentStatusHistory
        {
            // Query Parameter mapping
            QueryParamUpdatedAt = queryParameters.UpdatedAt,
            QueryParamConsentType = queryParameters.ConsentType ?? string.Empty,
            QueryParamStatus = queryParameters.Status ?? "UNKNOWN",
            QueryParamPage = queryParameters.Page,
            QueryParamPageSize = queryParameters.PageSize,
            CorrelationId = queryParameters.CorrelationId,
            StatusCode = "PENDING",
            CreatedBy = "System",
            CreatedOn = DateTime.UtcNow,
            // Serialize payload
            RequestPayload = JsonConvert.SerializeObject(requestDto, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            })


        };

        return consentStatusHistory;
    }

    public static List<ConsentResponseHistory> MapCbGetConsentResponseToEF(CbGetConsentResponseDto responseDto, IEnumerable<ConsentIdentifier> consentIdentifiers, long consentStatusHistoryId)
    {
        if (responseDto == null)
            throw new ArgumentNullException(nameof(responseDto));

        var response = responseDto.cbGetConsentResponse
                       ?? throw new ArgumentNullException(nameof(responseDto.cbGetConsentResponse));

        var data = responseDto.cbGetConsentResponse.Data
        ?? throw new ArgumentNullException(nameof(responseDto.cbGetConsentResponse.Data));

        List<ConsentResponseHistory> consentResponseHistories = new List<ConsentResponseHistory>();

        foreach (var da in data)
        {
            var consentId = da.ConsentBody.Data.ConsentId;
            var consentRequestId = consentIdentifiers.Where(r => r.ConsentId == consentId).Select(r => r.ConsentRequestId).FirstOrDefault();
            ConsentResponseHistory consentResponseHistory = new ConsentResponseHistory();
            consentResponseHistory.ConsentStatusHistoryId = consentStatusHistoryId;
            consentResponseHistory.ConsentRequestId = consentRequestId;
            consentResponseHistory.PsuUserId = da.PsuIdentifiers.UserId;
            consentResponseHistory.AccountIds = da.AccountIds.FirstOrDefault();
            consentResponseHistory.InsurancePolicyIds = da.InsurancePolicyIds.FirstOrDefault();

            if (da.SupplementaryInformation?.Count > 0)
            {
                consentResponseHistory.SupplementaryInformation =
                    JsonConvert.SerializeObject(da.SupplementaryInformation, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    });
            }

            if (da.PaymentContext?.Count > 0)
            {
                consentResponseHistory.PaymentContext =
                    JsonConvert.SerializeObject(da.PaymentContext, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        NullValueHandling = NullValueHandling.Ignore
                    });
            }

            consentResponseHistory.ConnectToken = da.ConnectToken;
            if (da.ConsentUsage != null)
            {
                consentResponseHistory.LastDataShared = da.ConsentUsage.LastDataShared;
                consentResponseHistory.LastServiceInitiationAttempt = da.ConsentUsage.LastServiceInitiationAttempt;
            }
            consentResponseHistory.AuthorizationChannel = da.AuthorizationChannel;
            consentResponseHistories.Add(consentResponseHistory);
        }

       
        return consentResponseHistories;
    }

    public static ConsentStatusHistory MapCbGetConsentStatusHistory(CbGetConsentResponseDto responseDto, IEnumerable<ConsentIdentifier> consentIdentifiers, long consentStatusHistoryId)
    {
        if (responseDto == null)
            throw new ArgumentNullException(nameof(responseDto));

        var response = responseDto.cbGetConsentResponse
                       ?? throw new ArgumentNullException(nameof(responseDto.cbGetConsentResponse));

        var consentStatusHistory = new ConsentStatusHistory
        {
            // Response mapping
            MetaPageNumber = response.Meta.PageNumber,
            MetaPageSize = response.Meta.PageSize,
            MetaTotalPages = response.Meta.TotalPages,
            MetaTotalRecords = response.Meta.TotalRecords,

            StatusCode = responseDto.Status,
            ChangedOn = DateTime.UtcNow,

            // Serialize payload
            ResponsePayload = JsonConvert.SerializeObject(responseDto.cbGetConsentResponse, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            })
        };

        return consentStatusHistory;
    }

    public static List<string> MapCbGetConsentIds(CbGetConsentResponseDto responseDto)
    {
        if (responseDto == null)
            throw new ArgumentNullException(nameof(responseDto));

        var response = responseDto.cbGetConsentResponse
            ?? throw new ArgumentNullException(nameof(responseDto.cbGetConsentResponse));

        var data = response.Data
            ?? throw new ArgumentNullException(nameof(response.Data));

        var consentIds = new List<string>();

        foreach (var item in data)
        {
            var consentId = item?.ConsentBody?.Data?.ConsentId;
            if (!string.IsNullOrWhiteSpace(consentId))
            {
                consentIds.Add(consentId);
            }
        }

        return consentIds;
    }

}
