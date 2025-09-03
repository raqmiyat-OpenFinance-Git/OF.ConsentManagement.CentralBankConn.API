using OF.ConsentManagement.Common.NLog;

namespace ConsentManagerCommon.NLog;

public class PaymentsApiLogger : BaseLogger
{
    public PaymentsApiLogger(IConfiguration configuration) : base(configuration)
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

