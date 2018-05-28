using System;

namespace MediatR.HttpBindings
{
    public class HttpBindingAttribute : Attribute
    {
        public string Method { get; private set; }
        public string Url { get; private set; }

        public HttpBindingAttribute(string method, string url)
        {
            Method = method;
            Url = url;
        }
    }
}
