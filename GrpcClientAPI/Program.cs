using GrpcServerAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add gRPC client
builder.Services.AddGrpcClient<ServerService.ServerServiceClient>(options =>
{
    options.Address = new Uri("https://localhost:5001"); // gRPC server URL
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
