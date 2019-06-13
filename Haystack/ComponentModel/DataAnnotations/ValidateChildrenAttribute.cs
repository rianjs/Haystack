using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Haystack.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Tells the Validator that the children underneath the property needs to be validated. You can optionally supply a `ValidationAttribute` type  to apply to
    /// the children being validated. Works for properties that are collections of objects, and properties that are single objects. 
    /// </summary>
    public class ValidateChildrenAttribute :
        ValidationAttribute
    {
        public Type ValidationType { get; set; }

        private IEnumerable<ValidationAttribute> GetValidations()
        {
            if (ValidationType == null)
            {
                yield break;
            }

            yield return (ValidationAttribute) Activator.CreateInstance(ValidationType);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (!(value is IEnumerable))
            {
                throw new ArgumentException($"Unrecognized collection type: {value.GetType()}");
            }
            
            var enumerable = (IEnumerable) value;

            var parentSuppliedValidator = ValidationType != null;
            var aggregateResults = new List<ValidationResult>();

            foreach (var element in enumerable)
            {
                var subResults = new List<ValidationResult>();
                var context = new ValidationContext(element, serviceProvider: null, items: null);
            
                if (parentSuppliedValidator)
                {
                    Validator.TryValidateValue(element, context, subResults, GetValidations());
                }
                else
                {
                    Validator.TryValidateObject(element, context, subResults, validateAllProperties: true);
                }
            
                aggregateResults.AddRange(subResults);
            }
           

            if (aggregateResults.Count < 1)
            {
                return ValidationResult.Success;
            }

            var compositeResults = new CompositeValidationResult($"Validation for {validationContext.DisplayName} failed!");
            compositeResults.AddRange(aggregateResults);
            return compositeResults;
        }
    }
    
    
}