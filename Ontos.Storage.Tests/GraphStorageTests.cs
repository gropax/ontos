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
        public async void TestCRUDPage()
        {
            // CREATE
            var newPage = new NewPage("Contenu super cool.");
            var createdPage = await _storage.CreatePage(newPage);
            Assert.Equal("Contenu super cool.", createdPage.Content);

            // GET
            var id = createdPage.Id;
            var getPage = await _storage.GetPage(id);
            Assert.Equal(createdPage, getPage);

            // UPDATE
            var updatePage = new UpdatePage(id, "Contenu super cool (mis à jour).");
            var updatedPage = await _storage.UpdatePage(updatePage);
            Assert.Equal("Contenu super cool (mis à jour).", updatedPage.Content);

            // GET
            getPage = await _storage.GetPage(id);
            Assert.Equal(updatedPage, getPage);

            // DELETE
            var deleted = await _storage.DeletePages(id);
            Assert.Single(deleted);

            // GET
            getPage = await _storage.GetPage(id);
            Assert.Null(getPage);
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
            await _storage.CreateReference(new NewReference(createdPage.Id, new NewExpression("fra", "Gloubi boulga")));

            // GET
            var gettedPage = await _storage.GetPage(createdPage.Id);
            Assert.Equal(2, gettedPage.References.Length);
            Assert.Equal(new[] { "Gloubi boulga", "Phénoménologie" },
                gettedPage.References.Select(r => r.Expression.Label).OrderBy(l => l));
        }

        [Fact]
        public async void TestDeletePage_NotFound()
        {
            var deleted = await _storage.DeletePages(999);
            Assert.Empty(deleted);
        }

        [Fact]
        public async void TestDeletePage_WithReferences()
        {
            // CREATE
            var p1 = await _storage.CreatePage(new NewPage("1"));
            var p2 = await _storage.CreatePage(new NewPage("2"));
            var e1 = new NewExpression("fra", "Phénoménologie");
            var e2 = new NewExpression("fra", "Philosophie");
            var r1 = await _storage.CreateReference(new NewReference(p1.Id, e1));
            var r2 = await _storage.CreateReference(new NewReference(p2.Id, e1));
            var r3 = await _storage.CreateReference(new NewReference(p1.Id, e2));

            // GET
            var getP1 = await _storage.GetPage(p1.Id);
            var getP2 = await _storage.GetPage(p2.Id);
            Assert.NotNull(getP1);
            Assert.NotNull(getP2);
            //
            Assert.True(await _storage.ReferenceExists(r1.Id));
            Assert.True(await _storage.ReferenceExists(r2.Id));
            Assert.True(await _storage.ReferenceExists(r3.Id));

            // DELETE
            var deleted = await _storage.DeletePages(p1.Id);
            Assert.Single(deleted);

            // GET
            getP1 = await _storage.GetPage(p1.Id);
            getP2 = await _storage.GetPage(p2.Id);
            Assert.Null(getP1);
            Assert.NotNull(getP2);
            //
            Assert.False(await _storage.ReferenceExists(r1.Id));
            Assert.True(await _storage.ReferenceExists(r2.Id));
            Assert.False(await _storage.ReferenceExists(r3.Id));
            //
            var getE1 = await _storage.GetExpression(r1.Expression.Id);
            var getE2 = await _storage.GetExpression(r3.Expression.Id);
            Assert.NotNull(getE1);
            Assert.Null(getE2);
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
                content.Id, new NewExpression("fra", "Phénoménologie")));
            Assert.Equal("fra", created.Expression.Language);
            Assert.Equal("Phénoménologie", created.Expression.Label);

            // GET
            var getted = await _storage.GetReference(created.Id);
            Assert.Equal(created, getted);

            // DELETE
            var deleted = await _storage.DeleteReferences(created.Id);
            Assert.Single(deleted);

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
                content.Id, new NewExpression(expression.Language, expression.Label)));

            Assert.Equal(expression, created.Expression);
        }

        [Fact]
        public async void TestDeleteReference_NotFound()
        {
            var deleted = await _storage.DeleteReferences(999);
            Assert.Empty(deleted);
        }

        [Fact]
        public async void TestValidateRelation_DirectedAcyclic()
        {
            // CREATE Page
            var p1 = await _storage.CreatePage(new NewPage("1"));
            var p2 = await _storage.CreatePage(new NewPage("2"));
            var p3 = await _storage.CreatePage(new NewPage("3"));
            var p4 = await _storage.CreatePage(new NewPage("4"));

            // CREATE Relations
            //
            var type = RelationType.Inclusion;
            Assert.True(type.Directed);
            Assert.True(type.Acyclic);
            //
            var r1 = await _storage.CreateRelation(new NewRelation(type, p1.Id, p2.Id));
            var r2 = await _storage.CreateRelation(new NewRelation(type, p1.Id, p3.Id));
            var r3 = await _storage.CreateRelation(new NewRelation(type, p3.Id, p4.Id));

            Assert.NotNull(r1);
            Assert.NotNull(r2);
            Assert.NotNull(r3);

            // VALIDATE
            bool v1 = await _storage.ValidateRelation(new NewRelation(type, p1.Id, p4.Id));
            bool v2 = await _storage.ValidateRelation(new NewRelation(type, p4.Id, p1.Id));
            bool v3 = await _storage.ValidateRelation(new NewRelation(type, p3.Id, p2.Id));
            bool v4 = await _storage.ValidateRelation(new NewRelation(type, p2.Id, p4.Id));
            bool v5 = await _storage.ValidateRelation(new NewRelation(type, p4.Id, p3.Id));

            Assert.True(v1);
            Assert.False(v2);
            Assert.True(v3);
            Assert.True(v4);
            Assert.False(v5);

            // ENSURE invalid Relations are not created
            var r4 = await _storage.CreateRelation(new NewRelation(type, p4.Id, p1.Id));
            var r5 = await _storage.CreateRelation(new NewRelation(type, p4.Id, p3.Id));

            Assert.Null(r4);
            Assert.Null(r5);
        }

        [Fact]
        public async void TestCRDRelations()
        {
            // CREATE Page
            var p1 = await _storage.CreatePage(new NewPage("1"));
            var p2 = await _storage.CreatePage(new NewPage("2"));

            // CREATE Relations
            var type = RelationType.Inclusion;
            var r1 = await _storage.CreateRelation(new NewRelation(type, p1.Id, p2.Id));
            var r2 = await _storage.CreateRelation(new NewRelation(type, p1.Id, p2.Id));  // Duplicate

            Assert.NotNull(r1);
            Assert.Equal(r1, r2);  // Can't create duplicate relations

            // GET Relations
            var rx = await _storage.GetRelationsFrom(p1.Id, type);
            Assert.Single(rx);
            Assert.Equal(r1, rx[0]);

            // DELETE Relations
            var success = await _storage.DeleteRelation(r1.Id);
            Assert.True(success);

            // GET Relations
            rx = await _storage.GetRelationsFrom(p1.Id, type);
            Assert.Empty(rx);
        }
    }
}
