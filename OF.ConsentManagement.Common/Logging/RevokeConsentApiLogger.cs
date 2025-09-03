using ConsentManagerCommon.NLog;
using OF.ConsentManagement.Common.NLog;

namespace ConsentManagerCommon.Logging
{
    public class RevokeConsentApiLogger:BaseLogger
    {
        public RevokeConsentApiLogger(IConfiguration configuration) : base(configuration)
        {
            bool siemEnabled = configuration.GetValue<bool>("SIEM-Ready-Log");
            if (siemEnabled)
            {
                LogManager.Setup().LoadConfigurationFromFile("NLog.config");
                Log = LogManager.GetLogger("PaymentsApiJsonLogger");
            }
            else
            {
                Log = LogManager.GetLogger("PaymentsApiLogger");
            }
        }
    }
}
