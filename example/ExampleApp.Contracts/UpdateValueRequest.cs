using MediatR;
using MediatR.HttpBindings;

namespace ExampleApp.Contracts
{
    [HttpBinding]
    public class UpdateValueRequest : IRequest<UpdateValueResponse>
    {
        public string Id { get; set; }
        public ValueDetail Value { get; set; }
    }
}