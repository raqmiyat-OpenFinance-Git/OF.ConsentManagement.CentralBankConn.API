namespace OF.ConsentManagement.Common.NLog;

public class ExceptionMiddlewareLogger : BaseLogger
{
    public ExceptionMiddlewareLogger(IConfiguration configuration) : base(configuration)
    {
        bool siemEnabled = configuration.GetValue<bool>("SIEM-Ready-Log");
        if (siemEnabled)
        {
            LogManager.Setup().LoadConfigurationFromFile("NLog.config");
            Log = LogManager.GetLogger("ExceptionMiddlewareJsonLogger");
        }
        else
        {
            Log = LogManager.GetLogger("ExceptionMiddlewareLogger");
        }
    }

}

