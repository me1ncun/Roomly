using MassTransit;

namespace Roomly.Rooms.Infrastructure.Rabbit;

public class BookingConsumer: IConsumer<OrderCreated>
{
    public async Task Consume(ConsumeContext<OrderCreated> context)
    {
        var jsonMessage = JsonConvert.SerializeObject(context.Message);
        Console.WriteLine($"OrderCreated message: {jsonMessage}");
    }
}