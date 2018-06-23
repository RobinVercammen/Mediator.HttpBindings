﻿using System;
using Antlr4.StringTemplate;

namespace MediatR.HttpBindings.CodeGeneration
{
    public class InterfaceTemplateRenderer : ITemplateRenderer
    {
        private readonly Template _template;
        private readonly Type _requestType;

        public InterfaceTemplateRenderer(Template template, Type requestType)
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