using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.StringTemplate;
using CommandLine;

namespace MediatR.HttpBindings.CodeGeneration
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Options options = null;
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(opts => options = opts);

            var assemblies = new AssemblyLoader(options.Assemblies).Load();

            var requests = new RequestTypeScanner(assemblies).Scan().ToArray();
            var responses = new ResponseTypeScanner(requests).Scan().ToArray();
            var usedTypes = new UsedTypesScanner(requests.Concat(responses)).Scan().ToArray();

            var templateReader = new TemplateReader();
            var requestTemplate = templateReader.Read(options.RequestTemplate);
            var classTemplate = templateReader.Read(options.ClassTemplate);

            foreach (var request in requests.Select(r=>new Class(r)))
            {
                var reqRenderer = new RequestTemplateRenderer(new Template(requestTemplate), request);
                var rendered = reqRenderer.Render();
                Console.WriteLine(rendered);
            }

            foreach (var response in responses.Select(r=>new Class(r)).Concat(usedTypes.Select(r=>new Class(r))))
            {
                var resRenderer = new ClassTemplateRenderer(new Template(classTemplate), response);
                var rendered = resRenderer.Render();
                Console.WriteLine(rendered);
            }
        }

        private class Options
        {
            [Option('a', "assemblies", Required = true, HelpText = "Assemblies to be processed.")]
            public IEnumerable<string> Assemblies { get; set; }

            [Option('r', "requesttemplate", Required = true, HelpText = "Request template to be processed.")]
            public string RequestTemplate { get; set; }

            [Option('c', "classtemplate", Required = true, HelpText = "Class template to be processed.")]
            public string ClassTemplate { get; set; }
        }
    }
}