using System;
using Antlr4.StringTemplate;

namespace MediatR.HttpBindings.CodeGeneration
{
    public class RequestTemplateRenderer : ITemplateRenderer
    {
        private readonly Template _template;
        private readonly Class _type;

        public RequestTemplateRenderer(Template template, Class type)
        {
            _template = template;
            _type = type;
        }

        public string Render()
        {
            var st = new Template(_template);
            st.Add("class", _type);
            return st.Render();
        }
    }
}