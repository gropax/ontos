using FluentValidation.Results;
using Ontos.Web.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ontos.Web.Validation
{
    public static class Validator
    {
        public static ValidationResult Validate(NewPageDto newPageDto) => new NewPageValidator().Validate(newPageDto);
        public static ValidationResult Validate(UpdatePageDto updatePageDto) => new UpdatePageValidator().Validate(updatePageDto);
    }
}
