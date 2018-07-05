using System;

namespace MediatR.HttpBindings.CodeGeneration
{
    public class Contract
    {
        public string Name { get; }
        public string Content { get; }

        public Contract(string name, string content)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }
    }
}