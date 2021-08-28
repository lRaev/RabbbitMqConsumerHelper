using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace rmq
{
    class Program
    {
        static void Main(string[] args)
        {
            string _queueName = "AggregationUpdateSend";
            var factory = new ConnectionFactory() { HostName = "localhost" };
        using(var connection = factory.CreateConnection())
        using(var _model = connection.CreateModel())
        {
            _model.QueueDeclare(_queueName, durable: true, autoDelete: false);
            _model.ExchangeDeclare("AggregationUpdate", ExchangeType.Direct
            , durable: true, autoDelete: false);
            _model.QueueBind(_queueName, "AggregationUpdate", string.Empty);
            // channel.QueueDeclare(queue: "AggregationUpdateSend", 
            //                      durable: false,
                                 
            //                      exclusive: false,
            //                      autoDelete: false,
            //                      arguments: null);

            var consumer = new EventingBasicConsumer(_model);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
               
                Console.WriteLine(" [x] Message_type: {0}, Received @ {1}:\n {2}", _queueName, DateTime.UtcNow ,message);
            };
            _model.BasicConsume(queue: _queueName,
                                
                                 autoAck: false,
                                 consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
}
