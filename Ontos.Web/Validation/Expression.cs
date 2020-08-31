using FluentValidation;
using Ontos.Web.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ontos.Web.Validation
{
    public class NewExpressionValidator : AbstractValidator<NewExpressionDto>
    {
        public NewExpressionValidator()
        {
            RuleFor(x => x.Language).NotEmpty();
            RuleFor(x => x.Label).NotEmpty();
        }
    }

}
