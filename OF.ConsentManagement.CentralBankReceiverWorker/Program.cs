
using OF.ConsentManagement.CentralBankReceiverWorker.Consumer;
using OF.ConsentManagement.CentralBankReceiverWorker.IServices;
using OF.ConsentManagement.Common.Custom;
using OF.ConsentManagement.Common.NLog;
using OF.ConsentManagement.Model.Common;

namespace OF.ConsentManagement.CentralBankReceiverWorker.API;

public static class Program
{
    private static readonly Logger _logger = LogManager.GetLogger("OF.ConsentManagement.CentralBankReceiverWorkerLogger");

    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        const string AllowSpecificOrigins = "OF.ConsentManagement.CentralBankReceiverWorker";

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

        ConfigureApplicationSettings(builder.Services, configuration);
        ConfigureAuditLogDbContext(builder.Services);
        ConfigureGetConsentAuditDbContext(builder.Services);
        ConfigureGetConsentDbContext(builder.Services);
        ConfigurePostConsentDbContext(builder.Services);
        ConfigureCbsDbContext(builder.Services);
        RegisterDbConnection(builder.Services);
        RegisterTransientServices(builder.Services);
        RegisterSingletonServices(builder.Services);
        AddMassTransitWithRabbitMq(builder.Services, rabbitMqSettings);

        builder.Services.AddControllers();
        builder.Services.AddHealthChecks();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.UseCors(AllowSpecificOrigins);
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        //app.MapHealthChecks("/health", new HealthCheckOptions
        //{
        //    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        //});

        try
        {
            await app.RunAsync();
        }
        catch (Exception ex)
        {
            _logger.Error("Unhandled exception in Main(): " + ex);
            throw;
        }
    }

    private static void ConfigureApplicationSettings(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RedisCacheSettings>(configuration.GetSection(nameof(RedisCacheSettings)));
        services.Configure<RabbitMqSettings>(configuration.GetSection(nameof(RabbitMqSettings)));
        services.Configure<DatabaseConfig>(configuration.GetSection(nameof(DatabaseConfig)));
        services.Configure<ServiceParams>(configuration.GetSection(nameof(ServiceParams)));
        services.Configure<StoredProcedureConfig>(configuration.GetSection(nameof(StoredProcedureConfig)));
    }
    private static void ConfigureAuditLogDbContext(IServiceCollection services)
    {
        services.AddDbContext<AuditLogDbContext>((provider, options) =>
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
    private static void ConfigureGetConsentAuditDbContext(IServiceCollection services)
    {
        services.AddDbContext<GetConsentAuditDbContext>((provider, options) =>
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
    private static void ConfigureGetConsentDbContext(IServiceCollection services)
    {
        services.AddDbContext<GetConsentDbContext>((provider, options) =>
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
    private static void ConfigurePostConsentDbContext(IServiceCollection services)
    {
        services.AddDbContext<PostConsentDbContext>((provider, options) =>
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
    private static void RegisterTransientServices(IServiceCollection services)
    {
        services.AddTransient<IAuditLogger, AuditLogger>();
        services.AddTransient<IGetConsentAuditService, GetConsentAuditService>();
        services.AddTransient<IGetConsentService, GetConsentService>();
        services.AddTransient<IPatchConsentService, PatchConsentService>();
        services.AddTransient<IPostConsentService, PostConsentService>();
    }

    private static void RegisterSingletonServices(IServiceCollection services)
    {
        services.AddSingleton<ConsentLogger>();
        services.AddSingleton<BaseLogger>();
    }
    public static IServiceCollection AddMassTransitWithRabbitMq(IServiceCollection services, RabbitMqSettings rabbitMqSettings)
    {
        var password = rabbitMqSettings.IsEncrypted
            ? EncryptDecrypt.Decrypt(rabbitMqSettings.Rabitphrase!, _logger)
            : rabbitMqSettings.Rabitphrase!;

        services.AddMassTransit(x =>
        {
            x.AddConsumer<AuditLogConsumer>();
            x.AddConsumer<CbGetConsentAuditRequestConsumer>();
            x.AddConsumer<CbGetConsentAuditResponseConsumer>();
            x.AddConsumer<CbGetConsentRequestConsumer>();
            x.AddConsumer<CbGetConsentResponseConsumer>();
            x.AddConsumer<CbPatchConsentRequestConsumer>();
            x.AddConsumer<CbPatchConsentResponseConsumer>();
            x.AddConsumer<CbPostConsentRequestConsumer>();
            x.AddConsumer<CbPostConsentResponseConsumer>();
            x.AddConsumer<CbsEnquiryConsumer>();
            x.AddConsumer<CbsPostingConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(rabbitMqSettings.Url!), h =>
                {
                    h.Username(rabbitMqSettings.UserName!);
                    h.Password(password);
                    h.Heartbeat(TimeSpan.FromSeconds(5));
                });
                

                cfg.ReceiveEndpoint(rabbitMqSettings.AuditLog!, ep =>
                {
                    ep.ConfigureConsumer<AuditLogConsumer>(context);
                    _logger.Info("AuditLogConsumer-ConfigureConsumer is initialized.");
                });

                cfg.ReceiveEndpoint(rabbitMqSettings.GetConsentAuditRequest!, ep =>
                {
                    ep.ConfigureConsumer<CbGetConsentAuditRequestConsumer>(context);
                    _logger.Info("CbGetConsentAuditRequestConsumer is initialized.");
                });

                cfg.ReceiveEndpoint(rabbitMqSettings.GetConsentAuditResponse!, ep =>
                {
                    ep.ConfigureConsumer<CbGetConsentAuditResponseConsumer>(context);
                    _logger.Info("CbGetConsentAuditResponseConsumer is initialized.");
                });

                cfg.ReceiveEndpoint(rabbitMqSettings.GetConsentRequest!, ep =>
                {
                    ep.ConfigureConsumer<CbGetConsentRequestConsumer>(context);
                    _logger.Info("CbGetConsentRequestConsumer is initialized.");
                });

                cfg.ReceiveEndpoint(rabbitMqSettings.GetConsentResponse!, ep =>
                {
                    ep.ConfigureConsumer<CbGetConsentResponseConsumer>(context);
                    _logger.Info("CbGetConsentResponseConsumer is initialized.");
                });

                cfg.ReceiveEndpoint(rabbitMqSettings.PatchConsentRequest!, ep =>
                {
                    ep.ConfigureConsumer<CbPatchConsentRequestConsumer>(context);
                    _logger.Info("CbPatchConsentRequestConsumer is initialized.");
                });

                cfg.ReceiveEndpoint(rabbitMqSettings.PatchConsentResponse!, ep =>
                {
                    ep.ConfigureConsumer<CbPatchConsentResponseConsumer>(context);
                    _logger.Info("CbPatchConsentResponseConsumer is initialized.");
                });

                cfg.ReceiveEndpoint(rabbitMqSettings.PostConsentRequest!, ep =>
                {
                    ep.ConfigureConsumer<CbPostConsentRequestConsumer>(context);
                    _logger.Info("CbPostConsentRequestConsumer is initialized.");
                });

                cfg.ReceiveEndpoint(rabbitMqSettings.PostConsentResponse!, ep =>
                {
                    ep.ConfigureConsumer<CbPostConsentResponseConsumer>(context);
                    _logger.Info("CbPostConsentResponseConsumer is initialized.");
                });

                cfg.ReceiveEndpoint(rabbitMqSettings.CbsEnquiryRequest!, ep =>
                {
                    ep.ConfigureConsumer<CbsEnquiryConsumer>(context);
                    _logger.Info("CbsEnquiryConsumer is initialized.");
                });

                cfg.ReceiveEndpoint(rabbitMqSettings.CbsPostingRequest!, ep =>
                {
                    ep.ConfigureConsumer<CbsPostingConsumer>(context);
                    _logger.Info("CbsPostingConsumer is initialized.");
                });


                cfg.ConfigureEndpoints(context);
            });
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
