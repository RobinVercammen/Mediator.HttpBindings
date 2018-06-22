using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MediatR.HttpBindings.Api
{
    [HttpBinding]
    public class GetUsersRequest : IRequest<GetUsersResponse>
    {
    }

    public class GetUsersResponse
    {
        public List<string> UserNames { get; set; }
    }

    public class GetUsersRequestHandler : IRequestHandler<GetUsersRequest, GetUsersResponse>
    {
        public async Task<GetUsersResponse> Handle(GetUsersRequest request, CancellationToken cancellationToken)
        {
            await Task.Delay(100);
            return new GetUsersResponse() {UserNames = new List<string> {"Robin"}};
        }
    }
}