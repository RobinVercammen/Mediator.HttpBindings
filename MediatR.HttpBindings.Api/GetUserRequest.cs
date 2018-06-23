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
        public User User { get; set; }
    }

    public class User
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }

    public class GetUserRequestHandler : IRequestHandler<GetUserRequest, GetUserResponse>
    {
        public async Task<GetUserResponse> Handle(GetUserRequest request, CancellationToken cancellationToken)
        {
            await Task.Delay(100);
            return new GetUserResponse {User = new User {Name = "Robin", Id = request.Id}};
        }
    }
}