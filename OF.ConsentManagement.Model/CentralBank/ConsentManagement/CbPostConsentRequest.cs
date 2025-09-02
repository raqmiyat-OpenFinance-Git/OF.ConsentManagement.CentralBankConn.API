namespace OF.ConsentManagement.Model.CentralBank.Consent.PostRequest;

public class CbPostConsentRequest
{
    public string Type { get; set; }
    public Consent Consent { get; set; }
    public Subscription Subscription { get; set; }
}

public class Consent
{
    public string BaseConsentId { get; set; }
    public DateTime ExpirationDateTime { get; set; }
    public DateTime TransactionFromDateTime { get; set; }
    public DateTime TransactionToDateTime { get; set; }
    public List<string> AccountType { get; set; }
    public List<string> AccountSubType { get; set; }
    public OnBehalfOf OnBehalfOf { get; set; }
    public string ConsentId { get; set; }
    public List<string> Permissions { get; set; }
    public OpenFinanceBilling OpenFinanceBilling { get; set; }
}

public class OnBehalfOf
{
    public string TradingName { get; set; }
    public string LegalName { get; set; }
    public string IdentifierType { get; set; }
    public string Identifier { get; set; }
}

public class OpenFinanceBilling
{
    public string UserType { get; set; }
    public string Purpose { get; set; }
}

public class Subscription
{
    public Webhook Webhook { get; set; }
}

public class Webhook
{
    public string Url { get; set; }
    public bool IsActive { get; set; }
}