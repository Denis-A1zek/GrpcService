using GrpcService.Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<GreeterService>();
app.MapGrpcService<MessengerService>();
app.MapGrpcService<ReceiverService>();
app.MapGrpcService<TwoWayService>();

app.MapGet("/", () => "Hello World!");

app.Run();
