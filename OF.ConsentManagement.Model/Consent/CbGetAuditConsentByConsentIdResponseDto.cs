using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsentMangerModel.Consent
{
    public class CbGetAuditConsentByConsentIdResponseDto
    {
        public Guid CorrelationId { get; set; }
        public string? status { get; set; }
        public GetAuditConsentsByConsentIdResponse? auditConsentsByConsentIdResponse { get; set; }

    }
}
