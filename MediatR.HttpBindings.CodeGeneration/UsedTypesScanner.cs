using System;
using System.Collections.Generic;
using System.Linq;

namespace MediatR.HttpBindings.CodeGeneration
{
    public class UsedTypesScanner : ITypeScanner
    {
        private readonly IEnumerable<Type> _types;

        public UsedTypesScanner(IEnumerable<Type> types)
        {
            _types = types ?? throw new ArgumentNullException(nameof(types));
        }

        public IEnumerable<Type> Scan()
        {
            var types = new List<Type>();
            var typeQueue = new Queue<Type>(_types.SelectMany(GetPropertyTypes));
            while (typeQueue.TryDequeue(out var currentType))
            {
                types.Add(currentType);
                foreach (var propertyType in GetPropertyTypes(currentType))
                {
                    typeQueue.Enqueue(propertyType);
                }
            }

            return types.GroupBy(t => t.Name).Select(t => t.First());
        }

        private static IEnumerable<Type> GetPropertyTypes(Type type)
        {
            return type.GetProperties().Select(p => p.PropertyType);
        }
    }
}