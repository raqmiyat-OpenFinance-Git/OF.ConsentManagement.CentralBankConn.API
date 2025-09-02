using OF.ConsentManagement.Model.CentralBank.Consent.PostRequestDto;
using OF.ConsentManagement.Model.EFModel;

namespace CentralBankReceiverWorker.Mappers;
public static class CbPatchConsentMapper
{
    public static ConsentResponse MapCbPatchConsentRequestToEF(CbPatchConsentRequestDto requestDto)
    {
        if (requestDto == null)
        {
            throw new ArgumentNullException(nameof(requestDto));
        }

        var request = requestDto.cbPatchConsentRequest
                       ?? throw new ArgumentNullException(nameof(requestDto.cbPatchConsentRequest));


        var consentResponse = new ConsentResponse
        {

            PsuUserId = request.PsuIdentifiers.UserId,
            AccountIds = request.AccountIds.First().ToString(),
            InsurancePolicyIds =request.InsurancePolicyIds.First().ToString(),
            SupplementaryInformation = request.SupplementaryInformation.ToString(),
            ConnectToken = request.ConnectToken,
            LastDataShared = request.ConsentUsage.LastDataShared,
            LastServiceInitiationAttempt = request.ConsentUsage.LastServiceInitiationAttempt,
            AuthorizationChannel = request.AuthorizationChannel,

            UnitCurrency= request.ConsentBody.Data.ExchangeRate.UnitCurrency,
            ExchangeRate = request.ConsentBody.Data.ExchangeRate.ExchangeRateValue,
            RateType = request.ConsentBody.Data.ExchangeRate.RateType,
            ContractIdentification = request.ConsentBody.Data.ExchangeRate.ContractIdentification,
            ExpirationDateTime = request.ConsentBody.Data.ExchangeRate.ExpirationDateTime,

            ChargeBearer = request.ConsentBody.Data.Charges.FirstOrDefault()!.ChargeBearer,
            ChargeBearerType = request.ConsentBody.Data.Charges.FirstOrDefault()!.Type,
            ChargeBearerAmount = Convert.ToDecimal(request.ConsentBody.Data.Charges.FirstOrDefault()!.Amount.Amount),
            ChargeBearerCurrency = request.ConsentBody.Data.Charges.FirstOrDefault()!.Amount.Currency,

            ResponseUpdatePayload = JsonConvert.SerializeObject(request)
        };

        return consentResponse;
    }

}
