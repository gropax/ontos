using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ontos.Contracts
{
    public interface IRelation
    {
        int Id { get; }
        string Label { get; }
        int OriginId { get; }
        int TargetId { get; }
    }

    /// <summary>
    /// WARNING: If Relation is to be dynamically created by user, beware to CYPHER injection in queries as 
    /// `Type.Label` is directly copied into CYPHER query without safety checks.
    /// </summary>
    public class Relation
    {
        public long Id { get; set; }
        public RelationType Type { get; set; }
        public long OriginId { get; set; }
        public long TargetId { get; set; }

        public Relation(long id, RelationType type, long originId, long targetId)
        {
            Id = id;
            Type = type;
            OriginId = originId;
            TargetId = targetId;
        }

        #region Equality methods
        public override bool Equals(object obj)
        {
            return obj is Relation relation &&
                   Id == relation.Id &&
                   EqualityComparer<RelationType>.Default.Equals(Type, relation.Type) &&
                   OriginId == relation.OriginId &&
                   TargetId == relation.TargetId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Type, OriginId, TargetId);
        }
        #endregion
    }

    public class RelatedPage
    {
        public long Id { get; set; }
        public RelationType Type { get; set; }
        public long OriginId { get; set; }
        public Page Target { get; set; }

        public RelatedPage(long id, RelationType type, long originId, Page target)
        {
            Id = id;
            Type = type;
            OriginId = originId;
            Target = target;
        }

        #region Equality methods
        public override bool Equals(object obj)
        {
            return obj is RelatedPage relation &&
                   Id == relation.Id &&
                   EqualityComparer<RelationType>.Default.Equals(Type, relation.Type) &&
                   OriginId == relation.OriginId &&
                   EqualityComparer<Page>.Default.Equals(Target, relation.Target);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Type, OriginId, Target);
        }
        #endregion
    }

    public class NewRelation
    {
        public RelationType Type { get; set; }
        public long OriginId { get; set; }
        public long TargetId { get; set; }
        public object Properties => new { origin_id = OriginId, target_id = TargetId };

        public NewRelation(RelationType type, long originId, long targetId)
        {
            Type = type;
            OriginId = originId;
            TargetId = targetId;
        }
    }

    public class RelationType
    {
        public string Label { get; }
        public bool Directed { get; }
        public bool Acyclic { get; }

        public RelationType(string label, bool directed, bool acyclic)
        {
            Label = label;
            Directed = directed;
            Acyclic = acyclic;
        }


        public static readonly RelationType Inclusion = new RelationType("INCLUDE", directed: true, acyclic: true);
        public static readonly RelationType Intersection = new RelationType("INTERSECT", directed: false, acyclic: false);

        public static readonly RelationType[] All = new[]
        {
            Inclusion, Intersection,
        };

        public static readonly string[] Labels = All.Select(r => r.Label).ToArray();

        private static readonly Dictionary<string, RelationType> _typesByLabel = All.ToDictionary(t => t.Label);

        public static bool TryParse(string s, out RelationType relationType)
        {
            return _typesByLabel.TryGetValue(s, out relationType);
        }

        public static RelationType Parse(string s)
        {
            if (_typesByLabel.TryGetValue(s, out var relationType))
                return relationType;
            else
                throw new ArgumentException($"Invalid relation type [{s}].");
        }

        public override bool Equals(object obj)
        {
            return obj is RelationType type && Label == type.Label;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Label);
        }
    }
}
