using System;
using System.Collections.Generic;

namespace MediatR.HttpBindings.CodeGeneration
{
    internal interface ITypeScanner
    {
        IEnumerable<Type> Scan();
    }
}