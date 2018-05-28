using System.Threading.Tasks;

namespace MediatR.HttpBindings.Api
{
    [HttpBinding("POST", "users")]
    public class AddUserRequest : IRequest<AddUserResponse>
    {
        public string Username { get; set; }
    }

    public class AddUserResponse
    {
        public string Id { get; set; }
    }

    public class AddUserRequestHandler : AsyncRequestHandler<AddUserRequest, AddUserResponse>
    {
        protected override async Task<AddUserResponse> Handle(AddUserRequest request)
        {
            return new AddUserResponse() { Id = request.Username };
        }
    }
}