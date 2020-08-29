using System;
using System.Threading.Tasks;
using Xunit;

namespace Ontos.Storage.Tests
{
    public class GraphStorageTests : IAsyncLifetime
    {
        private GraphStorage _storage;

        public GraphStorageTests()
        {
            _storage = new GraphStorage("bolt://localhost:7687/db/data", "neo4j", "1234");
        }

        public Task InitializeAsync()
        {
            return _storage.DeleteAll();
        }

        public Task DisposeAsync()
        {
            return _storage.DeleteAll();
        }

        [Fact]
        public async void TestCRUDContent()
        {
            // CREATE
            var newContent = new NewContent("Contenu super cool.");
            var createdContent = await _storage.CreateContent(newContent);
            Assert.True(createdContent.Id > 0);
            Assert.Equal("Contenu super cool.", createdContent.Details);

            // GET
            var id = createdContent.Id;
            var getContent = await _storage.GetContent(id);
            Assert.Equal(createdContent, getContent);

            // UPDATE
            var updateContent = new UpdateContent(id, "Contenu super cool (mis à jour).");
            var updatedContent = await _storage.UpdateContent(updateContent);
            Assert.Equal("Contenu super cool (mis à jour).", updatedContent.Details);

            // GET
            getContent = await _storage.GetContent(id);
            Assert.Equal(updatedContent, getContent);

            // DELETE
            bool deleted = await _storage.DeleteContent(id);
            Assert.True(deleted);

            // GET
            getContent = await _storage.GetContent(id);
            Assert.Null(getContent);
        }

        [Fact]
        public async void TestDeleteContent_NotFound()
        {
            bool deleted = await _storage.DeleteContent(999);
            Assert.False(deleted);
        }
    }
}
