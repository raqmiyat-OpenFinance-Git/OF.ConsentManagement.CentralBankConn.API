using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsentMangerModel.Consent
{
    public class GetAuditConsentsByConsentIdResponse
    {
        public List<Datum>? data { get; set; }
        public Meta? meta { get; set; }
    }
    public class AmountData
    {
        public string? Amount { get; set; }
        public string? Currency { get; set; }
    }

    public class AuthorizationCode
    {
        public string? paymentId { get; set; }
        public string? accessTokenHash { get; set; }
        public string? currentDateTime { get; set; }
    }

    public class CurrencyRequest
    {
        public string? InstructionPriority { get; set; }
        public string? ExtendedPurpose { get; set; }
        public string? ChargeBearer { get; set; }
        public string? CurrencyOfTransfer { get; set; }
        public string? DestinationCountryCode { get; set; }
        public ExchangeRateInformation? ExchangeRateInformation { get; set; }
        public string? FxQuoteId { get; set; }
    }

    public class RequestData
    {
        public string? ConsentId { get; set; }
        public Instruction? Instruction { get; set; }
        public CurrencyRequest? CurrencyRequest { get; set; }
        public string? PersonalIdentifiableInformation { get; set; }
        public string? PaymentPurposeCode { get; set; }
        public string? DebtorReference { get; set; }
        public string? CreditorReference { get; set; }
    }

    public class Datum
    {
        public string? consentId { get; set; }
        public string? paymentType { get; set; }
        public string? paymentId { get; set; }
        public string? idempotencyKey { get; set; }
        public PaymentResponse? paymentResponse { get; set; }
        public string? signedResponse { get; set; }
        public Tpp? tpp { get; set; }
        public int? accountId { get; set; }
        public PsuIdentifiers? psuIdentifiers { get; set; }
        public InteractionId? interactionId { get; set; }
        public AuthorizationCode? authorizationCode { get; set; }
        public RequestBody? requestBody { get; set; }
        public string? signedRequestBody { get; set; }
        public RequestHeaders? requestHeaders { get; set; }
    }

    public class DecodedSsa
    {
        public List<string>? redirect_uris { get; set; }
        public string? client_name { get; set; }
        public string? client_uri { get; set; }
        public string? logo_uri { get; set; }
        public string? jwks_uri { get; set; }
        public string? client_id { get; set; }
        public List<string>? roles { get; set; }
        public string? sector_identifier_uri { get; set; }
        public string? application_type { get; set; }
        public string? organisation_id { get; set; }
    }

    public class ExchangeRateInformation
    {
        public string? UnitCurrency { get; set; }
        public double? ExchangeRate { get; set; }
        public string? RateType { get; set; }
        public string? ContractIdentification { get; set; }
    }

    public class Instruction
    {
        public AmountData? Amount { get; set; }
    }

    public class InteractionId
    {
        public string? ozoneInteractionId { get; set; }
        public string? clientInteractionId { get; set; }
    }

    public class Meta
    {
        public int? TotalRecords { get; set; }

    }

    public class OpenFinanceBilling
    {
        public string? Type { get; set; }
        public string? MerchantId { get; set; }
        public int? NumberOfSuccessfulTransactions { get; set; }
    }

    public class PaymentResponse
    {
        public string? id { get; set; }
        public string? status { get; set; }
        public string? creationDateTime { get; set; }
        public string? statusUpdateDateTime { get; set; }
        public OpenFinanceBilling? OpenFinanceBilling { get; set; }
    }

    public class PsuIdentifiers
    {
        public string? userId { get; set; }
    }

    public class RequestBody
    {
        public RequestData? Data { get; set; }
    }

    public class RequestHeaders
    {
        [FromHeader(Name = "o3-provider-id")]
        public string? O3ProviderId { get; set; }

        [FromHeader(Name = "o3-caller-org-id")]
        public string? O3CallerOrgId { get; set; }

        [FromHeader(Name = "o3-caller-client-id")]
        public string? O3CallerClientId { get; set; }

        [FromHeader(Name = "o3-caller-software-statement-id")]
        public string? O3CallerSoftwareStatementId { get; set; }

        [FromHeader(Name = "o3-api-uri")]
        public string? O3ApiUri { get; set; }

        [FromHeader(Name = "o3-api-operation")]
        public string? O3ApiOperation { get; set; }

        [FromHeader(Name = "o3-caller-interaction-id")]
        public string? O3CallerInteractionId { get; set; }

        [FromHeader(Name = "o3-ozone-interaction-id")]
        public string? O3OzoneInteractionId { get; set; }
    }

    public class Tpp
    {
        public string? clientId { get; set; }
        public string? tppId { get; set; }
        public string? tppName { get; set; }
        public string? softwareStatementId { get; set; }
        public string? directoryRecord { get; set; }
        public DecodedSsa? decodedSsa { get; set; }
        public string? orgId { get; set; }
    }


}
