using Announcements.Data;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

public class RabbitMQUserConsumer
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IServiceProvider _serviceProvider;

    public RabbitMQUserConsumer(IServiceProvider serviceProvider)
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
            var announcementRepository = scope.ServiceProvider.GetRequiredService<IAnnouncementRepository>();

            // Get all announcements related to the user
            var announcements = await announcementRepository.GetAllAnnouncementsByUser(userId);

            // Update announcements to mark user deleted
            foreach (var announcement in announcements)
            {
                announcement.UserID = "UserDeleted"; // Or set to a specific value indicating user deletion
                await announcementRepository.UpdateAnnouncement(announcement);
            }

            Console.WriteLine($"Processed user delete event for announcements. User ID: {userId}");
        }
    }

    public void Close()
    {
        _connection.Close();
    }
}

public class RabbitMQCommentConsumer
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IServiceProvider _serviceProvider;

    public RabbitMQCommentConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

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

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (sender, e) =>
        {
            var body = e.Body.ToArray();
            var announcementId = Encoding.UTF8.GetString(body);
            try
            {
                await UpdateCommentCount(announcementId);
                _channel.BasicAck(e.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
                // Optionally, you can requeue the message by not acknowledging it
                // _channel.BasicNack(e.DeliveryTag, false, true);
            }
        };

        _channel.BasicConsume(queue: "comment.added", autoAck: false, consumer: consumer);
    }

    private async Task UpdateCommentCount(string announcementId)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var announcementRepository = scope.ServiceProvider.GetRequiredService<IAnnouncementRepository>();

            // Increment the comment count of the announcement
            var announcement = await announcementRepository.GetAnnouncement(announcementId);
            if (announcement != null)
            {
                announcement.CommentCount += 1;
                await announcementRepository.UpdateAnnouncement(announcement);
                Console.WriteLine($"Updated comment count for announcement ID: {announcementId}");
            }
        }
    }

    public void Close()
    {
        _connection.Close();
    }
}