using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace MediatR.HttpBindings
{
    internal class RequestModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            return new BinderTypeModelBinder(typeof(RequestModelBinder));
        }
    }
}