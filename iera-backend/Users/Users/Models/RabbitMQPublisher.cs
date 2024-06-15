using RabbitMQ.Client;
using System.Text;

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

        // Declare an exchange
        _channel.ExchangeDeclare(exchange: "user.delete", type: ExchangeType.Fanout);
    }

    public void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);

        // Publish message to the exchange
        _channel.BasicPublish(exchange: "user.delete",
                              routingKey: "",
                              basicProperties: null,
                              body: body);

        Console.WriteLine($"Sent message to RabbitMQ: {message}");
    }

    public void Close()
    {
        _connection.Close();
    }
}
