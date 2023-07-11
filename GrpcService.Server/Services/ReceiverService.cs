using Grpc.Core;
using GrpcService.Protos;

namespace GrpcService.Server.Services
{
    public class ReceiverService : ReceiverMessenger.ReceiverMessengerBase
    {
        public override async Task<ReceiverResponse> ReceiverDataStream
            (IAsyncStreamReader<ReceiverRequest> requestStream, 
            ServerCallContext context)
        {
            await foreach (var request in requestStream.ReadAllAsync()) 
            {
                Console.WriteLine(request.Content);
            }

            Console.WriteLine("Все данные успешно получены");
            return new ReceiverResponse() { Content = "Получены данные"};
        }
    }
}
