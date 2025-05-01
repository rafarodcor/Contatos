using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Polly;

namespace Contatos.API.Configurations;

public static class Configuration
{
    #region Methods

    public static void ConfigureOpenTelemetry(WebApplicationBuilder builder)
    {
        builder.Services.AddOpenTelemetry()
            .WithMetrics(opt =>
                opt
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("OpenRemoteManage.ContatosAPI"))
                    .AddMeter(builder.Configuration.GetValue<string>("OpenRemoteManageMeterName"))
                    .AddAspNetCoreInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddProcessInstrumentation()
                    .AddOtlpExporter(opts =>
                    {
                        opts.Endpoint = new Uri(builder.Configuration["Otel:Endpoint"]);
                    })
            )
            .WithTracing(opt =>
                opt
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("OpenRemoteManage.ContatosConsumer"))
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddOtlpExporter(opts =>
                    {
                        opts.Endpoint = new Uri(builder.Configuration["Otel:Endpoint"]);
                    }));//TESTAR
    }

    public static IAsyncPolicy ConfigureCircuitBreakerPolicy(WebApplicationBuilder builder)
    {
        return Policy
            .Handle<HttpRequestException>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: int.Parse(builder.Configuration["PolicyParameters:ExceptionsAllowedBeforeBreaking"]),
                durationOfBreak: TimeSpan.FromSeconds(int.Parse(builder.Configuration["PolicyParameters:CircuitBreakerDuration"])),
                onBreak: (exception, timespan) =>
                {
                    Console.WriteLine($"Circuito aberto por {timespan.TotalSeconds} segundos devido a: {exception.Message}");
                },
                onReset: () =>
                {
                    Console.WriteLine("Circuito fechado. Operações retomadas.");
                },
                onHalfOpen: () =>
                {
                    Console.WriteLine("Circuito em estado meio-aberto. Testando novamente...");
                });
    }

    public static IAsyncPolicy ConfigureBulkheadPolicy(WebApplicationBuilder builder)
    {
        return Policy
            .BulkheadAsync(
                maxParallelization: int.Parse(builder.Configuration["PolicyParameters:MaxParallelization"]),
                maxQueuingActions: int.Parse(builder.Configuration["PolicyParameters:MaxQueuingActions"]),
                onBulkheadRejectedAsync: async context =>
                {
                    Console.WriteLine("Requisição rejeitada pelo Bulkhead - o serviço está sobrecarregado.");
                });
    }

    public static IAsyncPolicy ConfigureRetryPolicy(WebApplicationBuilder builder)
    {
        return Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(
                retryCount: int.Parse(builder.Configuration["PolicyParameters:RetryCount"]),
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(int.Parse(builder.Configuration["PolicyParameters:SleepDurationProvider"]), retryAttempt)),
                onRetry: (exception, timeSpan, retryCount, context) =>
                {
                    Console.WriteLine($"Tentativa {retryCount} falhou. Tentando novamente em {timeSpan.Seconds} segundos.");
                });
    }

    #endregion
}