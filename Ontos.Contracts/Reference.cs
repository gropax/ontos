using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ontos.Contracts
{
    /// <summary>
    /// Represent a denotation relation between an Expression and a Content, in zero or many specific Contexts
    /// </summary>
    public class Reference
    {
        public long Id { get; }
        public string[] Contexts { get; }
        public long PageId { get; }
        public Expression Expression { get; }

        public Reference(long id, long pageId, Expression expression, string[] contexts = null)
        {
            Id = id;
            PageId = pageId;
            Expression = expression;
            Contexts = contexts ?? new string[0];
        }

        #region Equality methods
        public override bool Equals(object obj)
        {
            return obj is Reference reference &&
                   Id == reference.Id &&
                   Enumerable.SequenceEqual(Contexts, reference.Contexts) &&
                   EqualityComparer<Expression>.Default.Equals(Expression, reference.Expression);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Contexts, Expression);
        }
        #endregion
    }

    public class NewReference
    {
        public long PageId { get; }
        public string Language { get; }
        public string Label { get; }
        public NewExpression NewExpression => new NewExpression(Language, Label);
        public object Properties => new
        {
            content_id = PageId,
            expression = new { language = Language, label = Label },
        };

        public NewReference(long pageId, string language, string label)
        {
            PageId = pageId;
            Language = language;
            Label = label;
        }
    }
}
