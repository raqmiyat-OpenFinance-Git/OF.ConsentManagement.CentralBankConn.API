using ConsentMangerModel.Consent;
using ConsentMangerModel.EFModel.Payments;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsentManagerBackendReceiverWorker.Mappers
{
    public  static class CbPaymentLogMapper
    {
        public static GetPaymentLog MapCbGetPaymentLogToEF(CbGetAuditConsentByConsentIdRequestDto request, Logger logger)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request), "Request data is null");

            GetPaymentLog getPaymentLog = new GetPaymentLog();
            try
            {
                getPaymentLog.CorrelationId = request!.CorrelationId;
                getPaymentLog.ConsentId = request?.ConsentId!;
                getPaymentLog.AccountId = request?.AccountId!;
                getPaymentLog.Status = "PENDING";
                getPaymentLog.CreatedBy = "System";
                getPaymentLog.CreatedOn = DateTime.UtcNow;
                getPaymentLog.RequestPayload = JsonConvert.SerializeObject(request);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error occurred in MapCbGetPaymentLogToEF. CorrelationId: {request?.CorrelationId}");
            }
            return getPaymentLog;

        }

        public static PatchPaymentLog MapCbPatchPaymentLogToEF(CbPatchPaymentRecordRequestDto requestData, Logger logger)
        {
            var request = requestData?.paymentRecordBody;
            if (request == null)
                throw new ArgumentNullException(nameof(request), "Request data is null");

            PatchPaymentLog patchPaymentLog = new PatchPaymentLog();
            try
            {
                patchPaymentLog.CorrelationId = requestData!.CorrelationId;
                patchPaymentLog.RequestId = requestData?.Id!;
                patchPaymentLog.PaymentResponseStatus = request?.paymentResponsestatus!;
                patchPaymentLog.NumberOfSuccessfulTransactions = request?.paymentResponseOpenFinanceBilling!.NumberOfSuccessfulTransactions!;
                patchPaymentLog.PaymentResponsePaymentTransactionId = request?.paymentResponsepaymentTransactionId!;
                patchPaymentLog.Status = "PENDING";
                patchPaymentLog.CreatedBy = "System";
                patchPaymentLog.CreatedOn = DateTime.UtcNow;
                patchPaymentLog.RequestPayload = JsonConvert.SerializeObject(requestData);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error occurred in MapCbPatchPaymentLogToEF. CorrelationId: {requestData?.CorrelationId}");
            }
            return patchPaymentLog;

        }


    }
}
