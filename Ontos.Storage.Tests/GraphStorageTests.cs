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
            var updateContent = new UpdateContent(id, "Contenu super cool (mis � jour).");
            var updatedContent = await _storage.UpdateContent(updateContent);
            Assert.Equal("Contenu super cool (mis � jour).", updatedContent.Details);

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

        [Fact]
        public async void TestCRUDExpression()
        {
            // CREATE
            var newExpression = new NewExpression("fra", "Ph�nom�nologie");
            var createdExpression = await _storage.CreateExpression(newExpression);
            Assert.True(createdExpression.Id > 0);
            Assert.Equal("fra", createdExpression.Language);
            Assert.Equal("Ph�nom�nologie", createdExpression.Label);

            // GET
            var id = createdExpression.Id;
            var getExpression = await _storage.GetExpression(id);
            Assert.Equal(createdExpression, getExpression);

            // UPDATE 1
            var updateExpression = new UpdateExpression(id, language: "eng");
            var updatedExpression = await _storage.UpdateExpression(updateExpression);
            Assert.Equal("eng", updatedExpression.Language);
            Assert.Equal("Ph�nom�nologie", updatedExpression.Label);

            // GET
            getExpression = await _storage.GetExpression(id);
            Assert.Equal(updatedExpression, getExpression);

            // UPDATE 2
            updateExpression = new UpdateExpression(id, label: "Phenomenology");
            updatedExpression = await _storage.UpdateExpression(updateExpression);
            Assert.Equal("eng", updatedExpression.Language);
            Assert.Equal("Phenomenology", updatedExpression.Label);

            // GET
            getExpression = await _storage.GetExpression(id);
            Assert.Equal(updatedExpression, getExpression);

            // DELETE
            bool deleted = await _storage.DeleteExpression(id);
            Assert.True(deleted);

            // GET
            getExpression = await _storage.GetExpression(id);
            Assert.Null(getExpression);
        }
    }
}
