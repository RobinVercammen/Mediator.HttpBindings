using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MediatR.HttpBindings.CodeGeneration
{
    public class ContractWriter
    {
        private readonly string _extension;
        private readonly AbsolutePath _baseDirectory;

        public ContractWriter(string baseDirectory, string extension)
        {
            if (baseDirectory == null) throw new ArgumentNullException(nameof(baseDirectory));
            _extension = extension ?? throw new ArgumentNullException(nameof(extension));
            _baseDirectory = new AbsolutePath(baseDirectory);
        }

        public async Task<int> WriteContracts(params IEnumerable<Contract>[] contracts)
        {
            var flattendContracts = contracts.SelectMany(c => c).ToList();
            var directory = _baseDirectory.ToString();
            EnsureDirectoryExists(directory);
            ClearDirectory(directory);
            await Task.WhenAll(flattendContracts.Select(contract => WriteContract(directory, contract, _extension)));
            return flattendContracts.Count;
        }

        private Task WriteContract(string baseDir, Contract c, string extension)
        {
            var path = Path.Combine(baseDir, c.Name + extension);
            return File.WriteAllTextAsync(path, c.Content);
        }

        private void EnsureDirectoryExists(string dir)
        {
            Directory.CreateDirectory(dir);
        }

        private void ClearDirectory(string dir)
        {
            Directory.Delete(dir, true);
            EnsureDirectoryExists(dir);
        }
    }
}