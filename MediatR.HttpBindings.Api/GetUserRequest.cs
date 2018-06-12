using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediatR.HttpBindings.Api
{
    [HttpBinding("GET", "users/{id}")]
    public class GetUserRequest : IRequest<GetUserResponse>
    {
        public string Id { get; set; }
    }

    public class GetUserResponse
    {
        public List<string> UserNames { get; set; }
    }

    public class GetUserRequestHandler : AsyncRequestHandler<GetUserRequest, GetUserResponse>
    {
        protected override async Task<GetUserResponse> Handle(GetUserRequest request)
        {
            return new GetUserResponse {UserNames = new List<string> {"Robin", request.Id}};
        }
    }
}