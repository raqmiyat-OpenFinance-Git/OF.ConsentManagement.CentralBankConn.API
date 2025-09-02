namespace OF.ConsentManagement.Model.EFModel;

public class ConsentResponse
{
    [Key]
    public long ConsentResponseId { get; set; }

    public long? ConsentRequestId { get; set; }   // FK to ConsentRequest
    public string ParId { get; set; }
    public string RarType { get; set; }
    public string StandardVersion { get; set; }
    public string ConsentGroupId { get; set; }
    public string RequestUrl { get; set; }
    public string ConsentType { get; set; }
    public string ResponseStatus { get; set; }
    public string InteractionId { get; set; }
    public string ConsentStatus { get; set; }
    public string RevokedBy { get; set; }

    public string AuthorizerId { get; set; }
    public string AuthorizerType { get; set; }
    public DateTime? AuthorizationDate { get; set; }
    public string AuthorizationStatus { get; set; }


    public string? PsuUserId { get; set; }
    public string? AccountIds { get; set; }   // JSON array stored as string
    public string? InsurancePolicyIds { get; set; } // JSON array stored as string
    public string? SupplementaryInformation { get; set; }
    public string? PaymentContext { get; set; }
    public string? ConnectToken { get; set; }
    public DateTime? LastDataShared { get; set; }
    public DateTime? LastServiceInitiationAttempt { get; set; }
    public string? AuthorizationChannel { get; set; }

    // ExchangeRate
    public string? UnitCurrency { get; set; }
    public decimal? ExchangeRate { get; set; }
    public string? RateType { get; set; }
    public string? ContractIdentification { get; set; }
    public DateTime? ExpirationDateTime { get; set; }

    // Charges
    public string? ChargeBearer { get; set; }
    public string? ChargeBearerType { get; set; }
    public decimal? ChargeBearerAmount { get; set; }
    public string? ChargeBearerCurrency { get; set; }

    public string TppClientId { get; set; }
    public string TppId { get; set; }
    public string TppName { get; set; }
    public string TppSoftwareStatementId { get; set; }
    public string TppDirectoryRecord { get; set; }
    public string TppOrgId { get; set; }

    public string DecodedSsaRedirect_uris { get; set; }
    public string DecodedSsaClientName { get; set; }
    public string DecodedSsaClientUri { get; set; }
    public string DecodedSsaLogoUri { get; set; }
    public string DecodedSsaJwksUri { get; set; }
    public string DecodedSsaClientId_Roles { get; set; }
    public string DecodedSsaSectorIdentifierUri { get; set; }
    public string DecodedSsaApplicationType { get; set; }
    public string DecodedSsaOrganisationId { get; set; }

    public long? UpdatedAt { get; set; }
    public string Status { get; set; }

    public string CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }

    public string RequestPayload { get; set; }
    public string RequestUpdatePayload { get; set; }
    public string ResponsePayload { get; set; }
    public string ResponseUpdatePayload { get; set; }

    // Navigation property (EF Core)
    public ConsentRequest ConsentRequest { get; set; }
}