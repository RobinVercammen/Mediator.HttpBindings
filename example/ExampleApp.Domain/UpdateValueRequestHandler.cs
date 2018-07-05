using System.Threading;
using System.Threading.Tasks;
using ExampleApp.Contracts;
using MediatR;

namespace ExampleApp.Domain
{
    public class UpdateValueRequestHandler : IRequestHandler<UpdateValueRequest, UpdateValueResponse>
    {
        public Task<UpdateValueResponse> Handle(UpdateValueRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new UpdateValueResponse()
            {
                UpdatedValue = request.Value
            });
        }
    }
}