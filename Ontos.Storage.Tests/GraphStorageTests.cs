using System;
using Xunit;

namespace Ontos.Storage.Tests
{
    public class GraphStorageTests
    {
        private GraphStorage _storage;

        public GraphStorageTests()
        {
            _storage = new GraphStorage("bolt://localhost:7687/db/data", "neo4j", "1234");
        }

        [Fact]
        public async void TestCreateContent()
        {
            var newContent = new NewContent("Contenu super cool.");

            var content = await _storage.CreateContentAsync(newContent);

            Assert.True(content.Id > 0);
            Assert.Equal("Contenu super cool.", content.Details);
        }
    }
}
