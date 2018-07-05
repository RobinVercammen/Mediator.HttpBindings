using MediatR;
using MediatR.HttpBindings;

namespace ExampleApp.Contracts
{
    [HttpBinding]
    public class RemoveValueRequest : IRequest<RemoveValueResponse>
    {
        public string Id { get; set; }
    }
}