using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Antlr4.StringTemplate;
using CommandLine;

namespace MediatR.HttpBindings.CodeGeneration
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Options options = null;
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(opts => options = opts);

            var assemblies = LoadAssemblies(options.Assemblies);

            var requests = assemblies.SelectMany(a =>
                a.GetTypes().Where(t => t.GetCustomAttribute(typeof(HttpBindingAttribute)) != null)).ToArray();
            var responses = requests.Select(req =>
                req.GetInterfaces().First(i => i.GetGenericArguments().Any() && i.Name.StartsWith("IRequest"))
                    .GetGenericArguments().Single());

            var requestTemplate = File.ReadAllText(options.RequestTemplate);

            foreach (var request in requests)
            {
                var st = new Template(requestTemplate);
                st.Add("request", request);
                st.Add("properties", request.GetProperties());
                var rendered = st.Render();
                Console.WriteLine(rendered);
            }


            await Task.Delay(100);
            Console.WriteLine(string.Join(", ", requests.Select(r => r.FullName)));
            Console.WriteLine(string.Join(", ", responses.Select(r => r.FullName)));
        }

        public static Assembly[] LoadAssemblies(IEnumerable<string> paths)
        {
            return paths.Select(path => AssemblyLoadContext.Default.LoadFromAssemblyPath(path)).ToArray();
        }

        private class Options
        {
            [Option('a', "assemblies", Required = true, HelpText = "Assemblies to be processed.")]
            public IEnumerable<string> Assemblies { get; set; }

            [Option('r', "requesttemplate", Required = true, HelpText = "Request template to be processed.")]
            public string RequestTemplate { get; set; }
        }
    }
}