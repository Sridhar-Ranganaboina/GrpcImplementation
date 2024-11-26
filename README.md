# Grpc Server and Client Example

This project demonstrates how to create a solution with multiple ASP.NET Core projects, including a gRPC server and a client. The solution includes:

- **GrpcServerAPI**: An ASP.NET Core Web API that serves as the gRPC server.
- **GrpcClientAPI**: An ASP.NET Core Web API that acts as a client to call the gRPC server.
- **GrpcServerBase**: A class library containing the `.proto` files and generated gRPC classes.

## Prerequisites

Ensure that you have the following software installed:

- [.NET SDK](https://dotnet.microsoft.com/download/dotnet) (Recommended: .NET 8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) with the **ASP.NET and web development** workload.
- [Protobuf Compiler (protoc)](https://grpc.io/docs/protoc-installation/) if necessary.

---

## Setting Up the Project

### 1. Clone the Repository

If you haven't already, clone the repository to your local machine:
```bash
git clone https://github.com/your-repository-url.git

** 2. Install NuGet Packages **
In each project, install the necessary NuGet packages:

GrpcServerAPI
Grpc.AspNetCore
Google.Protobuf
Grpc.Tools
GrpcClientAPI
Grpc.Net.Client

Run the following commands in the terminal for each project:

dotnet add package Grpc.AspNetCore
dotnet add package Google.Protobuf
dotnet add package Grpc.Tools
dotnet add package Grpc.Net.Client


** Configure the gRPC Server (GrpcServerAPI) **
1. Define the gRPC Service
In the GrpcServerBase project, create your .proto file(s). For example, create a file named ServerService.proto with the following content:

** ServerService.proto **

syntax = "proto3";

option csharp_namespace = "GrpcServerAPI";

service ServerService {
    // Get all values
    rpc GetAll (Empty) returns (GetAllResponse);

    // Get a single value by ID
    rpc GetById (GetByIdRequest) returns (GetByIdResponse);

    // Create a new value
    rpc Create (CreateRequest) returns (Empty);

    // Update a value by ID
    rpc Update (UpdateRequest) returns (Empty);

    // Delete a value by ID
    rpc Delete (DeleteRequest) returns (Empty);
}

// Message for an empty request/response
message Empty {}

// Response for GetAll
message GetAllResponse {
    repeated string values = 1;
}

// Request for GetById
message GetByIdRequest {
    int32 id = 1;
}

// Response for GetById
message GetByIdResponse {
    string value = 1;
}

// Request for Create
message CreateRequest {
    string value = 1;
}

// Request for Update
message UpdateRequest {
    int32 id = 1;
    string value = 2;
}

// Request for Delete
message DeleteRequest {
    int32 id = 1;
}


** Implement the gRPC Service **

In the GrpcServerAPI project, implement the service by creating a class ServerServiceImpl:

public class ServerServiceImpl : ServerService.ServerServiceBase
{
    public override Task<GetByIdResponse> GetById(GetByIdRequest request, ServerCallContext context)
    {
        var response = new GetByIdResponse
        {
            Value = "Some value based on ID " + request.Id
        };
        return Task.FromResult(response);
    }
}

** Configure gRPC in Program.cs ** 
In GrpcServerAPI, update Program.cs to register and map gRPC services:

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();  // Register gRPC services
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<ServerServiceImpl>();  // Map gRPC service

app.Run();

** Configure the gRPC Client (GrpcClientAPI) **

1. Add gRPC Client Dependency
In GrpcClientAPI, update Program.cs to add the gRPC client:

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpcClient<ServerService.ServerServiceClient>(options =>
{
    options.Address = new Uri("https://localhost:5001");  // gRPC server URL
});

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();

** Inject the gRPC Client into the Controller **
In GrpcClientAPI, modify WeatherForecastController to call the gRPC server:

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ServerService.ServerServiceClient _grpcClient;

    public WeatherForecastController(ServerService.ServerServiceClient grpcClient)
    {
        _grpcClient = grpcClient;
    }

    [HttpGet("GetServerValue/{id}")]
    public async Task<string> GetServerValue(int id)
    {
        var response = await _grpcClient.GetByIdAsync(new GetByIdRequest { Id = id });
        return response.Value;
    }
}

** Running the Application **

1. Set Different Ports for Both Projects
In GrpcServerAPI:

Right-click the project > Properties > Debug tab.
Set App URL to https://localhost:5001.
In GrpcClientAPI:

Right-click the project > Properties > Debug tab.
Set App URL to https://localhost:5002.
2. Run Both Projects Simultaneously
Right-click the solution > Properties.
Under Startup Project, select Multiple Startup Projects.
Set both GrpcServerAPI and GrpcClientAPI to Start.
Press F5 to start both projects.
Now you can test the interaction between the server and client.

Testing the Application
To test the GrpcClientAPI calling the GrpcServerAPI, you can visit the following endpoint in a browser or use Postman:

GET https://localhost:5002/WeatherForecast/GetServerValue/1
This should call the GetById gRPC method on the server and return the value.


