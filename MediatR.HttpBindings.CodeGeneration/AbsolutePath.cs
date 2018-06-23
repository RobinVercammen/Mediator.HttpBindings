using System;
using System.IO;

namespace MediatR.HttpBindings.CodeGeneration
{
    public class AbsolutePath
    {
        private readonly string _path;

        public AbsolutePath(string path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            _path = Path.IsPathRooted(path) ? path : Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, path));
        }

        public override string ToString()
        {
            return _path;
        }
    }
}