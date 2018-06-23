using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using Xunit;

namespace MediatR.HttpBindings.Test
{
    public class Test
    {
        [Fact]
        public void Test1()
        {
            var combined = Path.GetFullPath(Path.Combine("/user/robin/dev", "../common"));
            combined.Should().Be("/user/robin/common");
        }
    }
}
