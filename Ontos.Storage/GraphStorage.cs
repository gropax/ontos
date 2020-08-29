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

        public async Task<Content> CreateContentAsync(NewContent newContent)
        {
            return await Transaction(t => CreateContent(t, newContent));
        }

        public async Task<Content> CreateContent(IAsyncTransaction transaction, NewContent newContent)
        {
            var cursor = await transaction.RunAsync(@"
                CREATE (c:Content $content)
                RETURN c",
                new { content = new { details = newContent.Description } });

            var content = await cursor.ToListAsync(r => new Content(r["c"].As<INode>()));
            return content.First();
        }
    }

    public class NewContent
    {
        public string Description { get; }
        public NewContent(string content)
        {
            Description = content;
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
