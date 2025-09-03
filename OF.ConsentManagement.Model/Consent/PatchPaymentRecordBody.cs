using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsentMangerModel.Consent
{
    public class PatchPaymentRecordBody
    {
        public string? paymentResponsestatus { get; set; }

        public PaymentResponseOpenFinanceBilling? paymentResponseOpenFinanceBilling { get; set; }

        public string? paymentResponsepaymentTransactionId { get; set; }
    }
    public class PaymentResponseOpenFinanceBilling
    {
        public int NumberOfSuccessfulTransactions { get; set; }
    }
}
