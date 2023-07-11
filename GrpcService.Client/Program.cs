using Grpc.Core;
using Grpc.Net.Client;
using GrpcService.Protos;


using var channel = GrpcChannel.ForAddress("https://localhost:7206");

//Unary
//var messageFromGreeter = await GetMessage(channel);
//Console.WriteLine($"Server say {messageFromGreeter}");

//Stream from server
//await GetMessageStream(channel);

await TwoWayMessageStream(channel);

Console.ReadLine();


async Task<string> GetMessage(GrpcChannel channel)
{
    var client = new Greeter.GreeterClient(channel);
    var reply = await client.SayHelloAsync(new() { Name = "Denis" });
    return reply.Message;
}

async Task GetMessageStream(GrpcChannel channel)
{
    var client = new Messenger.MessengerClient(channel);
    var serverData = client.ServerDataStream(new MessageRequest());

    var responseStream = serverData.ResponseStream;

    while (await responseStream.MoveNext(CancellationToken.None))
    {
        MessageResponse response = responseStream.Current;
        Console.WriteLine($"Server stream: {response}");
    }
}

async Task PostMessageStream(GrpcChannel channel)
{
    string[] messages = { "Привет", "Как дела?", "Че молчишь?", "Ты че, спишь?", "Ну пока" };

    var client = new ReceiverMessenger.ReceiverMessengerClient(channel);
    var stream = client.ReceiverDataStream();

    foreach (var message in messages)
    {
        await stream.RequestStream.WriteAsync(new ReceiverRequest() { Content = message });
    }

    await stream.RequestStream.CompleteAsync();

    var response = stream.ResponseAsync;
    Console.WriteLine($"Srver response {response}");
}

async Task TwoWayMessageStream(GrpcChannel channel)
{
    string[] messages = { "Привет", "Как дела?", "Че молчишь?", "Ты че, спишь?", "Ну пока" };

    var client = new TwoWay.TwoWayClient(channel);
    var stream = client.TwoWayDataStream();

    var readTask = Task.Run(async () =>
    {
        await foreach (var item in stream.ResponseStream.ReadAllAsync())
        {
            Console.WriteLine(item.Content);
        }
    });

    foreach (var message in messages)
    {
        await stream.RequestStream.WriteAsync(new TwoWayRequest() { Content = message});
        Console.WriteLine(message);
        Task.Delay(2000);
    }

    await stream.RequestStream.CompleteAsync();

    await readTask;
}