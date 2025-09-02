namespace OF.ConsentManagement.Model.CentralBank.Consent.PatchRequest;

public class CbPatchConsentRequest
{
    public PsuIdentifiers PsuIdentifiers { get; set; }
    public List<string> AccountIds { get; set; }
    public List<string> InsurancePolicyIds { get; set; }
    public SupplementaryInformation SupplementaryInformation { get; set; }
    public string Status { get; set; }

    public ConsentBody ConsentBody { get; set; }

    public string AuthorizationChannel { get; set; }
    public string ConnectToken { get; set; }
    public ConsentUsage ConsentUsage { get; set; }
}
public class PsuIdentifiers
{
    public string UserId { get; set; }
}

public class SupplementaryInformation
{
    // Add fields if supplementary information has details
}

public class ConsentBody
{
    public ConsentData Data { get; set; }
    public ConsentMeta Meta { get; set; }
}

public class ConsentData
{
    public string Status { get; set; }
    public ExchangeRate ExchangeRate { get; set; }
    public List<Charge> Charges { get; set; }
    public string RevokedBy { get; set; }
    public OpenFinanceBilling OpenFinanceBilling { get; set; }
}

public class ExchangeRate
{
    public string UnitCurrency { get; set; }
    public decimal ExchangeRateValue { get; set; }
    public string RateType { get; set; }
    public string ContractIdentification { get; set; }
    public DateTime ExpirationDateTime { get; set; }
}

public class Charge
{
    public string ChargeBearer { get; set; }
    public string Type { get; set; }
    public ChargeAmount Amount { get; set; }
}

public class ChargeAmount
{
    public string Amount { get; set; }
    public string Currency { get; set; }
}

public class OpenFinanceBilling
{
    public bool IsLargeCorporate { get; set; }
}

public class ConsentMeta
{
    public MultipleAuthorizers MultipleAuthorizers { get; set; }
}

public class MultipleAuthorizers
{
    public int TotalRequired { get; set; }
    public List<Authorization> Authorizations { get; set; }
}

public class Authorization
{
    public string AuthorizerId { get; set; }
    public string AuthorizerType { get; set; }
    public DateTime AuthorizationDate { get; set; }
    public string AuthorizationStatus { get; set; }
}

public class ConsentUsage
{
    public DateTime LastDataShared { get; set; }
    public DateTime LastServiceInitiationAttempt { get; set; }
}
