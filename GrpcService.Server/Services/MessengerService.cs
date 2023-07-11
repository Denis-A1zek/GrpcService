using Grpc.Core;
using GrpcService.Protos;

namespace GrpcService.Server.Services;

public class MessengerService : Messenger.MessengerBase
{
    string[] messages = { "Привет", "Как дела?", "Че молчишь?", "Ты че, спишь?", "Ну пока" };

    public override async Task ServerDataStream
        (MessageRequest request, 
        IServerStreamWriter<MessageResponse> responseStream, 
        ServerCallContext context)
    {
        foreach (var message in messages)
        {
            await responseStream.WriteAsync(new MessageResponse { Content = message });
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
}
