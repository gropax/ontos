using Neo4j.Driver;
using Ontos.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ontos.Storage
{
    public static class Map
    {
        public static Content Content(INode node)
        {
            return new Content(
                node.Id,
                node["details"].As<string>());
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
