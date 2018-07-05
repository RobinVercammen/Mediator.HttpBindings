using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace MediatR.HttpBindings.CodeGeneration
{
    public class AssemblyLoader
    {
        private readonly IEnumerable<string> _assemblyLocations;

        public AssemblyLoader(IEnumerable<string> assemblyLocations)
        {
            _assemblyLocations = assemblyLocations?.Select(EnsureAbsolutePath) ??
                                 throw new ArgumentNullException(nameof(assemblyLocations));
        }

        private static string EnsureAbsolutePath(string path)
        {
            return new AbsolutePath(path).ToString();
        }

        public IEnumerable<Assembly> Load()
        {
            return _assemblyLocations.Select(path => AssemblyLoadContext.Default.LoadFromAssemblyPath(path));
        }
    }
}