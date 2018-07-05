using System;
using System.Linq;

namespace MediatR.HttpBindings.CodeGeneration
{
    public class Extend
    {
        public Extend(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            TypeName = type.Name.Split('`').First();
            GenericArguments = type.GetGenericArguments().Select(t => t.Name).ToArray();
        }

        public string TypeName { get; }
        public string[] GenericArguments { get; }
    }
}