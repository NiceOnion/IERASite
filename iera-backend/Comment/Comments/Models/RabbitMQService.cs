using Comments.Models;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

public class RabbitMQConsumer
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IServiceProvider _serviceProvider;

    public RabbitMQConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        var factory = new ConnectionFactory
        {
            Uri = new Uri("amqps://yrevqyvz:YUfdzQrRQM3E17DYXvwQLphc1137tjcN@rat.rmq2.cloudamqp.com/yrevqyvz")
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        // Declare the exchange and bind a queue to it
        _channel.ExchangeDeclare(exchange: "user.delete", type: ExchangeType.Fanout);

        var queueName = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(queue: queueName,
                           exchange: "user.delete",
                           routingKey: "");

        _channel.BasicQos(0, 1, false); // Fair dispatch

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (sender, e) =>
        {
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            try
            {
                await ProcessUserDeleteEvent(message);
                _channel.BasicAck(e.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                // Log the exception and decide on requeuing
                Console.WriteLine($"Error processing message: {ex.Message}");
                // Optionally, you can requeue the message by not acknowledging it
                // _channel.BasicNack(e.DeliveryTag, false, true);
            }
        };

        _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
    }

    private async Task ProcessUserDeleteEvent(string userId)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var commentsRepository = scope.ServiceProvider.GetRequiredService<ICommentRepository>();

            // Get all announcements related to the user
            var Comments = await commentsRepository.GetAllCommentsFromUser(userId);

            // Update announcements to mark user deleted
            foreach (var announcement in Comments)
            {
                announcement.UserId = "UserDeleted"; // Or set to a specific value indicating user deletion
                await commentsRepository.UpdateComment(announcement);
            }

            Console.WriteLine($"Processed user delete event for announcements. User ID: {userId}");
        }
    }

    public void Close()
    {
        _connection.Close();
    }
}

public class RabbitMQPublisher
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMQPublisher()
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri("amqps://yrevqyvz:YUfdzQrRQM3E17DYXvwQLphc1137tjcN@rat.rmq2.cloudamqp.com/yrevqyvz")
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: "comment.added",
                              durable: true,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);
    }

    public void SendCommentAddedMessage(string announcementId)
    {
        var body = Encoding.UTF8.GetBytes(announcementId);

        _channel.BasicPublish(exchange: "",
                              routingKey: "comment.added",
                              basicProperties: null,
                              body: body);

        Console.WriteLine($"Sent comment added message to RabbitMQ: {announcementId}");
    }

    public void Close()
    {
        _connection.Close();
    }
}