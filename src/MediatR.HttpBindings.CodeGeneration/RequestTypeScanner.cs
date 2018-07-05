using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MediatR.HttpBindings.CodeGeneration
{
    public class RequestTypeScanner : ITypeScanner
    {
        private readonly IEnumerable<Assembly> _assemblies;

        public RequestTypeScanner(IEnumerable<Assembly> assemblies)
        {
            _assemblies = assemblies;
        }

        public IEnumerable<Type> Scan()
        {
            return _assemblies.SelectMany(a =>
                a.GetTypes().Where(t => t.GetCustomAttribute(typeof(HttpBindingAttribute)) != null)).ToArray();
        }
    }
}