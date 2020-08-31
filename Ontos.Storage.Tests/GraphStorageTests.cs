using Ontos.Contracts;
using System;
using System.Linq;
using System.Reflection.Metadata;
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
            //return _storage.DeleteAll();
            return Task.CompletedTask;
        }

        [Fact]
        public async void TestCRUDContent()
        {
            // CREATE
            var newContent = new NewPage("Contenu super cool.");
            var createdContent = await _storage.CreatePage(newContent);
            Assert.Equal("Contenu super cool.", createdContent.Content);

            // GET
            var id = createdContent.Id;
            var getContent = await _storage.GetPage(id);
            Assert.Equal(createdContent, getContent);

            // UPDATE
            var updateContent = new UpdatePage(id, "Contenu super cool (mis à jour).");
            var updatedContent = await _storage.UpdatePage(updateContent);
            Assert.Equal("Contenu super cool (mis à jour).", updatedContent.Content);

            // GET
            getContent = await _storage.GetPage(id);
            Assert.Equal(updatedContent, getContent);

            // DELETE
            bool deleted = await _storage.DeletePage(id);
            Assert.True(deleted);

            // GET
            getContent = await _storage.GetPage(id);
            Assert.Null(getContent);
        }

        [Fact]
        public async void TestCreatePage_WithExpression()
        {
            // CREATE
            var newPage = new NewPage("Contenu super cool.", new NewExpression("fra", "Phénoménologie"));
            var createdPage = await _storage.CreatePage(newPage);
            Assert.Equal("Contenu super cool.", createdPage.Content);
            Assert.Single(createdPage.References);

            var reference = createdPage.References[0];
            Assert.Equal(createdPage.Id, reference.PageId);

            var expression = reference.Expression;
            Assert.Equal("fra", expression.Language);
            Assert.Equal("Phénoménologie", expression.Label);
        }

        [Fact]
        public async void TestGetPage_WithMultipleReferences()
        {
            // CREATE
            var newPage = new NewPage("Contenu super cool.", new NewExpression("fra", "Phénoménologie"));
            var createdPage = await _storage.CreatePage(newPage);
            await _storage.CreateReference(new NewReference(createdPage.Id, "fra", "Gloubi boulga"));

            // GET
            var gettedPage = await _storage.GetPage(createdPage.Id);
            Assert.Equal(2, gettedPage.References.Length);
            Assert.Equal(new[] { "Gloubi boulga", "Phénoménologie" },
                gettedPage.References.Select(r => r.Expression.Label).OrderBy(l => l));
        }

        [Fact]
        public async void TestDeleteContent_NotFound()
        {
            bool deleted = await _storage.DeletePage(999);
            Assert.False(deleted);
        }

        [Fact]
        public async void TestCRUDExpression()
        {
            // CREATE
            var newExpression = new NewExpression("fra", "Phénoménologie");
            var createdExpression = await _storage.CreateExpression(newExpression);
            Assert.Equal("fra", createdExpression.Language);
            Assert.Equal("Phénoménologie", createdExpression.Label);

            // GET
            var id = createdExpression.Id;
            var getExpression = await _storage.GetExpression(id);
            Assert.Equal(createdExpression, getExpression);

            // UPDATE 1
            var updateExpression = new UpdateExpression(id, language: "eng");
            var updatedExpression = await _storage.UpdateExpression(updateExpression);
            Assert.Equal("eng", updatedExpression.Language);
            Assert.Equal("Phénoménologie", updatedExpression.Label);

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

        [Fact]
        public async void TestDeleteExpression_NotFound()
        {
            bool deleted = await _storage.DeleteExpression(999);
            Assert.False(deleted);
        }

        [Fact]
        public async void TestCRUDReference()
        {
            var content = await _storage.CreatePage(new NewPage("Contenu super cool."));

            // CREATE
            var created = await _storage.CreateReference(new NewReference(
                content.Id, "fra", "Phénoménologie"));
            Assert.Equal("fra", created.Expression.Language);
            Assert.Equal("Phénoménologie", created.Expression.Label);

            // GET
            var getted = await _storage.GetReference(created.Id);
            Assert.Equal(created, getted);

            // DELETE
            bool deleted = await _storage.DeleteReference(created.Id);
            Assert.True(deleted);

            // GET
            getted = await _storage.GetReference(created.Id);
            Assert.Null(getted);
        }

        [Fact]
        public async void TestCreateReference_ExpressionExists()
        {
            var content = await _storage.CreatePage(new NewPage("Contenu super cool."));
            var expression = await _storage.CreateExpression(new NewExpression("fra", "Phénoménologie"));

            // CREATE
            var created = await _storage.CreateReference(new NewReference(
                content.Id, expression.Language, expression.Label));

            Assert.Equal(expression, created.Expression);
        }

        [Fact]
        public async void TestDeleteReference_NotFound()
        {
            bool deleted = await _storage.DeleteReference(999);
            Assert.False(deleted);
        }
    }
}
