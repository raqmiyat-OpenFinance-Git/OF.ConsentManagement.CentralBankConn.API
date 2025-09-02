using OF.ConsentManagement.Model.CentralBank.Consent.PostRequestDto;
using OF.ConsentManagement.Model.CentralBank.Consent.PostResponseDto;
using OF.ConsentManagement.Model.EFModel;

namespace CentralBankReceiverWorker.Mappers;
public static class CbPostConsentMapper
{
    public static ConsentRequest MapCbPostConsentRequestToEF(CbPostConsentRequestDto requestDto)
    {
        if (requestDto == null)
            throw new ArgumentNullException(nameof(requestDto));

        if (requestDto.cbPostConsentRequest?.Consent == null)
            throw new ArgumentNullException(nameof(requestDto.cbPostConsentRequest.Consent));

        var request = requestDto.cbPostConsentRequest;
        var consent = request.Consent;

        var consentRequest = new ConsentRequest
        {
            CorrelationId = requestDto.CorrelationId,
            Type = request.Type,
            ConsentId = consent.ConsentId,
            BaseConsentId = consent.BaseConsentId,
            ExpirationDateTime = consent.ExpirationDateTime,
            TransactionFromDateTime = consent.TransactionFromDateTime,
            TransactionToDateTime = consent.TransactionToDateTime,

            AccountType = consent.AccountType?.FirstOrDefault(),
            AccountSubType = consent.AccountSubType?.FirstOrDefault(),

            OnBehalfOfTradingName = consent.OnBehalfOf?.TradingName,
            OnBehalfOfLegalName = consent.OnBehalfOf?.LegalName,
            OnBehalfOfIdentifierType = consent.OnBehalfOf?.IdentifierType,
            OnBehalfOfIdentifier = consent.OnBehalfOf?.Identifier,

            Permission = consent.Permissions?.FirstOrDefault(),

            BillingIsLargeCorporate = true,
            BillingUserType = consent.OpenFinanceBilling?.UserType,
            BillingPurpose = consent.OpenFinanceBilling?.Purpose,

            WebhookUrl = request.Subscription?.Webhook?.Url,
            WebhookIsActive = request.Subscription?.Webhook?.IsActive ?? false,
            WebhookSecret = string.Empty,

            // System / tracking fields
            CreatedBy = "System",
            CreatedOn = DateTime.UtcNow,
            Status = "PENDING",

            // Serialize full request for traceability
            RequestPayload = JsonConvert.SerializeObject(requestDto, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            })
        };

        return consentRequest;
    }


    public static ConsentResponse MapCbPostConsentResponseToEF(CbPostConsentResponseDto responseDto)
    {
        if (responseDto == null)
            throw new ArgumentNullException(nameof(responseDto));

        var response = responseDto.cbPostConsentResponse
                       ?? throw new ArgumentNullException(nameof(responseDto.cbPostConsentResponse));

        var data = response.Data
                   ?? throw new ArgumentNullException(nameof(response.Data), "Response data is null");

        var firstAuthorization = data.ConsentBody?.Meta?.MultipleAuthorizers?.Authorizations?.FirstOrDefault();
        var firstRedirectUri = data.Tpp?.DecodedSsa?.Redirect_uris?.FirstOrDefault();
        var firstRole = data.Tpp?.DecodedSsa?.Roles?.FirstOrDefault();

        var consentResponse = new ConsentResponse
        {
            ParId = data.ParId,
            RarType = data.RarType,
            StandardVersion = data.StandardVersion,
            ConsentGroupId = data.ConsentGroupId,
            RequestUrl = data.RequestUrl,
            ConsentType = data.ConsentType,
            ResponseStatus = data.Status,
            InteractionId = data.InteractionId,
            ConsentStatus = data.ConsentBody?.Data?.Status,
            RevokedBy = null, // 👈 double-check business meaning, placeholder for now
            AuthorizerId = firstAuthorization?.AuthorizerId,
            AuthorizerType = firstAuthorization?.AuthorizerType,
            AuthorizationDate = firstAuthorization?.AuthorizationDate,
            AuthorizationStatus = firstAuthorization?.AuthorizationStatus,

            TppClientId = data.Tpp?.ClientId,
            TppId = data.Tpp?.TppId,
            TppName = data.Tpp?.TppName,
            TppSoftwareStatementId = data.Tpp?.SoftwareStatementId,
            TppDirectoryRecord = data.Tpp?.DirectoryRecord,
            TppOrgId = data.Tpp?.OrgId,

            DecodedSsaRedirect_uris = firstRedirectUri,
            DecodedSsaClientName = data.Tpp?.DecodedSsa?.Client_name,
            DecodedSsaClientUri = data.Tpp?.DecodedSsa?.Client_uri,
            DecodedSsaLogoUri = data.Tpp?.DecodedSsa?.Logo_uri,
            DecodedSsaJwksUri = data.Tpp?.DecodedSsa?.Jwks_uri,
            DecodedSsaClientId_Roles = firstRole,
            DecodedSsaSectorIdentifierUri = data.Tpp?.DecodedSsa?.Sector_identifier_uri,
            DecodedSsaApplicationType = data.Tpp?.DecodedSsa?.Application_type,
            DecodedSsaOrganisationId = data.Tpp?.DecodedSsa?.Organisation_id,

            ResponsePayload = JsonConvert.SerializeObject(response, Formatting.None),
            CreatedOn = DateTime.UtcNow,
            CreatedBy = "ConsentManagement Backend Worker"
        };

        return consentResponse;
    }


}
