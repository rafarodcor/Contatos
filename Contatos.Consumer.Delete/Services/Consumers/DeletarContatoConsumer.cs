using Contatos.Dados.Repositories;
using Contatos.Modelos.Modelos;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Contatos.Consumer.Delete.Services.Consumers;

public class DeletarContatoConsumer : BackgroundService
{
    #region Constants

    private const string QUEUE_NAME = "DELETAR_CONTATO";

    #endregion

    #region Properties

    private readonly RabbitMQConnectionManager _connectionManager;
    private readonly IModel _channel;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DeletarContatoConsumer> _logger;

    #endregion

    #region Methods

    public DeletarContatoConsumer(
        RabbitMQConnectionManager connectionManager,
        IServiceProvider servicesProvider,
        ILogger<DeletarContatoConsumer> logger)
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

            await DeleteAsync(model);

            _channel.BasicAck(eventArgs.DeliveryTag, false);
        };

        _channel.BasicConsume(QUEUE_NAME, false, consumer);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Contato model)
    {
        _logger.LogInformation($"Consumer > ExecuteAsync > Contato > {QUEUE_NAME} > DeleteAsync");

        using (var scope = _serviceProvider.CreateScope())
        {
            var contatoRepository = scope.ServiceProvider.GetRequiredService<IContatoRepository>();
            await contatoRepository.DeletarContatoAsync(model);

            _logger.LogInformation($"Consumer > ExecuteAsync > Contato > {QUEUE_NAME} > DeletarContatoConsumer");
        }
    }

    #endregion
}