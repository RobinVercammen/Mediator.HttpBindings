using System.Collections;
using System.Linq;
using System.Reflection;

namespace MediatR.HttpBindings.CodeGeneration
{
    public class Property
    {
        public Property(PropertyInfo propertyInfo)
        {
            Name = propertyInfo.Name;
            ArrayLike = propertyInfo.PropertyType != typeof(string) &&
                        typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType);
            TypeName = ArrayLike
                ? (propertyInfo.PropertyType.HasElementType
                    ? propertyInfo.PropertyType.GetElementType().Name
                    : propertyInfo.PropertyType.GetGenericArguments().First().Name)
                : propertyInfo.PropertyType.Name;
        }

        public string Name { get; }
        public string TypeName { get; }
        public bool ArrayLike { get; }
    }
}