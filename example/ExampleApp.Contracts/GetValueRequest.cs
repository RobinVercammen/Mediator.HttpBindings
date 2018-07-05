using MediatR;
using MediatR.HttpBindings;

namespace ExampleApp.Contracts
{
    [HttpBinding]
    public class GetValueRequest : IRequest<GetValueResponse>
    {
        public string Id { get; set; }
    }
}