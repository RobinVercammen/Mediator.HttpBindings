using System.Collections.Generic;
using System.Linq;

namespace MediatR.HttpBindings.CodeGeneration
{
    public class ContractFactory
    {
        private readonly ITemplateRenderer _renderer;

        public ContractFactory(ITemplateRenderer renderer)
        {
            _renderer = renderer;
        }

        public IEnumerable<Contract> Create(IEnumerable<Class> classes)
        {
            return classes.Select(c => new Contract(c.TypeName, _renderer.Render(c)));
        }
    }
}