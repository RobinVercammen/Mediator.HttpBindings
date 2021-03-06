﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace MediatR.HttpBindings.CodeGeneration
{
    public class Class
    {
        public Class(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            TypeName = type.Name;
            Properties = type.GetProperties().Select(p => new Property(p)).ToArray();
            Extends = type.GetInterfaces().Select(i => new Extend(i)).ToArray();
        }

        public string TypeName { get; }
        public Extend[] Extends { get; }
        public Property[] Properties { get; }

        private static Class FromType(Type type)
        {
            return new Class(type);
        }

        public static IEnumerable<Class> FromTypes(IEnumerable<Type> types)
        {
            return types.Select(FromType);
        }
    }
}