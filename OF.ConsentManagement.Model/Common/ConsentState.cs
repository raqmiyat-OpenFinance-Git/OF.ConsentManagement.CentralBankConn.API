namespace OF.ConsentManagement.Model.Common;

public enum ConsentState
{
    Created = 0,            // Consent created, not yet acted upon
    AwaitingAuthorisation,  // Pending customer authorisation
    Authorised,             // Authorised by customer, ready to use
    Active,                 // In use, valid for operations
    Rejected,               // Customer explicitly rejected
    Revoked,                // Revoked by customer or bank
    Expired,                // Expired by validity period
    Terminated              // Terminated by bank/system (fraud, regulation, etc.)
}
