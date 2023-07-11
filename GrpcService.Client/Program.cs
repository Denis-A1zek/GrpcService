using Grpc.Net.Client;
using GrpcService.Protos;


using var channel = GrpcChannel.ForAddress("https://localhost:7206");

//Unary
//var messageFromGreeter = await GetMessage(channel);
//Console.WriteLine($"Server say {messageFromGreeter}");

//Stream from server
await GetMessageStream(channel);

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