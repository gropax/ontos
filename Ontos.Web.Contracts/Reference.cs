using Ontos.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ontos.Web.Contracts
{
    public class ReferenceDto
    {
        public long Id { get; set; }
        public long PageId { get; set; }
        public ExpressionDto Expression { get; set; }

        public ReferenceDto(long id, long pageId, ExpressionDto expression)
        {
            Id = id;
            PageId = pageId;
            Expression = expression;
        }

        public ReferenceDto(Reference reference)
        {
            Id = reference.Id;
            PageId = reference.PageId;
            Expression = new ExpressionDto(reference.Expression);
        }
    }
}
