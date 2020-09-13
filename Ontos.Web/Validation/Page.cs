using FluentValidation;
using Ontos.Contracts;
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
            // Update can't be entirely empty (do nothing), but some fields might be.
            RuleFor(x => x).Must(x => !IsEmpty(x)).WithMessage("Update can't be empty");
            RuleFor(x => x.Type)
                .IsEnumName(typeof(PageType), caseSensitive: false)
                .When(x => !string.IsNullOrEmpty(x.Type));
        }

        private bool IsEmpty(UpdatePageDto x)
        {
            return string.IsNullOrEmpty(x.Content)
                && string.IsNullOrEmpty(x.Type);
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
