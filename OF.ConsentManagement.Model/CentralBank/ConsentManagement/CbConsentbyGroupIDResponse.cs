namespace OF.ConsentManagement.Model.CentralBank.ConsentManagement
{
    public class ConsentBody

    {
        public string id { get; set; }
        public string parId { get; set; }
        public string rarType { get; set; }
        public string standardVersion { get; set; }
        public string consentGroupId { get; set; }
        public string requestUrl { get; set; }
        public string consentType { get; set; }
        public string status { get; set; }
        public Request request { get; set; }
        public RequestHeaders requestHeaders { get; set; }
        public string ConsentId { get; set; }
        public string BaseConsentId { get; set; }
        public bool IsSingleAuthorization { get; set; }
        public DateTime AuthorizationExpirationDateTime { get; set; }
        public List<string> Permissions { get; set; }
        public string AcceptedAuthorizationType { get; set; }
        public DateTime ExpirationDateTime { get; set; }
        public string Status { get; set; }
        public string RevokedBy { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime StatusUpdateDateTime { get; set; }
        public List<Charges> Charges { get; set; }
        public ExchangeRateResponse ExchangeRate { get; set; }
        public CurrencyRequest CurrencyRequest { get; set; }
        public ControlParameters ControlParameters { get; set; }
        public string DebtorReference { get; set; }
        public string CreditorReference { get; set; }
        public string PaymentPurposeCode { get; set; }
        public SponsoredTPPInformation SponsoredTPPInformation { get; set; }
        public PaymentConsumption PaymentConsumption { get; set; }
        public OpenFinanceBilling OpenFinanceBilling { get; set; }
        public ConsentBody consentBody { get; set; }
        public string interactionId { get; set; }
        public Tpp tpp { get; set; }
        public OzoneSupplementaryInformation ozoneSupplementaryInformation { get; set; }
        public double updatedAt { get; set; }
        public PsuIdentifiers psuIdentifiers { get; set; }
        public List<string> accountIds { get; set; }
        public List<string> insurancePolicyIds { get; set; }
        public SupplementaryInformation supplementaryInformation { get; set; }
        public PaymentContext paymentContext { get; set; }
        public string ConnectToken { get; set; }
        public ConsentUsage consentUsage { get; set; }
        public string authorizationChannel { get; set; }
    }


    public class Request
    {
        public string type { get; set; }
        public Consent consent { get; set; }
        public Subscription subscription { get; set; }
    }


    public class Consent
    {
        public string ConsentId { get; set; }
        public string BaseConsentId { get; set; }
        public bool IsSingleAuthorization { get; set; }
        public DateTime AuthorizationExpirationDateTime { get; set; }
        public DateTime ExpirationDateTime { get; set; }
        public List<string> Permissions { get; set; }
        public CurrencyRequest CurrencyRequest { get; set; }
        public PersonalIdentifiableInformation PersonalIdentifiableInformation { get; set; }
        public ControlParameters ControlParameters { get; set; }
        public string DebtorReference { get; set; }
        public string CreditorReference { get; set; }
        public string PaymentPurposeCode { get; set; }
        public SponsoredTPPInformation SponsoredTPPInformation { get; set; }
    }


    public class CurrencyRequest
    {
        public string InstructionPriority { get; set; }
        public string ExtendedPurpose { get; set; }
        public string ChargeBearer { get; set; }
        public string CurrencyOfTransfer { get; set; }
        public string DestinationCountryCode { get; set; }
        public ExchangeRateInformation ExchangeRateInformation { get; set; }
        public string FxQuoteId { get; set; }
    }


    public class ExchangeRateInformation
    {
        public string UnitCurrency { get; set; }
        public double ExchangeRate { get; set; }
        public string RateType { get; set; }
        public string ContractIdentification { get; set; }
    }


    public class PersonalIdentifiableInformation
    {
        public Initiation Initiation { get; set; }
        public Risk Risk { get; set; }
    }


    public class Initiation
    {
        public DebtorAccount DebtorAccount { get; set; }
        public List<CreditorResponse> Creditor { get; set; }
    }


    public class DebtorAccount
    {
        public string SchemeName { get; set; }
        public string Identification { get; set; }
        public Name Name { get; set; }
    }


    public class Name
    {
        public string en { get; set; }
        public string ar { get; set; }
    }


    public class CreditorResponse
    {
        public CreditorAgent CreditorAgent { get; set; }
        public string Name { get; set; }
        public List<PostalAddress> PostalAddress { get; set; }
        public CreditorResponse Creditor { get; set; }
        public CreditorAccount CreditorAccount { get; set; }
        public string ConfirmationOfPayeeResponse { get; set; }
    }


    public class CreditorAgent
    {
        public string SchemeName { get; set; }
        public string Identification { get; set; }
        public string Name { get; set; }
        public List<PostalAddress> PostalAddress { get; set; }
    }


    public class PostalAddress
    {
        public string AddressType { get; set; }
        public string ShortAddress { get; set; }
        public string UnitNumber { get; set; }
        public string FloorNumber { get; set; }
        public string BuildingNumber { get; set; }
        public string StreetName { get; set; }
        public string SecondaryNumber { get; set; }
        public string District { get; set; }
        public string PostalCode { get; set; }
        public string POBox { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
    }


    public class CreditorAccount
    {
        public string SchemeName { get; set; }
        public string Identification { get; set; }
        public Name Name { get; set; }
        public TradingName TradingName { get; set; }
    }


    public class TradingName
    {
        public string en { get; set; }
        public string ar { get; set; }
    }


    public class Risk
    {
        public DebtorIndicators DebtorIndicators { get; set; }
        public DestinationDeliveryAddress DestinationDeliveryAddress { get; set; }
        public TransactionIndicators TransactionIndicators { get; set; }
        public CreditorIndicators CreditorIndicators { get; set; }
    }


    public class DebtorIndicators
    {
        public Authentication Authentication { get; set; }
        public UserName UserName { get; set; }
        public GeoLocation GeoLocation { get; set; }
        public DeviceInformation DeviceInformation { get; set; }
        public BiometricCapabilities BiometricCapabilities { get; set; }
        public AppInformation AppInformation { get; set; }
        public BrowserInformation BrowserInformation { get; set; }
        public UserBehavior UserBehavior { get; set; }
        public AccountRiskIndicators AccountRiskIndicators { get; set; }
        public SupplementaryData SupplementaryData { get; set; }
    }


    public class Authentication
    {
        public string AuthenticationChannel { get; set; }
        public PossessionFactor PossessionFactor { get; set; }
        public KnowledgeFactor KnowledgeFactor { get; set; }
        public InherenceFactor InherenceFactor { get; set; }
        public string ChallengeOutcome { get; set; }
        public string AuthenticationFlow { get; set; }
        public string AuthenticationValue { get; set; }
        public DateTime ChallengeDateTime { get; set; }
    }


    public class PossessionFactor
    {
        public bool IsUsed { get; set; }
        public string Type { get; set; }
    }


    public class KnowledgeFactor
    {
        public bool IsUsed { get; set; }
        public string Type { get; set; }
    }


    public class InherenceFactor
    {
        public bool IsUsed { get; set; }
        public string Type { get; set; }
    }


    public class UserName
    {
        public string en { get; set; }
        public string ar { get; set; }
    }


    public class GeoLocation
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }


    public class DeviceInformation
    {
        public string DeviceId { get; set; }
        public string AlternativeDeviceId { get; set; }
        public string DeviceOperatingSystem { get; set; }
        public string DeviceOperatingSystemVersion { get; set; }
        public string DeviceBindingId { get; set; }
        public DateTime LastBindingDateTime { get; set; }
        public string BindingDuration { get; set; }
        public string BindingStatus { get; set; }
        public string DeviceType { get; set; }
        public DeviceManufacturer DeviceManufacturer { get; set; }
        public string DeviceLanguage { get; set; }
        public string DeviceLocalDateTime { get; set; }
        public string ConnectionType { get; set; }
        public ScreenInformation ScreenInformation { get; set; }
        public BatteryStatus BatteryStatus { get; set; }
        public TouchSupport TouchSupport { get; set; }
        public MotionSensors MotionSensors { get; set; }
        public List<string> DeviceEnvironmentContext { get; set; }
    }


    public class DeviceManufacturer
    {
        public string Model { get; set; }
        public string Manufacturer { get; set; }
    }


    public class ScreenInformation
    {
        public double PixelDensity { get; set; }
        public string Orientation { get; set; }
    }


    public class BatteryStatus
    {
        public double Level { get; set; }
        public bool IsCharging { get; set; }
    }


    public class TouchSupport
    {
        public bool Supported { get; set; }
        public int MaxTouchPoints { get; set; }
    }


    public class MotionSensors
    {
        public string Status { get; set; }
        public bool Accelerometer { get; set; }
        public bool Gyroscope { get; set; }
    }


    public class BiometricCapabilities
    {
        public bool SupportsBiometric { get; set; }
        public List<string> BiometricTypes { get; set; }
    }


    public class AppInformation
    {
        public string AppVersion { get; set; }
        public string PackageName { get; set; }
        public string BuildNumber { get; set; }
    }


    public class BrowserInformation
    {
        public string UserAgent { get; set; }
        public bool IsCookiesEnabled { get; set; }
        public List<string> AvailableFonts { get; set; }
        public List<string> Plugins { get; set; }
        public double PixelRatio { get; set; }
    }


    public class UserBehavior
    {
        public ScrollBehavior ScrollBehavior { get; set; }
    }


    public class ScrollBehavior
    {
        public string Direction { get; set; }
        public double Speed { get; set; }
        public double Frequency { get; set; }
    }


    public class AccountRiskIndicators
    {
        public DateTime UserOnboardingDateTime { get; set; }
        public DateTime LastAccountChangeDate { get; set; }
        public DateTime LastPasswordChangeDate { get; set; }
        public string SuspiciousActivity { get; set; }
        public TransactionHistory TransactionHistory { get; set; }
    }


    public class TransactionHistory
    {
        public int LastDay { get; set; }
        public int LastYear { get; set; }
    }


    public class SupplementaryData
    {
    }


    public class DestinationDeliveryAddress
    {
        public string RecipientType { get; set; }
        public RecipientName RecipientName { get; set; }
        public List<NationalAddress> NationalAddress { get; set; }
    }


    public class RecipientName
    {
        public string en { get; set; }
        public string ar { get; set; }
    }


    public class NationalAddress
    {
        public string AddressType { get; set; }
        public string ShortAddress { get; set; }
        public string UnitNumber { get; set; }
        public string FloorNumber { get; set; }
        public string BuildingNumber { get; set; }
        public string StreetName { get; set; }
        public string SecondaryNumber { get; set; }
        public string District { get; set; }
        public string PostalCode { get; set; }
        public string POBox { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
    }


    public class TransactionIndicators
    {
        public bool IsCustomerPresent { get; set; }
        public bool IsContractPresent { get; set; }
        public string Channel { get; set; }
        public string ChannelType { get; set; }
        public string SubChannelType { get; set; }
        public PaymentProcess PaymentProcess { get; set; }
        public MerchantRisk MerchantRisk { get; set; }
        public SupplementaryData SupplementaryData { get; set; }
    }


    public class PaymentProcess
    {
        public int TotalDuration { get; set; }
        public int CurrentSessionAttempts { get; set; }
        public int CurrentSessionFailedAttempts { get; set; }
        public int Last24HourAttempts { get; set; }
        public int Last24HourFailedAttempts { get; set; }
    }


    public class MerchantRisk
    {
        public string DeliveryTimeframe { get; set; }
        public string ReorderItemsIndicator { get; set; }
        public string PreOrderPurchaseIndicator { get; set; }
        public bool IsGiftCardPurchase { get; set; }
        public bool IsDeliveryAddressMatchesBilling { get; set; }
        public string AddressMatchLevel { get; set; }
    }


    public class CreditorIndicators
    {
        public string AccountType { get; set; }
        public bool IsCreditorPrePopulated { get; set; }
        public TradingName TradingName { get; set; }
        public bool IsVerifiedByTPP { get; set; }
        public List<AdditionalAccountHolderIdentifiers> AdditionalAccountHolderIdentifiers { get; set; }
        public MerchantDetails MerchantDetails { get; set; }
        public bool IsCreditorConfirmed { get; set; }
        public string ConfirmationOfPayeeResponse { get; set; }
        public SupplementaryData SupplementaryData { get; set; }
    }


    public class AdditionalAccountHolderIdentifiers
    {
        public string SchemeName { get; set; }
        public string Identification { get; set; }
        public Name Name { get; set; }
    }


    public class MerchantDetails
    {
        public string MerchantId { get; set; }
        public string MerchantName { get; set; }
        public string MerchantSICCode { get; set; }
        public string MerchantCategoryCode { get; set; }
    }


    public class ControlParameters
    {
        public bool IsDelegatedAuthentication { get; set; }
        public ConsentSchedule ConsentSchedule { get; set; }
    }


    public class ConsentSchedule
    {
        public SinglePayment SinglePayment { get; set; }
        public MultiPayment MultiPayment { get; set; }
        public FilePayment FilePayment { get; set; }
    }


    public class SinglePayment
    {
        public string Type { get; set; }
        public AmountResponse Amount { get; set; }
        public DateTime RequestedExecutionDate { get; set; }
    }


    public class AmountResponse
    {
        public string Currency { get; set; }
        public string Amount { get; set; }
    }


    public class MultiPayment
    {
        public MaximumCumulativeValueOfPayments MaximumCumulativeValueOfPayments { get; set; }
        public int MaximumCumulativeNumberOfPayments { get; set; }
        public PeriodicSchedule PeriodicSchedule { get; set; }
    }


    public class MaximumCumulativeValueOfPayments
    {
        public string Currency { get; set; }
        public string Amount { get; set; }
    }


    public class PeriodicSchedule
    {
        public string Type { get; set; }
        public string PeriodType { get; set; }
        public DateTime PeriodStartDate { get; set; }
        public Controls Controls { get; set; }
    }


    public class Controls
    {
        public MaximumIndividualAmount MaximumIndividualAmount { get; set; }
        public MaximumCumulativeValueOfPaymentsPerPeriod MaximumCumulativeValueOfPaymentsPerPeriod { get; set; }
        public int MaximumCumulativeNumberOfPaymentsPerPeriod { get; set; }
    }


    public class MaximumIndividualAmount
    {
        public string Currency { get; set; }
        public string Amount { get; set; }
    }


    public class MaximumCumulativeValueOfPaymentsPerPeriod
    {
        public string Currency { get; set; }
        public string Amount { get; set; }
    }


    public class FilePayment
    {
        public string FileType { get; set; }
        public string FileHash { get; set; }
        public string FileReference { get; set; }
        public int NumberOfTransactions { get; set; }
        public string ControlSum { get; set; }
        public DateTime RequestedExecutionDate { get; set; }
    }


    public class SponsoredTPPInformation
    {
        public string Name { get; set; }
        public string Identification { get; set; }
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


    public class RequestHeaders
    {
    }


    public class CbConsentbyGroupIDResponse
    {
        public ConsentBody Data { get; set; }
        public Subscription Subscription { get; set; }
        public Meta Meta { get; set; }
    }


    public class Charges
    {
        public string ChargeBearer { get; set; }
        public string Type { get; set; }
        public AmountResponse Amount { get; set; }
    }


    public class ExchangeRateResponse
    {
        public string UnitCurrency { get; set; }
        public double ExchangeRate { get; set; }
        public string RateType { get; set; }
        public string ContractIdentification { get; set; }
        public DateTime ExpirationDateTime { get; set; }
    }


    public class PaymentConsumption
    {
        public int CumulativeNumberOfPayments { get; set; }
        public CumulativeValueOfPayments CumulativeValueOfPayments { get; set; }
        public int CumulativeNumberOfPaymentsInCurrentPeriod { get; set; }
        public CumulativeValueOfPaymentsInCurrentPeriod CumulativeValueOfPaymentsInCurrentPeriod { get; set; }
    }


    public class CumulativeValueOfPayments
    {
        public string Amount { get; set; }
        public string Currency { get; set; }
    }


    public class CumulativeValueOfPaymentsInCurrentPeriod
    {
        public string Amount { get; set; }
        public string Currency { get; set; }
    }


    public class OpenFinanceBilling
    {
        public bool IsLargeCorporate { get; set; }
    }


    public class Meta
    {
        public double TotalRequired { get; set; }
        public List<Authorizations> Authorizations { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int totalPages { get; set; }
        public int totalRecords { get; set; }
    }


    public class Authorizations
    {
        public string AuthorizerId { get; set; }
        public string AuthorizerType { get; set; }
        public DateTime AuthorizationDate { get; set; }
        public string AuthorizationStatus { get; set; }
    }


    public class Tpp
    {
        public string clientId { get; set; }
        public string tppId { get; set; }
        public string tppName { get; set; }
        public string softwareStatementId { get; set; }
        public string directoryRecord { get; set; }
        public DecodedSsa decodedSsa { get; set; }
        public string orgId { get; set; }
    }


    public class DecodedSsa
    {
        public List<string> redirect_uris { get; set; }
        public string client_name { get; set; }
        public string client_uri { get; set; }
        public string logo_uri { get; set; }
        public string jwks_uri { get; set; }
        public string client_id { get; set; }
        public List<string> roles { get; set; }
        public string sector_identifier_uri { get; set; }
        public string application_type { get; set; }
        public string organisation_id { get; set; }
    }


    public class OzoneSupplementaryInformation
    {
    }


    public class PsuIdentifiers
    {
        public string userId { get; set; }
    }


    public class SupplementaryInformation
    {
    }


    public class PaymentContext
    {
    }


    public class ConsentUsage
    {
        public DateTime lastDataShared { get; set; }
        public DateTime lastServiceInitiationAttempt { get; set; }
    }


    public class Root
    {
        public List<ConsentBody> data { get; set; }
        public Meta meta { get; set; }
    }


}
