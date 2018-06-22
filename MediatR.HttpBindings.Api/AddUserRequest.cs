using System.Threading;
using System.Threading.Tasks;

namespace MediatR.HttpBindings.Api
{
    [HttpBinding]
    public class AddUserRequest : IRequest<AddUserResponse>
    {
        public string Username { get; set; }
    }

    public class AddUserResponse
    {
        public string Id { get; set; }
    }

    public class AddUserRequestHandler : IRequestHandler<AddUserRequest, AddUserResponse>
    {
        public async Task<AddUserResponse> Handle(AddUserRequest request, CancellationToken cancellationToken)
        {
            await Task.Delay(1000);
            return new AddUserResponse() { Id = request.Username };
        }
    }
}