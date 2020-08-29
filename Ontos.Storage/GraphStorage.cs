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

        public async Task<Content> GetContent(IAsyncTransaction transaction, long id)
        {
            var cursor = await transaction.RunAsync(@"
                MATCH (c:Content)
                WHERE id(c)=$id
                RETURN c",
                new { id });

            var content = await cursor.ToListAsync(r => new Content(r["c"].As<INode>()));
            return content.FirstOrDefault();
        }

        public async Task<Content> CreateContent(IAsyncTransaction transaction, NewContent newContent)
        {
            var cursor = await transaction.RunAsync(@"
                CREATE (c:Content $content)
                RETURN c",
                new { content = new { details = newContent.Details } });

            var content = await cursor.ToListAsync(r => new Content(r["c"].As<INode>()));
            return content.First();
        }

        public async Task<Content> UpdateContent(IAsyncTransaction transaction, UpdateContent updateContent)
        {
            var cursor = await transaction.RunAsync(@"
                MATCH (c:Content)
                WHERE id(c)=$id
                SET c = $content
                RETURN c",
                new
                {
                    id = updateContent.Id,
                    content = new { details = updateContent.Details },
                });

            var content = await cursor.ToListAsync(r => new Content(r["c"].As<INode>()));
            return content.First();
        }

        public async Task<bool> DeleteContent(IAsyncTransaction transaction, long id)
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
        public NewContent(string details)
        {
            Details = details;
        }
    }

    public class UpdateContent
    {
        public long Id { get; }
        public string Details { get; }
        public UpdateContent(long id, string details)
        {
            Id = id;
            Details = details;
        }
    }

    public class Content
    {
        public long Id { get; set; }
        public string Details { get; set; }

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
        public string Language { get; set; }
        public string Label { get; set; }
    }

    /// <summary>
    /// Represent a denotation relation between an Expression and a Content, in zero or many specific Contexts
    /// </summary>
    public class Reference
    {
        public string[] Contexts { get; set; }
    }
}
