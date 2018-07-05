using MediatR;
using MediatR.HttpBindings;

namespace ExampleApp.Contracts
{
    [HttpBinding]
    public class GetValuesRequest : IRequest<GetValuesResponse>
    {
    }
}