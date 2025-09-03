namespace OF.ConsentManagement.Common.NLog;

public class ResourceLogApiLogger : BaseLogger
{
    public ResourceLogApiLogger(IConfiguration configuration) : base(configuration)
    {
        bool siemEnabled = configuration.GetValue<bool>("SIEM-Ready-Log");

        if (siemEnabled)
        {
            LogManager.Setup().LoadConfigurationFromFile("NLog.config");
            Log = LogManager.GetLogger("ConsentLoggerJson");
        }
        else
        {
            Log = LogManager.GetLogger("CconsentLogger");
        }
    }
}



