﻿using Ontos.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ontos.Web.Contracts
{
    public class RelationDto
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public long OriginId { get; set; }
        public long TargetId { get; set; }

        public RelationDto(long id, string type, long originId, long targetId)
        {
            Id = id;
            Type = type;
            OriginId = originId;
            TargetId = targetId;
        }

        public RelationDto(Relation relation)
        {
            Id = relation.Id;
            Type = relation.Type.Label;
            OriginId = relation.OriginId;
            TargetId = relation.TargetId;
        }
    }

    public class RelatedPageDto
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public long OriginId { get; set; }
        public PageDto Target { get; set; }

        public RelatedPageDto(long id, string type, long originId, PageDto target)
        {
            Id = id;
            Type = type;
            OriginId = originId;
            Target = target;
        }

        public RelatedPageDto(RelatedPage relatedPage)
        {
            Id = relatedPage.Id;
            Type = relatedPage.Type.Label;
            OriginId = relatedPage.OriginId;
            Target = new PageDto(relatedPage.Target);
        }
    }

    public class NewRelationDto
    {
        public string Type { get; set; }
        public long OriginId { get; set; }
        public long TargetId { get; set; }

        public NewRelation ToModel()
        {
            return new NewRelation(RelationType.Parse(Type), OriginId, TargetId);
        }
    }

    public class NewRelatedPageDto
    {
        public string Type { get; set; }
        public bool Reversed { get; set; }
        public NewExpressionDto Expression { get; set; }
        public string Content { get; set; }

        public NewPage GetNewPage()
        {
            return new NewPage(Content, Expression.ToModel());
        }

        public NewRelation GetNewRelation(long originId, long targetId)
        {
            long realOriginId, realTargetId;
            if (Reversed)
            {
                realOriginId = targetId;
                realTargetId = originId;
            }
            else
            {
                realOriginId = originId;
                realTargetId = targetId;
            }

            return new NewRelation(RelationType.Parse(Type), realOriginId, realTargetId);
        }
    }
}
