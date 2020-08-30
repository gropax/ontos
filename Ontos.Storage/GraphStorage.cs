using Neo4j.Driver;
using Ontos.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ontos.Storage
{
    public interface IGraphStorage
    {
    }

    public class GraphStorage : IGraphStorage
    {
        private IDriver _driver;

        public GraphStorage(string uri, string user, string password)
        {
            _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
        }

        public async Task DeleteAll()
        {
            await Transaction(t => t.RunAsync(@"MATCH (n) DETACH DELETE n"));
        }

        public async Task<Content> GetContent(long id)
        {
            return await Transaction(t => GetContent(t, id));
        }

        public async Task<Content> CreateContent(NewContent newContent)
        {
            return await Transaction(t => CreateContent(t, newContent));
        }

        public async Task<Content> UpdateContent(UpdateContent updateContent)
        {
            return await Transaction(t => UpdateContent(t, updateContent));
        }

        public async Task<bool> DeleteContent(long id)
        {
            return await Transaction(t => DeleteContent(t, id));
        }

        public async Task<Expression> GetExpression(long id)
        {
            return await Transaction(t => GetExpressionById(t, id));
        }

        public async Task<Expression> CreateExpression(NewExpression newExpression)
        {
            return await Transaction(t => CreateExpression(t, newExpression));
        }

        public async Task<Expression> UpdateExpression(UpdateExpression updateExpression)
        {
            return await Transaction(t => UpdateExpression(t, updateExpression));
        }

        public async Task<bool> DeleteExpression(long id)
        {
            return await Transaction(t => DeleteExpression(t, id));
        }



        public async Task<Reference> GetReference(long id)
        {
            return await Transaction(t => GetReferenceById(t, id));
        }

        public async Task<Reference> CreateReference(NewReference newReference)
        {
            return await Transaction(async t =>
            {
                var expression = await CreateExpression(t, newReference.NewExpression);
                return await CreateReference(t, newReference.ContentId, expression.Id);
            });
        }

        public async Task<bool> DeleteReference(long id)
        {
            return await Transaction(t => DeleteReference(t, id));
        }



        private async Task<Content> GetContent(IAsyncTransaction transaction, long id)
        {
            var cursor = await transaction.RunAsync(@"
                MATCH (c:Content)
                WHERE id(c)=$id
                RETURN c",
                new { id });

            var content = await cursor.ToListAsync(r => Map.Content(r["c"].As<INode>()));
            return content.FirstOrDefault();
        }

        private async Task<Content> CreateContent(IAsyncTransaction transaction, NewContent newContent)
        {
            var cursor = await transaction.RunAsync(@"
                CREATE (c:Content $content)
                RETURN c",
                new { content = newContent.Properties });

            var content = await cursor.ToListAsync(r => Map.Content(r["c"].As<INode>()));
            return content.First();
        }

        private async Task<Content> UpdateContent(IAsyncTransaction transaction, UpdateContent updateContent)
        {
            var cursor = await transaction.RunAsync(@"
                MATCH (c:Content)
                WHERE id(c)=$id
                SET c = $content
                RETURN c",
                new
                {
                    id = updateContent.Id,
                    content = updateContent.Properties,
                });

            var content = await cursor.ToListAsync(r => Map.Content(r["c"].As<INode>()));
            return content.First();
        }

        private async Task<bool> DeleteContent(IAsyncTransaction transaction, long id)
        {
            var cursor = await transaction.RunAsync(@"
                MATCH (c:Content)
                WHERE id(c)=$id
                WITH c, id(c) as id
                DETACH DELETE c
                RETURN id",
                new { id });

            var content = await cursor.ToListAsync(r => r["id"].As<long>());
            return content.Count > 0;
        }



        private async Task<Expression> GetExpressionById(IAsyncTransaction transaction, long id)
        {
            var cursor = await transaction.RunAsync(@"
                MATCH (c:Expression)
                WHERE id(c)=$id
                RETURN c",
                new { id });

            var content = await cursor.ToListAsync(r => Map.Expression(r["c"].As<INode>()));
            return content.FirstOrDefault();
        }

        private async Task<Expression> GetExpression(IAsyncTransaction transaction, string language, string label)
        {
            var cursor = await transaction.RunAsync(@"
                MATCH (c:Expression { language = $language, label = $label })
                RETURN c",
                new { language, label });

            var content = await cursor.ToListAsync(r => Map.Expression(r["c"].As<INode>()));
            return content.FirstOrDefault();
        }

        private async Task<Expression> CreateExpression(IAsyncTransaction transaction, NewExpression newExpression)
        {
            var cursor = await transaction.RunAsync(@"
                MERGE (e:Expression {language: $expression.language, label: $expression.label})
                RETURN e",
                new { expression = newExpression.Properties });

            var expression = await cursor.ToListAsync(r => Map.Expression(r["e"].As<INode>()));
            return expression.First();
        }

        private async Task<Expression> UpdateExpression(IAsyncTransaction transaction, UpdateExpression updateExpression)
        {
            var setQueries = new List<string>();
            if (updateExpression.Language != null)
                setQueries.Add("e.language = $language");
            if (updateExpression.Label != null)
                setQueries.Add("e.label = $label");

            var setQuery = string.Join(", ", setQueries);

            var cursor = await transaction.RunAsync($@"
                MATCH (e:Expression)
                WHERE id(e) = $id
                SET {setQuery}
                RETURN e",
                updateExpression.Properties);

            var content = await cursor.ToListAsync(r => Map.Expression(r["e"].As<INode>()));
            return content.First();
        }

        private async Task<bool> DeleteExpression(IAsyncTransaction transaction, long id)
        {
            var cursor = await transaction.RunAsync(@"
                MATCH (e:Expression)
                WHERE id(e) = $id
                WITH e, id(e) as id
                DETACH DELETE e
                RETURN id",
                new { id });

            var content = await cursor.ToListAsync(r => r["id"].As<long>());
            return content.Count > 0;
        }

        

        private async Task<Reference> GetReferenceById(IAsyncTransaction transaction, long id)
        {
            var cursor = await transaction.RunAsync(@"
                MATCH (e:Expression)-[r:REFER_TO]->(:Content)
                WHERE id(r) = $id
                RETURN r, e",
                new { id });

            var reference = await cursor.ToListAsync(r =>
                Map.Reference(r["r"].As<IRelationship>(), r["e"].As<INode>()));

            return reference.FirstOrDefault();
        }

        private async Task<Reference> CreateReference(IAsyncTransaction transaction, long contentId, long expressionId)
        {
            var cursor = await transaction.RunAsync(@"
                MATCH (c:Content), (e:Expression)
                WHERE id(c) = $content_id AND id(e) = $expression_id
                MERGE (e)-[r:REFER_TO]->(c)
                RETURN r, e",
                new { content_id = contentId, expression_id = expressionId });

            var reference = await cursor.ToListAsync(r =>
                Map.Reference(r["r"].As<IRelationship>(), r["e"].As<INode>()));

            return reference.First();
        }

        private async Task<bool> DeleteReference(IAsyncTransaction transaction, long id)
        {
            var cursor = await transaction.RunAsync(@"
                MATCH (:Expression)-[r:REFER_TO]->(:Content)
                WHERE id(r) = $id
                WITH r, id(r) as id
                DELETE r
                RETURN id",
                new { id });

            var content = await cursor.ToListAsync(r => r["id"].As<long>());
            return content.Count > 0;
        }



        private async Task Transaction(Func<IAsyncTransaction, Task> func)
        {
            var session = _driver.AsyncSession();
            var transaction = await session.BeginTransactionAsync();
            try
            {
                await func(transaction);
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        private async Task<T> Transaction<T>(Func<IAsyncTransaction, Task<T>> func)
        {
            var session = _driver.AsyncSession();
            var transaction = await session.BeginTransactionAsync();
            try
            {
                var result = await func(transaction);
                await transaction.CommitAsync();
                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
            finally
            {
                await session.CloseAsync();
            }
        }
    }
}
