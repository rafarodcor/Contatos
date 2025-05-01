using Contatos.Consumer.Services.Consumers;
using Contatos.Dados.Repositories;
using Contatos.Modelos.Modelos;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Contatos.Consumer.API.Services.Consumers;

public class AtualizarContatoConsumer : BackgroundService
{
    #region Constants

    private const string QUEUE_NAME = "ATUALIZAR_CONTATO";

    #endregion

    #region Properties

    private readonly RabbitMQConnectionManager _connectionManager;
    private readonly IModel _channel;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<AtualizarContatoConsumer> _logger;

    #endregion

    #region Methods

    public AtualizarContatoConsumer(
        RabbitMQConnectionManager connectionManager,
        IServiceProvider servicesProvider,
        ILogger<AtualizarContatoConsumer> logger)
    {
        _connectionManager = connectionManager;
        _serviceProvider = servicesProvider;
        _channel = _connectionManager.GetChannel(QUEUE_NAME);
        _logger = logger;

        /*_configuration = configuration;

        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQConnection:Host"],
            UserName = _configuration["RabbitMQConnection:Username"],
            Password = _configuration["RabbitMQConnection:Password"]
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(
            queue: QUEUE_NAME,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);*/
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

            await PutAsync(model);

            _channel.BasicAck(eventArgs.DeliveryTag, false);
        };

        _channel.BasicConsume(QUEUE_NAME, false, consumer);
        return Task.CompletedTask;
    }

    public async Task PutAsync(Contato model)
    {
        _logger.LogInformation($"Consumer > ExecuteAsync > Contato > {QUEUE_NAME} > PutAsync");

        using (var scope = _serviceProvider.CreateScope())
        {
            var contatoRepository = scope.ServiceProvider.GetRequiredService<IContatoRepository>();
            await contatoRepository.AtualizarContatoAsync(model);

            _logger.LogInformation($"Consumer > ExecuteAsync > Contato > {QUEUE_NAME} > AtualizarContatoAsync");
        }
    }

    #endregion
}