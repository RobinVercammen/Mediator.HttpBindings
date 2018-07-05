using System.Threading;
using System.Threading.Tasks;
using ExampleApp.Contracts;
using MediatR;

namespace ExampleApp.Domain
{
    public class GetValueRequestHandler : IRequestHandler<GetValueRequest, GetValueResponse>
    {
        public Task<GetValueResponse> Handle(GetValueRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new GetValueResponse()
            {
                Value = new ValueDetail {Id = request.Id, DetailText = "detailtext", Name = "name"}
            });
        }
    }
}