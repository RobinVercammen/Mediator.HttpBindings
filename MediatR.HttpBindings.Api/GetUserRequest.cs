using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediatR.HttpBindings.Api
{

    [HttpBinding("GET", "users")]
    public class GetUsersRequest : IRequest<GetUsersResponse>
    {

    }

    public class GetUsersResponse
    {
        public List<string> UserNames { get; set; }
    }

    public class GetUsersRequestHandler : AsyncRequestHandler<GetUsersRequest, GetUsersResponse>
    {
        protected override async Task<GetUsersResponse> Handle(GetUsersRequest request)
        {
            return new GetUsersResponse() { UserNames = new List<string> { "Robin" } };
        }
    }
}