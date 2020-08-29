using Neo4j.Driver;
using Newtonsoft.Json;
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

        public async Task<Content> CreateContent(NewContent newContent)
        {
            var session = _driver.AsyncSession();

            try
            {
                var cursor = await session.RunAsync(@"
                    CREATE (c:Content $content)
                    RETURN c",
                    new { content = new { details = newContent.Description } });

                var content = await cursor.ToListAsync(r =>
                {
                    var node = r["c"].As<INode>();
                    return new Content()
                    {
                        Id = node.Id,
                        Details = node["details"].As<string>()
                    };
                });

                return content.First();
            }
            catch
            {
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
