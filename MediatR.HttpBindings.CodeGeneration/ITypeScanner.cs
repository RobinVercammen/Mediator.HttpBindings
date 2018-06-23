using System;
using System.Collections.Generic;

namespace MediatR.HttpBindings.CodeGeneration
{
    interface ITypeScanner
    {
        IEnumerable<Type> Scan();
    }
}