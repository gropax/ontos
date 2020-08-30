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
        public long Id { get; set; }
        public string[] Contexts { get; set; } = new string[0];
        public Expression Expression { get; set; }

        public Reference(long id, string[] contexts, Expression expression)
        {
            Id = id;
            Contexts = contexts;
            Expression = expression;
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
        public long ContentId { get; }
        public string Language { get; }
        public string Label { get; }
        public NewExpression NewExpression => new NewExpression(Language, Label);
        public object Properties => new
        {
            content_id = ContentId,
            expression = new { language = Language, label = Label },
        };

        public NewReference(long contentId, string language, string label)
        {
            ContentId = contentId;
            Language = language;
            Label = label;
        }
    }
}
