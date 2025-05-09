using Contatos.Dados.Repositories;
using Contatos.Modelos.Modelos;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Contatos.Consumer.Include.Services.Consumers;

public class IncluirContatoConsumer : BackgroundService
{
    #region Constants

    private const string QUEUE_NAME = "INCLUIR_CONTATO";

    #endregion  

    #region Properties

    private readonly RabbitMQConnectionManager _connectionManager;    
    private readonly IModel _channel;
    private readonly IServiceProvider _serviceProvider;    
    private readonly ILogger<IncluirContatoConsumer> _logger;   

    #endregion

    #region Methods

    public IncluirContatoConsumer(
        RabbitMQConnectionManager connectionManager,
        IServiceProvider servicesProvider,         
        ILogger<IncluirContatoConsumer> logger)
    {
        _connectionManager = connectionManager;
        _serviceProvider = servicesProvider;
        _channel = _connectionManager.GetChannel(QUEUE_NAME);
        _logger = logger;        
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Consumer > ExecuteAsync > Contato > {QUEUE_NAME} > ExecuteAsync");

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (sender, eventArgs) =>
        {
            var modelBytes = eventArgs.Body.ToArray();
            var modelJson = Encoding.UTF8.GetString(modelBytes);
            var model = JsonSerializer.Deserialize<Contato>(modelJson);

            await PostAsync(model);

            _channel.BasicAck(eventArgs.DeliveryTag, false);
        };

        _channel.BasicConsume(QUEUE_NAME, false, consumer);
        return Task.CompletedTask;
    }

    public async Task PostAsync(Contato model)
    {
        _logger.LogInformation($"Consumer > ExecuteAsync > Contato > {QUEUE_NAME} > PostAsync");

        using (var scope = _serviceProvider.CreateScope())
        {
            var contatoRepository = scope.ServiceProvider.GetRequiredService<IContatoRepository>();
            await contatoRepository.IncluirContatoAsync(model);

            _logger.LogInformation($"Consumer > ExecuteAsync > Contato > {QUEUE_NAME} > IncluirContatoAsync");
        }
    }

    #endregion
}