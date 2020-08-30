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
        }
    }

    public class UpdatePageValidator : AbstractValidator<UpdatePageDto>
    {
        public UpdatePageValidator()
        {
            RuleFor(x => x.Content).NotEmpty();
        }
    }
}
