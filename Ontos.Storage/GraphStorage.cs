﻿using Neo4j.Driver;
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
        Task<Paginated<Page>> GetPages(PaginationParams<PageSortKey> @params);
        Task<Page> GetPage(long id);
        Task<Page> CreatePage(NewPage newPage);
        Task<Page> UpdatePage(UpdatePage updatePage);
        Task<bool> DeletePage(long id);
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

        public async Task<Paginated<Page>> GetPages(PaginationParams<PageSortKey> @params)
        {
            return await Transaction(async t =>
            {
                var total = await CountPages(t);
                var pages = await GetPages(t, @params);
                return Paginated<Page>.FromParams(@params, total, pages);
            });
        }

        public async Task<Page> GetPage(long id)
        {
            return await Transaction(t => GetPage(t, id));
        }

        public async Task<Page> CreatePage(NewPage newPage)
        {
            return await Transaction(async t =>
            {
                var page = await CreatePage(t, newPage);
                if (newPage.Expression == null)
                    return page;
                else
                {
                    var expression = await EnsureExpression(t, newPage.Expression);
                    var reference = await CreateReference(t, page.Id, expression.Id);
                    return new Page(page, new[] { reference });
                }
            });
        }

        public async Task<Page> UpdatePage(UpdatePage updatePage)
        {
            return await Transaction(t => UpdatePage(t, updatePage));
        }

        public async Task<bool> DeletePage(long id)
        {
            return await Transaction(t => DeletePage(t, id));
        }

        public async Task<Expression> GetExpression(long id)
        {
            return await Transaction(t => GetExpressionById(t, id));
        }

        public async Task<Expression> CreateExpression(NewExpression newExpression)
        {
            return await Transaction(t => EnsureExpression(t, newExpression));
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
                var expression = await EnsureExpression(t, newReference.NewExpression);
                return await CreateReference(t, newReference.PageId, expression.Id);
            });
        }

        public async Task<bool> DeleteReference(long id)
        {
            return await Transaction(t => DeleteReference(t, id));
        }


        private async Task<long> CountPages(IAsyncTransaction transaction)
        {
            var cursor = await transaction.RunAsync($@"
                MATCH (:Page)
                RETURN count(*) as count");

            var count = await cursor.ToListAsync(r => r["count"].As<long>());
            return count.First();
        }

        private async Task<Page[]> GetPages(IAsyncTransaction transaction, PaginationParams<PageSortKey> p)
        {
            var sortKey = GetSortKey(p.SortColumn);
            var desc = p.SortDirection == SortDirection.DESC ? "DESC" : "";

            var cursor = await transaction.RunAsync($@"
                MATCH (p:Page)
                RETURN p
                ORDER BY p.{sortKey} {desc}
                SKIP {p.Skip}
                LIMIT {p.PageSize}");

            var pages = await cursor.ToListAsync(r => Map.Page(r["p"].As<INode>()));
            return pages.ToArray();
        }

        private string GetSortKey(PageSortKey sortKey)
        {
            switch (sortKey)
            {
                case PageSortKey.CreatedAt:
                    return "created_at";
                case PageSortKey.UpdatedAt:
                    return "updated_at";
                default:
                    throw new NotImplementedException($"Unsupported page sort key [{sortKey}].");
            }
        }

        private async Task<Page> GetPage(IAsyncTransaction transaction, long id)
        {
            var cursor = await transaction.RunAsync(@"
                MATCH (p:Page)
                WHERE id(p)=$id
                OPTIONAL MATCH (p)<-[:SIGNIFIED]-(r:Reference)-[:SIGNIFIER]->(e:Expression)
                RETURN p, r, e",
                new { id });

            var record = await cursor.PeekAsync();
            if (record == null)
                return null;

            var page = Map.Page(record["p"].As<INode>());
            if (record["r"] == null)
                return page;
            else
            {
                var references = await cursor.ToListAsync(r => Map.Reference(r["r"].As<INode>(), page.Id, r["e"].As<INode>()));
                return new Page(page, references.ToArray());
            }
        }

        private async Task<Page> CreatePage(IAsyncTransaction transaction, NewPage newPage)
        {
            var cursor = await transaction.RunAsync(@"
                CREATE (p:Page $page)
                RETURN p",
                new { page = newPage.Properties });

            var page = await cursor.ToListAsync(r => Map.Page(r["p"].As<INode>()));
            return page.First();
        }

        private async Task<Page> UpdatePage(IAsyncTransaction transaction, UpdatePage updatePage)
        {
            var cursor = await transaction.RunAsync(@"
                MATCH (p:Page)
                WHERE id(p)=$id
                SET p += $page
                RETURN p",
                new
                {
                    id = updatePage.Id,
                    page = updatePage.Properties,
                });

            var page = await cursor.ToListAsync(r => Map.Page(r["p"].As<INode>()));
            return page.First();
        }

        private async Task<bool> DeletePage(IAsyncTransaction transaction, long id)
        {
            var cursor = await transaction.RunAsync(@"
                MATCH (p:Page)
                WHERE id(p)=$id
                WITH p, id(p) as id
                DETACH DELETE p
                RETURN id",
                new { id });

            var page = await cursor.ToListAsync(r => r["id"].As<long>());
            return page.Count > 0;
        }



        private async Task<Expression> GetExpressionById(IAsyncTransaction transaction, long id)
        {
            var cursor = await transaction.RunAsync(@"
                MATCH (e:Expression)
                WHERE id(e)=$id
                RETURN e",
                new { id });

            var expression = await cursor.ToListAsync(r => Map.Expression(r["e"].As<INode>()));
            return expression.FirstOrDefault();
        }

        private async Task<Expression> GetExpression(IAsyncTransaction transaction, string language, string label)
        {
            var cursor = await transaction.RunAsync(@"
                MATCH (e:Expression { language = $language, label = $label })
                RETURN e",
                new { language, label });

            var expression = await cursor.ToListAsync(r => Map.Expression(r["e"].As<INode>()));
            return expression.FirstOrDefault();
        }

        private async Task<Expression> EnsureExpression(IAsyncTransaction transaction, NewExpression newExpression)
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

            var expression = await cursor.ToListAsync(r => Map.Expression(r["e"].As<INode>()));
            return expression.First();
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

            var ids = await cursor.ToListAsync(r => r["id"].As<long>());
            return ids.Count > 0;
        }

        

        private async Task<Reference> GetReferenceById(IAsyncTransaction transaction, long id)
        {
            var cursor = await transaction.RunAsync(@"
                MATCH (p:Page)<-[:SIGNIFIED]-(r:Reference)-[:SIGNIFIER]->(e:Expression)
                WHERE id(r) = $id
                RETURN id(p), r, e",
                new { id });

            var reference = await cursor.ToListAsync(r =>
                Map.Reference(r["r"].As<INode>(), r["id(p)"].As<long>(), r["e"].As<INode>()));

            return reference.FirstOrDefault();
        }

        private async Task<Reference> CreateReference(IAsyncTransaction transaction, long pageId, long expressionId)
        {
            var cursor = await transaction.RunAsync(@"
                MATCH (p:Page), (e:Expression)
                WHERE id(p) = $page_id AND id(e) = $expression_id
                CREATE (p)<-[:SIGNIFIED]-(r:Reference)-[:SIGNIFIER]->(e)
                RETURN r, e",
                new { page_id = pageId, expression_id = expressionId });

            var reference = await cursor.ToListAsync(r =>
                Map.Reference(r["r"].As<INode>(), pageId, r["e"].As<INode>()));

            return reference.First();
        }

        private async Task<bool> DeleteReference(IAsyncTransaction transaction, long id)
        {
            var cursor = await transaction.RunAsync(@"
                MATCH (r:Reference)
                WHERE id(r) = $id
                WITH r, id(r) as id
                DETACH DELETE r
                RETURN id",
                new { id });

            var reference = await cursor.ToListAsync(r => r["id"].As<long>());
            return reference.Count > 0;
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
