using Neo4j.Driver;
using Ontos.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ontos.Storage
{
    public static class Map
    {
        public static Page Page(INode node)
        {
            return new Page(
                node.Id,
                node["content"].As<string>(),
                Enum.Parse<PageType>(node["type"].As<string>()),
                DateTime.Parse(node["created_at"].As<string>()).ToUniversalTime(),
                DateTime.Parse(node["updated_at"].As<string>()).ToUniversalTime());
        }

        public static Expression Expression(INode node)
        {
            return new Expression(
                node.Id,
                node["language"].As<string>(),
                node["label"].As<string>());
        }

        public static Reference Reference(INode reference, long pageId, INode expression)
        {
            return new Reference(
                reference.Id,
                pageId,
                Map.Expression(expression));
        }

        public static Relation Relation(IRelationship relationship)
        {
            return new Relation(
                relationship.Id,
                RelationType.Parse(relationship.Type),
                relationship.StartNodeId,
                relationship.EndNodeId);
        }

        public static PagePath PagePath(IPath path)
        {
            return new PagePath(
                path.Nodes.Select(n => Page(n)).ToArray(),
                path.Relationships.Select(r => Relation(r)).ToArray());
        }
    }
}
