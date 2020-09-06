using FluentValidation;
using Ontos.Web.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Ontos.Web.Validation
{
    public class NewPageValidator : AbstractValidator<NewPageDto>
    {
        public NewPageValidator()
        {
            RuleFor(x => x.Content).NotEmpty();
            RuleFor(x => x.Expression).SetValidator(new NewExpressionValidator())
                .When(x => x.Expression != null);
        }
    }

    public class UpdatePageValidator : AbstractValidator<UpdatePageDto>
    {
        public UpdatePageValidator()
        {
            RuleFor(x => x.Content).NotEmpty();
        }
    }

    public class SearchPageValidator : AbstractValidator<SearchPageDto>
    {
        public SearchPageValidator()
        {
            RuleFor(x => x.Language).NotEmpty();
            RuleFor(x => x.Text).MinimumLength(2);
        }
    }
}
