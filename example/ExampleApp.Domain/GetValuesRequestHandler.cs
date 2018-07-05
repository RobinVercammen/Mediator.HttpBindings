using System.Threading;
using System.Threading.Tasks;
using ExampleApp.Contracts;
using MediatR;

namespace ExampleApp.Domain
{
    public class GetValuesRequestHandler : IRequestHandler<GetValuesRequest, GetValuesResponse>
    {
        public Task<GetValuesResponse> Handle(GetValuesRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new GetValuesResponse()
            {
                ValuesOverview = new[] {new ValueOverview {Id = "id", Name = "name"}}
            });
        }
    }
}