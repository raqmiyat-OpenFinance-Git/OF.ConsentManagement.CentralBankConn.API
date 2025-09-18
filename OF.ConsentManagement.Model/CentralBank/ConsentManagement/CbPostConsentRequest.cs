namespace OF.ConsentManagement.Model.CentralBank.Consent.PostRequest;

public class CbPostConsentRequest
{
    public string? Type { get; set; }
    public Consent? Consent { get; set; }
    public PersonalIdentifiableInformation? PersonalIdentifiableInformation { get; set; }
    public Subscription? Subscription { get; set; }
}

public class Consent
{
    public string? BaseConsentId { get; set; }
    public DateTime? ExpirationDateTime { get; set; }
    public DateTime? TransactionFromDateTime { get; set; }
    public DateTime? TransactionToDateTime { get; set; }
    public List<string>? AccountType { get; set; }
    public List<string>? AccountSubType { get; set; }
    public OnBehalfOf? OnBehalfOf { get; set; }
    public string? ConsentId { get; set; }
    public List<string>? Permissions { get; set; }
    public OpenFinanceBilling? OpenFinanceBilling { get; set; }
}

public class OnBehalfOf
{
    public string? TradingName { get; set; }
    public string? LegalName { get; set; }
    public string? IdentifierType { get; set; }
    public string? Identifier { get; set; }
}

public class OpenFinanceBilling
{
    public string? UserType { get; set; }
    public string? Purpose { get; set; }
}

public class Subscription
{
    public Webhook? Webhook { get; set; }
}

public class Webhook
{
    public string? Url { get; set; }
    public bool IsActive { get; set; }
}

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class CreditorDetails
{
    public CreditorAgent? CreditorAgent { get; set; }
    public Creditor? Creditor { get; set; }
    public CreditorAccount? CreditorAccount { get; set; }
    public string? Name { get; set; }
    public List<PostalAddress>? PostalAddress { get; set; }
}

public class CreditorAccount
{
    public string? SchemeName { get; set; }
    public string? Identification { get; set; }
    public string? Name { get; set; }
    public string? TradingName { get; set; }
}
public class Creditor
{
    public string? Name { get; set; }
    public List<PostalAddress>? PostalAddress { get; set; }
}

public class CreditorAgent
{
    public string? SchemeName { get; set; }
    public string? Identification { get; set; }
    public string? Name { get; set; }
    public List<PostalAddress>? PostalAddress { get; set; }
}

public class DebtorAccount
{
    public string? SchemeName { get; set; }
    public string? Identification { get; set; }
    public string? Name { get; set; }
}

public class Initiation
{
    public string? TppId { get; set; }
    public string? TppName { get; set; }
    public DebtorAccount? DebtorAccount { get; set; }
    public List<CreditorDetails>? Creditor { get; set; }
}

public class PersonalIdentifiableInformation
{
    public Initiation? Initiation { get; set; }
}

public class PostalAddress
{
    public string? FloorNumber { get; set; }
    public string? BuildingNumber { get; set; }
    public string? StreetName { get; set; }
    public string? SecondaryNumber { get; set; }
    public string? District { get; set; }
    public string? PostalCode { get; set; }
    public string? POBox { get; set; }
    public string? ZipCode { get; set; }
    public string? City { get; set; }
    public string? Region { get; set; }
    public string? Country { get; set; }
}


