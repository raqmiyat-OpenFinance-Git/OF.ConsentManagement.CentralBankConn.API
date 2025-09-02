using HealthChecks.UI.Client;
using MassTransit;
using OF.ConsentManagement.CentralBankConn.API.Model;
using OF.ConsentManagement.Common.AES;
using OF.ConsentManagement.Common.Custom;
using OF.ConsentManagement.Common.NLog;
using OF.ConsentManagement.CoreBankConn.API.IServices;
using OF.ConsentManagement.CoreBankConn.API.Repositories;
using OF.ConsentManagement.Model.Common;

namespace OF.ConsentEvent.CoreBankConn.API;

public static class Program
{
    private static readonly Logger _logger = LogManager.GetLogger("OF.ConsentManagement.CoreBankConn.APILogger");

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var AllowSpecificOrigins = "OF.ConsentManagement.CoreBankConn.API";
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: AllowSpecificOrigins, policy =>
            {
                policy.AllowAnyOrigin().AllowAnyHeader();
            });
        });

        var configuration = builder.Configuration;
        var rabbitMqSettings = configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();

        if (rabbitMqSettings == null)
            throw new InvalidOperationException("RabbitMqSettings section is missing in configuration.");

        ConfigureApplicationSettings(builder.Services, builder.Configuration);
        RegisterDbConnection(builder.Services);
        RegisterTransientServices(builder.Services);
        RegisterSingletonServices(builder.Services);
        RegisterConsentManagementServiceHttpClient(builder.Services);
        AddMassTransitWithRabbitMq(builder.Services, rabbitMqSettings);
        GetSingletonSendPointInitialize(builder.Services, rabbitMqSettings);
        ConfigureCbsDbContext(builder.Services);
        ConfigureCertificateValidation(builder.Services);

        builder.Services.AddControllers();
        builder.Services.AddHealthChecks();
        builder.Services.AddEndpointsApiExplorer();
        //builder.Services.AddSwaggerGen();
        builder.Services.AddSwaggerGen(c =>
        {
            c.CustomSchemaIds(type => type.FullName);
            // or
            // c.CustomSchemaIds(type => type.FullName.Replace(".", "_"));
        });

        var app = builder.Build();

        app.UseCors(AllowSpecificOrigins);
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.Run();
    }

    private static void ConfigureApplicationSettings(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SocketsHttpHandlerSettings>(configuration.GetSection(nameof(SocketsHttpHandlerSettings)));
        services.Configure<SecurityParameters>(configuration.GetSection(nameof(SecurityParameters)));
        services.Configure<DatabaseConfig>(configuration.GetSection(nameof(DatabaseConfig)));
        services.Configure<ServiceParams>(configuration.GetSection(nameof(ServiceParams)));
        services.Configure<CoreBankApis>(configuration.GetSection(nameof(CoreBankApis)));
    }

    private static void ConfigureCbsDbContext(IServiceCollection services)
    {
        services.AddDbContext<CbsDbContext>((provider, options) =>
        {
            var config = provider.GetRequiredService<IOptions<DatabaseConfig>>().Value;

            var connectionString = SqlConManager.GetConnectionString(
                config.ConnectionString!,
                config.UseEncryption,
                config.IsEntityFramework,
                _logger
            );

            options.UseSqlServer(connectionString);
        });
    }

    private static void RegisterDbConnection(IServiceCollection services)
    {
        services.AddScoped<IDbConnection>(provider =>
        {
            var config = provider.GetRequiredService<IOptions<DatabaseConfig>>().Value;

            var connectionString = SqlConManager.GetConnectionString(
                config.ConnectionString!,
                config.UseEncryption,
                config.IsEntityFramework,
                _logger
            );

            var dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();

            _logger.Info(ForStructuredLog("Program", "RegisterDbConnection", "DBConnection opened"));
            return dbConnection;
        });
    }

    private static void RegisterConsentManagementServiceHttpClient(IServiceCollection services)
    {
        services.AddHttpClient<IConsentManagementService, OF.ConsentManagement.CoreBankConn.API.Services.ConsentManagementService>((provider, client) =>
        {
            var handlerSettings = provider.GetRequiredService<IOptions<SocketsHttpHandlerSettings>>().Value;
            var coreBankApis = provider.GetRequiredService<IOptions<CoreBankApis>>().Value;

            if (string.IsNullOrWhiteSpace(coreBankApis.BaseUrl))
                throw new InvalidOperationException("CoreBankApis.BaseUrl must be configured.");

            client.DefaultRequestHeaders.Connection.Add("keep-alive");
            client.DefaultRequestHeaders.ConnectionClose = false;
            client.BaseAddress = new Uri(coreBankApis.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(handlerSettings.ClientTimeoutInSeconds);
        })
        .SetHandlerLifetime(TimeSpan.FromHours(2))
        .ConfigurePrimaryHttpMessageHandler(provider =>
        {
            var settings = provider.GetRequiredService<IOptions<SocketsHttpHandlerSettings>>().Value;

            return new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromHours(settings.PooledConnectionLifetimeInHours),
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(settings.PooledConnectionIdleTimeoutInMinutes),
                ConnectTimeout = TimeSpan.FromSeconds(settings.ClientTimeoutInSeconds),
                KeepAlivePingDelay = TimeSpan.FromMinutes(settings.KeepAlivePingDelayInMinutes),
                KeepAlivePingTimeout = TimeSpan.FromSeconds(settings.KeepAlivePingTimeoutInSeconds),
                UseCookies = settings.UseCookies,
                SslOptions = new SslClientAuthenticationOptions
                {
                    EnabledSslProtocols = SslClient.ParseSslProtocols(settings.EnabledSslProtocols ?? "Tls12"),
                    CertificateRevocationCheckMode = SslClient.ParseRevocationMode(settings.CertificateRevocationCheckMode ?? "NoCheck"),
                    RemoteCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
                        sslPolicyErrors == SslPolicyErrors.None || settings.RemoteCertificateValidationCallback
                }
            };
        });
    }

    private static void ConfigureCertificateValidation(IServiceCollection services)
    {
        using var provider = services.BuildServiceProvider();
        var settings = provider.GetRequiredService<IOptions<SocketsHttpHandlerSettings>>().Value;

        ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
        {
            return sslPolicyErrors == SslPolicyErrors.None || settings.RemoteCertificateValidationCallback;
        };
    }

    private static void RegisterTransientServices(IServiceCollection services)
    {
        services.AddTransient<IConsentManagementService, OF.ConsentManagement.CoreBankConn.API.Services.ConsentManagementService>();
        services.AddTransient<ICbsRepository, CbsRepository>();
    }

    private static void RegisterSingletonServices(IServiceCollection services)
    {
        services.AddSingleton<ConsentLogger>();
        services.AddSingleton<BaseLogger>();
        services.AddSingleton<AesCbcGenericService>();
        services.AddSingleton<SendPointInitialize>();
    }
    public static IServiceCollection AddMassTransitWithRabbitMq(IServiceCollection services, RabbitMqSettings rabbitMqSettings)
    {
        var password = rabbitMqSettings.IsEncrypted
            ? EncryptDecrypt.Decrypt(rabbitMqSettings.Rabitphrase!, _logger)
            : rabbitMqSettings.Rabitphrase!;

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(rabbitMqSettings.Url!), h =>
                {
                    h.Username(rabbitMqSettings.UserName!);
                    h.Password(password);
                });

                // cfg.ConfigureEndpoints(context); // If you have consumers
            });
        });

        return services;
    }
    public static IServiceCollection GetSingletonSendPointInitialize(IServiceCollection services, RabbitMqSettings rabbitMqSettings)
    {
        services.AddSingleton<SendPointInitialize>(provider =>
        {
            var sendPointInitialize = new SendPointInitialize();
            var bus = provider.GetRequiredService<IBus>();

            var sendEndpoint = bus.GetSendEndpoint(new Uri("queue:" + rabbitMqSettings!.CbsEnquiryRequest));
            sendPointInitialize.CbsEnquiryRequest = sendEndpoint.Result;

            sendEndpoint = bus.GetSendEndpoint(new Uri("queue:" + rabbitMqSettings!.CbsPostingRequest));
            sendPointInitialize.CbsPostingRequest = sendEndpoint.Result;

            return sendPointInitialize;

        });
        return services;
    }
    private static string ForStructuredLog(string controllerName, string methodName, string message)
    {
        return
$@"---------------------------------------------------------------
 Class Name   : {controllerName}
 Method Name  : {methodName}
 Message      : {message}
---------------------------------------------------------------";
    }
}
