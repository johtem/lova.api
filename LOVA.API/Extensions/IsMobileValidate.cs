using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LOVA.API.Extensions
{
    public class IsMobileValidate : Attribute, IModelValidator
    {
        public string[] NotAllowed { get; set; }
        public string ErrorMessage { get; set; }
        public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
        {

            if (NotAllowed.Contains(context.Model as string))
                return new List<ModelValidationResult> {
                    new ModelValidationResult("", ErrorMessage)
                };
            else
                return Enumerable.Empty<ModelValidationResult>();
        }
    }
}