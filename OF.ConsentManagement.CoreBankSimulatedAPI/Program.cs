using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using OF.ConsentManagement.Common.NLog;

namespace OF.ServiceInitiation.CoreBankConn.API;

public static class Program
{

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var AllowSpecificOrigins = "OF.ServiceInitiation.Model.CoreBankSimulatedAPI";
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: AllowSpecificOrigins, policy =>
            {
                policy.AllowAnyOrigin().AllowAnyHeader();
            });
        });

       
        RegisterSingletonServices(builder.Services);

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
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.Run();
    }
   
    private static void RegisterSingletonServices(IServiceCollection services)
    {
        services.AddSingleton<ConsentLogger>();
    }
}
