using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Haystack.ComponentModel.DataAnnotations
{
    public class CompositeValidationResult :
        ValidationResult
    {
        private readonly List<ValidationResult> _results = new List<ValidationResult>();
        public IEnumerable<ValidationResult> Results => _results.AsReadOnly();

        public void Add(ValidationResult validationResult)
            => _results.Add(validationResult);

        public void AddRange(IEnumerable<ValidationResult> validationResults)
            => _results.AddRange(validationResults);

        protected CompositeValidationResult(ValidationResult validationResult) :
            base(validationResult) {}

        public CompositeValidationResult(string errorMessage) :
            base(errorMessage) {}

        public CompositeValidationResult(string errorMessage, IEnumerable<string> memberNames) :
            base(errorMessage, memberNames) {}
    }
}