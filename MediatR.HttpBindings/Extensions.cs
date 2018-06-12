using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MediatR.HttpBindings
{
    public static class Extensions
    {
        public static IMvcBuilder AddMediatRHttpBindings(this IMvcBuilder builder, params Assembly[] assemblies)
        {
            builder.AddMvcOptions(opt => opt.ModelBinderProviders.Insert(0, new RequestModelBinderProvider()));
            return builder.ConfigureApplicationPartManager(apm =>
                        apm.FeatureProviders.Add(new HttpBindingsProvider(assemblies)));
        }
    }
}
