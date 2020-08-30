using System;
using System.Collections.Generic;
using System.Text;

namespace Ontos.Contracts
{
    /// <summary>
    /// In relation with :
    ///   - Content through a Reference relation
    ///   - Expression through a Translation relation
    /// </summary>
    public class Expression
    {
        public long Id { get; }
        public string Language { get; }
        public string Label { get; }

        public Expression(long id, string language, string label)
        {
            Id = id;
            Language = language;
            Label = label;
        }

        #region Equality methods
        public override bool Equals(object obj)
        {
            return obj is Expression expression &&
                   Id == expression.Id &&
                   Language == expression.Language &&
                   Label == expression.Label;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Language, Label);
        }
        #endregion
    }

    public class NewExpression
    {
        public string Language { get; }
        public string Label { get; }
        public object Properties => new { language = Language, label = Label };
        public NewExpression(string language, string label)
        {
            Language = language;
            Label = label;
        }
    }

    public class UpdateExpression
    {
        public long Id { get; }
        public string Language { get; }
        public string Label { get; }
        public object Properties => new { id = Id, language = Language, label = Label };
        public UpdateExpression(long id, string language = null, string label = null)
        {
            Id = id;
            Language = language;
            Label = label;
        }
    }
}
