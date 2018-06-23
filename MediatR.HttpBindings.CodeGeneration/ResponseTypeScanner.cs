using System;
using System.Collections.Generic;
using System.Linq;

namespace MediatR.HttpBindings.CodeGeneration
{
    public class ResponseTypeScanner : ITypeScanner
    {
        private readonly IEnumerable<Type> _requestTypes;

        public ResponseTypeScanner(IEnumerable<Type> requestTypes)
        {
            _requestTypes = requestTypes?.ToList() ?? throw new ArgumentNullException(nameof(requestTypes));
        }

        public IEnumerable<Type> Scan()
        {
            return _requestTypes.Select(req =>
                req.GetInterfaces().First(i => i.GetGenericArguments().Any() && i.Name.StartsWith("IRequest"))
                    .GetGenericArguments().Single());
        }
    }
}