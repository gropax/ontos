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
        public static ValidationResult Validate(SearchPageDto searchPageDto) => new SearchPageValidator().Validate(searchPageDto);
        public static ValidationResult Validate(NewReferenceDto newReferenceDto) => new NewReferenceValidator().Validate(newReferenceDto);
        public static ValidationResult Validate(NewRelationDto newRelationDto) => new NewRelationValidator().Validate(newRelationDto);
    }
}
