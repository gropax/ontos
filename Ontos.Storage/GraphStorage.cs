using Neo4j.Driver;
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
            return await Transaction(t => GetExpression(t, id));
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


        private async Task<Content> GetContent(IAsyncTransaction transaction, long id)
        {
            var cursor = await transaction.RunAsync(@"
                MATCH (c:Content)
                WHERE id(c)=$id
                RETURN c",
                new { id });

            var content = await cursor.ToListAsync(r => new Content(r["c"].As<INode>()));
            return content.FirstOrDefault();
        }

        private async Task<Content> CreateContent(IAsyncTransaction transaction, NewContent newContent)
        {
            var cursor = await transaction.RunAsync(@"
                CREATE (c:Content $content)
                RETURN c",
                new { content = newContent.Properties });

            var content = await cursor.ToListAsync(r => new Content(r["c"].As<INode>()));
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

            var content = await cursor.ToListAsync(r => new Content(r["c"].As<INode>()));
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



        private async Task<Expression> GetExpression(IAsyncTransaction transaction, long id)
        {
            var cursor = await transaction.RunAsync(@"
                MATCH (c:Expression)
                WHERE id(c)=$id
                RETURN c",
                new { id });

            var content = await cursor.ToListAsync(r => new Expression(r["c"].As<INode>()));
            return content.FirstOrDefault();
        }

        private async Task<Expression> CreateExpression(IAsyncTransaction transaction, NewExpression newExpression)
        {
            var cursor = await transaction.RunAsync(@"
                CREATE (e:Expression $expression)
                RETURN e",
                new { expression = newExpression.Properties });

            var expression = await cursor.ToListAsync(r => new Expression(r["e"].As<INode>()));
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

            var content = await cursor.ToListAsync(r => new Expression(r["e"].As<INode>()));
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

    public class NewContent
    {
        public string Details { get; }
        public object Properties => new { details = Details };
        public NewContent(string details)
        {
            Details = details;
        }
    }

    public class UpdateContent
    {
        public long Id { get; }
        public string Details { get; }
        public object Properties => new { details = Details };
        public UpdateContent(long id, string details)
        {
            Id = id;
            Details = details;
        }
    }

    public class NewExpression
    {
        public string Language { get; }
        public string Label { get; }
        public object Properties => new { language = Language, label = Label };
        public NewExpression(string language, string label)
        {
            Language = language;
            Label = label;
        }
    }

    public class UpdateExpression
    {
        public long Id { get; }
        public string Language { get; }
        public string Label { get; }
        public object Properties => new { id = Id, language = Language, label = Label };
        public UpdateExpression(long id, string language = null, string label = null)
        {
            Id = id;
            Language = language;
            Label = label;
        }
    }

    public class Content
    {
        public long Id { get; }
        public string Details { get; }

        public Content(long id, string details)
        {
            Id = id;
            Details = details;
        }

        public Content(INode node)
        {
            Id = node.Id;
            Details = node["details"].As<string>();
        }

        #region Equality methods
        public override bool Equals(object obj)
        {
            return obj is Content content &&
                   Id == content.Id &&
                   Details == content.Details;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Details);
        }
        #endregion
    }

    public class Relation
    {
        public int Id { get; set; }
        public string Details { get; set; }
    }

    /// <summary>
    /// In relation with :
    ///   - Content through a Reference relation
    ///   - Expression through a Translation relation
    /// </summary>
    public class Expression
    {
        public long Id { get; }
        public string Language { get; }
        public string Label { get; }

        public Expression(long id, string language, string label)
        {
            Id = id;
            Language = language;
            Label = label;
        }

        public Expression(INode node)
        {
            Id = node.Id;
            Language = node["language"].As<string>();
            Label = node["label"].As<string>();
        }

        #region Equality methods
        public override bool Equals(object obj)
        {
            return obj is Expression expression &&
                   Id == expression.Id &&
                   Language == expression.Language &&
                   Label == expression.Label;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Language, Label);
        }
        #endregion
    }

    /// <summary>
    /// Represent a denotation relation between an Expression and a Content, in zero or many specific Contexts
    /// </summary>
    public class Reference
    {
        public string[] Contexts { get; set; }
    }
}
