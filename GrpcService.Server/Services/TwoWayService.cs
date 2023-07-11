using Grpc.Core;
using GrpcService.Protos;

namespace GrpcService.Server.Services
{
    public class TwoWayService : TwoWay.TwoWayBase
    {
        private string[] messages = { "Привет", "Норм", "...", "Нет", "пока" };
        
        public override async Task TwoWayDataStream
            (IAsyncStreamReader<TwoWayRequest> requestStream, 
            IServerStreamWriter<TwoWayResponse> responseStream,
            ServerCallContext context)
        {
            var readTask = Task.Run(async () =>
            {
                await foreach (var message in requestStream.ReadAllAsync())
                {
                    Console.WriteLine(message.Content);
                }
            });

            foreach (var message in messages) 
            {
                if(!readTask.IsCompleted)
                {
                    await responseStream.WriteAsync(new TwoWayResponse() { Content = message });
                    Console.WriteLine(message);
                }
                await Task.Delay(2000);
            }

            await readTask;
        }
    }
}
