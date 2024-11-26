using Grpc.Core;
namespace GrpcServerAPI
{
    public class ServerServiceImpl : ServerService.ServerServiceBase
    {
        public override Task<GetByIdResponse> GetById(GetByIdRequest request, ServerCallContext context)
        {
            return Task.FromResult(new GetByIdResponse { Value = $"Value for ID: {request.Id}" });
        }
    }

}
