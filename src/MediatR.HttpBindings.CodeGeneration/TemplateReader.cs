using System;
using System.IO;

namespace MediatR.HttpBindings.CodeGeneration
{
    public class TemplateReader
    {
        public string Read(string path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            var absolutePath = new AbsolutePath(path);
            return File.ReadAllText(absolutePath.ToString());
        }
    }
}