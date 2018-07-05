using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MediatR.HttpBindings
{
    public class RequestResponseController<T, U> : Controller where T : IRequest<U>
    {
        private readonly IMediator mediator;

        public RequestResponseController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        protected async Task<U> ExecuteAsync(T request)
        {
            return await mediator.Send(request);
        }
    }
}