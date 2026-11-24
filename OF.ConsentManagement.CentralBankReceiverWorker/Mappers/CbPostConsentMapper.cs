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

        if (requestDto.cbPostConsentRequest?.consent == null)
            throw new ArgumentNullException(nameof(requestDto.cbPostConsentRequest.consent));

        if (requestDto.cbPostConsentRequest?.PersonalIdentifiableInformation == null)
            throw new ArgumentNullException(nameof(requestDto.cbPostConsentRequest.PersonalIdentifiableInformation));


        var request = requestDto.cbPostConsentRequest;
        var consent = request.consent;
        var pII = request.PersonalIdentifiableInformation;
        ConsentRequest consentRequest = new ConsentRequest();

        consentRequest.CorrelationId = requestDto.CorrelationId;
        consentRequest.Type = request.Type;
        consentRequest.ConsentId = consent.ConsentId;
        consentRequest.BaseConsentId = consent.BaseConsentId;
        consentRequest.ExpirationDateTime = consent.ExpirationDateTime;
        consentRequest.TransactionFromDateTime = consent.TransactionFromDateTime;
        consentRequest.TransactionToDateTime = consent.TransactionToDateTime;
        var firstAccountType = consent.accountType?.FirstOrDefault();
        var accountSubType = consent.accountSubType?.FirstOrDefault();
        consentRequest.AccountType = firstAccountType.ToString();
        consentRequest.AccountSubType = accountSubType.ToString();

        consentRequest.OnBehalfOfTradingName = consent.OnBehalfOf?.TradingName;
        consentRequest.OnBehalfOfLegalName = consent.OnBehalfOf?.LegalName;
        consentRequest.OnBehalfOfIdentifierType = consent.OnBehalfOf?.IdentifierType;
        consentRequest.OnBehalfOfIdentifier = consent.OnBehalfOf?.Identifier;

        consentRequest.Permission = string.Join(",", consent!.Permissions!);

        consentRequest.BillingIsLargeCorporate = true;
        consentRequest.BillingUserType = consent.OpenFinanceBilling?.UserType.ToString()!;
        consentRequest.BillingPurpose = consent.OpenFinanceBilling?.Purpose.ToString()!;

        consentRequest.TppId = pII.Initiation.TppId;
        consentRequest.TppName = pII.Initiation.TppName;

        consentRequest.DebtorAccountSchemeName = pII.Initiation.DebtorAccount.SchemeName.ToString();
        consentRequest.DebtorAccountIdentification = pII.Initiation.DebtorAccount.Identification;
        consentRequest.DebtorAccountName = pII.Initiation.DebtorAccount.Name;

        foreach (var creditor in pII.Initiation.Creditor)
        {
            consentRequest.CreditorAccountSchemeName = creditor.CreditorAccount.SchemeName.ToString();
            consentRequest.CreditorAccountIdentification = creditor.CreditorAccount.Identification;
            consentRequest.CreditorAccountName = creditor.CreditorAccount.Name;
            consentRequest.CreditorAccountTradingName = creditor.CreditorAccount.TradingName;

            foreach (var address in creditor.Creditor.PostalAddress)
            {
                consentRequest.CreditorPostalAddressFloorNumber = address.FloorNumber;
                consentRequest.CreditorPostalAddressBuildingNumber = address.BuildingNumber;
                consentRequest.CreditorPostalAddressStreetName = address.StreetName;
                consentRequest.CreditorPostalAddressSecondaryNumber = address.SecondaryNumber;
                consentRequest.CreditorPostalAddressDistrict = address.District;
                consentRequest.CreditorPostalAddressPostalCode = address.PostalCode;
                consentRequest.CreditorPostalAddressPOBox = address.POBox;
                consentRequest.CreditorPostalAddressZipCode = address.ZipCode;
                consentRequest.CreditorPostalAddressCity = address.City;
                consentRequest.CreditorPostalAddressRegion = address.Region;
                consentRequest.CreditorPostalAddressCountry = address.Country;
            }

        }
        consentRequest.WebhookUrl = request.Subscription?.Webhook?.Url;
        consentRequest.WebhookIsActive = request.Subscription?.Webhook?.IsActive ?? false;
        consentRequest.WebhookSecret = string.Empty;

        // System / tracking fields
        consentRequest.CreatedBy = "System";
        consentRequest.CreatedOn = DateTime.Now;
        consentRequest.Status = "PENDING";

        // Serialize full request for traceability
        consentRequest.RequestPayload = JsonConvert.SerializeObject(requestDto, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });



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
            ResponseStatus = data.Status.ToString(),
            InteractionId = data.InteractionId,
            ConsentStatus = data.ConsentBody?.Data?.Status,
            RevokedBy = null, // 👈 double-check business meaning, placeholder for now
            AuthorizerId = firstAuthorization?.AuthorizerId,
            AuthorizerType = firstAuthorization?.AuthorizerType,
            AuthorizationDate = firstAuthorization?.AuthorizationDate,
            AuthorizationStatus = firstAuthorization?.AuthorizationStatus,
            PsuUserId = Guid.NewGuid().ToString(),

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
            CreatedOn = DateTime.Now,
            CreatedBy = "ConsentManagement Backend Worker"
        };

        return consentResponse;
    }


}
