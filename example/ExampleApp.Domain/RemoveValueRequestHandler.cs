using System.Threading;
using System.Threading.Tasks;
using ExampleApp.Contracts;
using MediatR;

namespace ExampleApp.Domain
{
    public class RemoveValueRequestHandler : IRequestHandler<RemoveValueRequest, RemoveValueResponse>
    {
        public Task<RemoveValueResponse> Handle(RemoveValueRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new RemoveValueResponse());
        }
    }
}