using System;
using Antlr4.StringTemplate;

namespace MediatR.HttpBindings.CodeGeneration
{
    public class ClassTemplateRenderer : ITemplateRenderer
    {
        private readonly Template _template;

        public ClassTemplateRenderer(Template template)
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