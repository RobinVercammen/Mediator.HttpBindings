using System;
using System.Collections.Generic;
using System.Linq;
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

            var assemblies = new AssemblyLoader(options.Assemblies).Load();

            var requests = new RequestTypeScanner(assemblies).Scan().ToArray();
            var responses = new ResponseTypeScanner(requests).Scan().ToArray();
            var usedTypes = new UsedTypesScanner(requests.Concat(responses)).Scan().ToArray();

            var reqClasses = Class.FromTypes(requests);
            var resClasses = Class.FromTypes(responses);
            var usedClasses = Class.FromTypes(usedTypes);

            var templateReader = new TemplateReader();
            var requestTemplate = templateReader.Read(options.RequestTemplate);
            var classTemplate = templateReader.Read(options.ClassTemplate);

            var requestContractFactory =
                new ContractFactory(new RequestTemplateRenderer(new Template(requestTemplate)));
            var requestContracts = requestContractFactory.Create(reqClasses);

            var classContractFactory = new ContractFactory(new ClassTemplateRenderer(new Template(classTemplate)));
            var responseContracts = classContractFactory.Create(resClasses);
            var usedClassContracts = classContractFactory.Create(usedClasses);


            var contractWriter = new ContractWriter(options.OutputFolder, options.Extension);
            var fileCount = await contractWriter.WriteContracts(requestContracts, responseContracts, usedClassContracts);

            Console.WriteLine($"Wrote {fileCount} files");
        }

        private class Options
        {
            [Option('a', "assemblies", Required = true, HelpText = "Assemblies to be processed.")]
            public IEnumerable<string> Assemblies { get; set; }

            [Option('r', "requesttemplate", Required = true, HelpText = "Request template to be processed.")]
            public string RequestTemplate { get; set; }

            [Option('c', "classtemplate", Required = true, HelpText = "Class template to be processed.")]
            public string ClassTemplate { get; set; }

            [Option('o', "output", Required = true, HelpText = "Output folder location.")]
            public string OutputFolder { get; set; }

            [Option('e', "extension", Required = true, HelpText = "Extension of generated contracts.")]
            public string Extension { get; set; }
        }
    }
}