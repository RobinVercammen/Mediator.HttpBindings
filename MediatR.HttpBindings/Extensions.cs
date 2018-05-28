using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MediatR.HttpBindings
{
    public static class Extensions
    {
        public static IMvcBuilder AddMediatRHttpBindings(this IMvcBuilder builder, params Assembly[] assemblies)
        {
            return builder.ConfigureApplicationPartManager(apm =>
                        apm.FeatureProviders.Add(new HttpBindingsProvider(assemblies)));
        }
    }
}
