using System;
using Antlr4.StringTemplate;

namespace MediatR.HttpBindings.CodeGeneration
{
    public class InterfaceTemplateRenderer : ITemplateRenderer
    {
        private readonly Template _template;

        public InterfaceTemplateRenderer(Template template)
        {
            _template = template;
        }

        public string Render(Class type)
        {
            var st = new Template(_template);
            st.Add("class", type);
            return st.Render();
        }
    }
}