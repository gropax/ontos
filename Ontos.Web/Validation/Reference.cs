using FluentValidation;
using Ontos.Web.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ontos.Web.Validation
{
    public class NewReferenceValidator : AbstractValidator<NewReferenceDto>
    {
        public NewReferenceValidator()
        {
            RuleFor(x => x.Expression).SetValidator(new NewExpressionValidator());
        }
    }

}
