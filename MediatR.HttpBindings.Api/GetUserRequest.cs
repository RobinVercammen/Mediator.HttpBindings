using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MediatR.HttpBindings.Api
{
    [HttpBinding]
    public class GetUserRequest : IRequest<GetUserResponse>
    {
        public string Id { get; set; }
    }

    public class GetUserResponse
    {
        public List<string> UserNames { get; set; }
    }

    public class GetUserRequestHandler : IRequestHandler<GetUserRequest, GetUserResponse>
    {
        public async Task<GetUserResponse> Handle(GetUserRequest request, CancellationToken cancellationToken)
        {
            await Task.Delay(100);
            return new GetUserResponse {UserNames = new List<string> {"Robin", request.Id}};
        }
    }
}