using Neo4j.Driver;
using Ontos.Contracts;
using System;
using System.Collections.Generic;
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

        public static Reference Reference(IRelationship reference, INode expression)
        {
            return new Reference(
                reference.Id,
                new string[0],
                Map.Expression(expression));
        }
    }
}
