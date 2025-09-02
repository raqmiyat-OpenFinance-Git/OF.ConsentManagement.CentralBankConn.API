namespace OF.ConsentManagement.Model.CentralBank.Consent.PostResponse;

public class CbPostConsentResponse
{
    public ConsentResponseData Data { get; set; }
    public object Meta { get; set; }
}

public class ConsentResponseData
{
    public string Id { get; set; }
    public string ParId { get; set; }
    public string RarType { get; set; }
    public string StandardVersion { get; set; }
    public string ConsentGroupId { get; set; }
    public string RequestUrl { get; set; }
    public string ConsentType { get; set; }
    public string Status { get; set; }

    public ConsentRequestDto Request { get; set; }
    public Dictionary<string, string> RequestHeaders { get; set; }
    public ConsentBody ConsentBody { get; set; }

    public string InteractionId { get; set; }
    public TppInfo Tpp { get; set; }
    public object OzoneSupplementaryInformation { get; set; }
    public long UpdatedAt { get; set; }
}

#region Request
public class ConsentRequestDto
{
    public string Type { get; set; }
    public ConsentDetails Consent { get; set; }
    public Subscription Subscription { get; set; }
}

public class ConsentDetails
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
    public bool? IsLargeCorporate { get; set; }  // appears in ConsentBody only
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
#endregion

#region ConsentBody
public class ConsentBody
{
    public ConsentBodyData Data { get; set; }
    public ConsentMeta Meta { get; set; }
    public Subscription Subscription { get; set; }
}

public class ConsentBodyData
{
    public string BaseConsentId { get; set; }
    public DateTime ExpirationDateTime { get; set; }
    public DateTime TransactionFromDateTime { get; set; }
    public DateTime TransactionToDateTime { get; set; }
    public List<string> AccountType { get; set; }
    public List<string> AccountSubType { get; set; }
    public OnBehalfOf OnBehalfOf { get; set; }
    public string Status { get; set; }
    public string RevokedBy { get; set; }
    public DateTime CreationDateTime { get; set; }
    public string ConsentId { get; set; }
    public List<string> Permissions { get; set; }
    public OpenFinanceBilling OpenFinanceBilling { get; set; }
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
#endregion

#region TPP
public class TppInfo
{
    public string ClientId { get; set; }
    public string TppId { get; set; }
    public string TppName { get; set; }
    public string SoftwareStatementId { get; set; }
    public string DirectoryRecord { get; set; }
    public DecodedSsa DecodedSsa { get; set; }
    public string OrgId { get; set; }
}

public class DecodedSsa
{
    public List<string> Redirect_uris { get; set; }
    public string Client_name { get; set; }
    public string Client_uri { get; set; }
    public string Logo_uri { get; set; }
    public string Jwks_uri { get; set; }
    public string Client_id { get; set; }
    public List<string> Roles { get; set; }
    public string Sector_identifier_uri { get; set; }
    public string Application_type { get; set; }
    public string Organisation_id { get; set; }
}
#endregion