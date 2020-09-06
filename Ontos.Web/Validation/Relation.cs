using FluentValidation;
using Ontos.Contracts;
using Ontos.Web.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ontos.Web.Validation
{
    public class NewRelationValidator : AbstractValidator<NewRelationDto>
    {
        public NewRelationValidator()
        {
            RuleFor(x => x.Type)
                .Must(type => RelationType.Labels.Contains(type))
                .WithMessage(x => $"Invalid relation type [{x.Type}].");
        }
    }
}
