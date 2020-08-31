using Ontos.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ontos.Web.Contracts
{
    public class ExpressionDto
    {
        public long Id { get; set; }
        public string Language { get; set; }
        public string Label { get; set; }

        public ExpressionDto(long id, string language, string label)
        {
            Id = id;
            Language = language;
            Label = label;
        }

        public ExpressionDto(Expression expression)
        {
            Id = expression.Id;
            Language = expression.Language;
            Label = expression.Label;
        }
    }

    public class NewExpressionDto
    {
        public string Language { get; set; }
        public string Label { get; set; }

        public NewExpression ToModel()
        {
            return new NewExpression(Language, Label);
        }
    }

}
