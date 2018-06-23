using System;
using Antlr4.StringTemplate;

namespace MediatR.HttpBindings.CodeGeneration
{
    public class ClassTemplateRenderer : ITemplateRenderer
    {
        private readonly Template _template;
        private readonly Type _requestType;

        public ClassTemplateRenderer(Template template, Type requestType)
        {
            _template = template;
            _requestType = requestType;
        }

        public string Render()
        {
            var st = new Template(_template);
            st.Add("request", _requestType);
            st.Add("properties", _requestType.GetProperties());
            return st.Render();
        }
    }
}