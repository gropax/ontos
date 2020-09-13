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
        Task<Paginated<Page>> GetPages(PaginationParams<PageSortKey> @params);
        Task<PageSearchResult[]> SearchPages(string language, string text);
        Task<Page> GetPage(long id);
        Task<Page> CreatePage(NewPage newPage);
        Task<Page> UpdatePage(UpdatePage updatePage);
        Task<long[]> DeletePages(params long[] ids);
        Task<Reference[]> GetPageReferences(long pageId);
        Task<Reference> CreateReference(NewReference reference);
        Task<long[]> DeleteReferences(params long[] ids);
        Task<RelatedPage[]> GetAllRelatedPages(long pageId);
        Task<Relation> CreateRelation(NewRelation relation);
        Task<long[]> DeleteRelations(params long[] ids);
    }

    public class GraphStorage : IGraphStorage
    {
        private IDriver _driver;

        private const int MAX_PATH_LENGTH = 50;

        public GraphStorage(string uri, string user, string password)
        {
            _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
        }

        public async Task CreateIndexes()
        {
            await Transaction(t =>
                t.RunAsync("CALL db.index.fulltext.createNodeIndex(\"expressionLabel\", [\"Expression\"], [\"label\"])"));
        }

        public async Task DropIndexes()
        {
            await Transaction(t =>
                t.RunAsync("CALL db.index.fulltext.drop(\"expressionLabel\")"));
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

        public async Task<PageSearchResult[]> SearchPages(string language, string text)
        {
            return await Transaction(t => SearchPages(t, language, text));
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

        public async Task<long[]> DeletePages(params long[] ids)
        {
            return await Transaction(t => DeletePages(t, ids));
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

        public async Task<Reference[]> GetPageReferences(long pageId)
        {
            return await Transaction(t => GetPageReferences(t, pageId));
        }

        public async Task<Reference> GetReference(long id)
        {
            return await Transaction(t => GetReferenceById(t, id));
        }

        public async Task<bool> ReferenceExists(long id)
        {
            return await Transaction(t => ReferenceExists(t, id));
        }

        public async Task<Reference> CreateReference(NewReference newReference)
        {
            return await Transaction(async t =>
            {
                var expression = await EnsureExpression(t, newReference.NewExpression);
                return await CreateReference(t, newReference.PageId, expression.Id);
            });
        }

        public async Task<long[]> DeleteReferences(params long[] ids)
        {
            return await Transaction(t => DeleteReferences(t, ids));
        }

        public async Task<Relation> CreateRelation(NewRelation newRelation)
        {
            bool isValid = await ValidateRelation(newRelation);
            if (isValid)
                return await Transaction(t => EnsureRelation(t, newRelation));
            else
                return null;
        }

        public async Task<bool> ValidateRelation(NewRelation newRelation)
        {
            return await Transaction(async t =>
            {
                // If acyclic, ensure no paths from target to origin
                if (newRelation.Type.Acyclic)
                {
                    var paths = await GetRelationPaths(t, newRelation.TargetId, newRelation.OriginId, newRelation.Type.Label, newRelation.Type.Directed);
                    if (paths.Length > 0)
                        return false;
                }
                return true;
            });
        }

        public async Task<RelatedPage[]> GetAllRelatedPages(long pageId)
        {
            return await Transaction(t => GetAllRelatedPages(t, pageId));
        }

        public async Task<Relation[]> GetRelationsFrom(long originId, RelationType type)
        {
            return await Transaction(t => GetRelationsFrom(t, originId, type.Label, type.Directed));
        }

        public async Task<long[]> DeleteRelations(params long[] ids)
        {
            return await Transaction(t => DeleteRelations(t, ids));
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

        private async Task<PageSearchResult[]> SearchPages(IAsyncTransaction transaction, string language, string text)
        {
            var cursor = await transaction.RunAsync($@"
                CALL db.index.fulltext.queryNodes(""expressionLabel"", $text) YIELD node, score
                WITH score, node AS e
                WHERE e.language = $language
                MATCH (p:Page)<-[:SIGNIFIED]-(r:Reference)-[:SIGNIFIER]->(e)
                RETURN p, e, score
                ORDER BY score DESC
                LIMIT 10",
                new { language, text });

            var objs = await cursor.ToListAsync(r => new
            {
                Page = r["p"].As<INode>(),
                Expression = Map.Expression(r["e"].As<INode>()),
                Score = r["score"].As<double>(),
            });

            var results = objs.GroupBy(o => o.Page)
                .Select(g =>
                {
                    var expressions = g.Select(r => r.Expression.Label).ToArray();
                    double score = g.Select(r => r.Score).Max(s => s);
                    return new PageSearchResult(
                        pageId: g.Key.Id,
                        score: score,
                        expressions: expressions);
                }).ToArray();

            return results;
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
            var map = new Dictionary<string, object>() {  };
            if (updatePage.Content != null)
                map["content"] = updatePage.Content;
            if (updatePage.Type != null)
                map["type"] = updatePage.Type.ToString();

            var cursor = await transaction.RunAsync(@"
                MATCH (p:Page)
                WHERE id(p)=$id
                SET p += $page
                RETURN p",
                new { id = updatePage.Id, page = map, });

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

        private async Task<long[]> DeletePages(IAsyncTransaction transaction, IEnumerable<long> ids)
        {
            var cursor = await transaction.RunAsync(@"
                WITH $ids AS ids
                MATCH (p:Page)
                WHERE id(p) IN ids
                OPTIONAL MATCH (p)<-[:SIGNIFIED]-(r:Reference)
                WITH p, id(p) as id, r
                DETACH DELETE p
                RETURN id, r",
                new { ids = ids.ToArray() });

            var res = await cursor.ToListAsync(r => new { PageId = r["id"].As<long>(), ReferenceId = r["r"].As<INode>()?.Id });
            if (res.Count == 0)
                return new long[0];
            else
            {
                await DeleteReferences(transaction, res.SelectValues(r => r.ReferenceId));
                return res.GroupBy(r => r.PageId).Select(g => g.Key).ToArray();
            }
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

        private async Task<long[]> DeleteOrphanExpressions(IAsyncTransaction transaction, IEnumerable<long> ids)
        {
            var cursor = await transaction.RunAsync(@"
                WITH $ids AS ids
                MATCH (e:Expression)
                WHERE id(e) IN ids AND NOT (e)<-[:SIGNIFIER]-(:Reference)
                WITH e, id(e) as id
                DETACH DELETE e
                RETURN id",
                new { ids = ids.ToArray() });

            var res = await cursor.ToListAsync(r => r["id"].As<long>());

            return res.ToArray();
        }

        

        private async Task<Reference[]> GetPageReferences(IAsyncTransaction transaction, long pageId)
        {
            var cursor = await transaction.RunAsync(@"
                MATCH (p:Page)<-[:SIGNIFIED]-(r:Reference)-[:SIGNIFIER]->(e:Expression)
                WHERE id(p) = $pageId
                RETURN id(p), r, e",
                new { pageId });

            var references = await cursor.ToListAsync(r =>
                Map.Reference(r["r"].As<INode>(), r["id(p)"].As<long>(), r["e"].As<INode>()));

            return references.ToArray();
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

        private async Task<bool> ReferenceExists(IAsyncTransaction transaction, long id)
        {
            var cursor = await transaction.RunAsync(@"
                MATCH (r:Reference)
                WHERE id(r) = $id
                RETURN id(r)",
                new { id });

            var reference = await cursor.ToListAsync(r => r["id(r)"].As<long>());

            return reference.Count > 0;
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

        private async Task<long[]> DeleteReferences(IAsyncTransaction transaction, IEnumerable<long> ids)
        {
            var cursor = await transaction.RunAsync(@"
                WITH $ids AS ids
                MATCH (r:Reference)-[:SIGNIFIER]->(e:Expression)
                WHERE id(r) IN ids
                WITH r, id(r) as id, e
                DETACH DELETE r
                RETURN id, e",
                new { ids = ids.ToArray() });

            var res = await cursor.ToListAsync(r => new { ReferenceId = r["id"].As<long>(), ExpressionId = r["e"].As<INode>()?.Id });
            if (res.Count == 0)
                return new long[0];
            else
            {
                await DeleteOrphanExpressions(transaction, res.SelectValues(r => r.ExpressionId));
                return res.GroupBy(r => r.ReferenceId).Select(g => g.Key).ToArray();
            }
        }

        private async Task<PagePath[]> GetRelationPaths(IAsyncTransaction transaction, long originId, long targetId, string relationType = null, bool directed = true)
        {
            string relationQuery = string.IsNullOrWhiteSpace(relationType) ? string.Empty : ":" + relationType;
            string direction = directed ? ">" : string.Empty;

            var cursor = await transaction.RunAsync($@"
                MATCH p=(o:Page)-[r$rel_query*1..{MAX_PATH_LENGTH}]-{direction}(t:Page)
                WHERE id(o) = $origin_id AND id(t) = $target_id
                RETURN p",
                new { origin_id = originId, target_id = targetId, rel_query = relationQuery }); 

            var paths = await cursor.ToListAsync(r => Map.PagePath(r["p"].As<IPath>()));

            return paths.ToArray();
        }

        private async Task<RelatedPage[]> GetAllRelatedPages(IAsyncTransaction transaction, long pageId)
        {
            var relationQuery = string.Join("|", RelationType.Labels);
            var cursor = await transaction.RunAsync($@"
                MATCH (p:Page)-[r:{relationQuery}]-(q:Page)
                WHERE id(p) = $page_id
                OPTIONAL MATCH (q)<-[:SIGNIFIED]-(ref:Reference)-[:SIGNIFIER]->(e:Expression)
                RETURN r, q, ref, e",
                new { page_id = pageId });

            var results = await cursor.ToListAsync(r => new
            {
                Relationship = r["r"].As<IRelationship>(),
                OtherPage = r["q"].As<INode>(),
                Reference = r["ref"].As<INode>(),
                Expression = r["e"].As<INode>(),
            });

            var relations = results
                .GroupBy(r => r.Relationship)
                .Select(g =>
                {
                    var page = Map.Page(g.First().OtherPage);
                    var references = g.Select(o => Map.Reference(o.Reference, page.Id, o.Expression)).ToArray();
                    return new RelatedPage(
                        g.Key.Id,
                        RelationType.Parse(g.Key.Type),
                        pageId,
                        new Page(page, references));
                }).ToArray();

            return relations.ToArray();
        }

        private async Task<Relation[]> GetRelationsFrom(IAsyncTransaction transaction, long originId, string relationType = null, bool directed = true)
        {
            string relationQuery = string.IsNullOrWhiteSpace(relationType) ? string.Empty : ":" + relationType;
            string direction = directed ? ">" : string.Empty;

            var cursor = await transaction.RunAsync($@"
                MATCH (o:Page)-[r{relationQuery}]-{direction}(t:Page)
                WHERE id(o) = $origin_id
                RETURN r",
                new { origin_id = originId, rel_query = relationQuery }); 

            var relations = await cursor.ToListAsync(r => Map.Relation(r["r"].As<IRelationship>()));

            return relations.ToArray();
        }

        private async Task<Relation> EnsureRelation(IAsyncTransaction transaction, NewRelation newRelation)
        {
            var cursor = await transaction.RunAsync($@"
                MATCH (o:Page), (t:Page)
                WHERE id(o) = $origin_id AND id(t) = $target_id
                MERGE (o)-[r:{newRelation.Type.Label}]-(t)
                RETURN r",
                newRelation.Properties);

            var relation = await cursor.ToListAsync(r => Map.Relation(r["r"].As<IRelationship>()));

            return relation.First();
        }

        private async Task<long[]> DeleteRelations(IAsyncTransaction transaction, params long[] ids)
        {
            var cursor = await transaction.RunAsync(@"
                WITH $ids AS ids
                MATCH (:Page)-[r]->(:Page)
                WHERE id(r) IN ids
                WITH r, id(r) as id
                DELETE r
                RETURN id",
                new { ids = ids.ToArray() });

            var res = await cursor.ToListAsync(r => r["id"].As<long>());

            return res.ToArray();
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
