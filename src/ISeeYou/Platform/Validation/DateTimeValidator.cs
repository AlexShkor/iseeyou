using System;
using System.Linq.Expressions;
using FluentValidation.Validators;
using ISeeYou.Platform.Extensions;

namespace ISeeYou.Platform.Validation
{
    public class DateTimeValidator: PropertyValidator
    {
        public DateTimeValidator():this(() => "Can't parse date.")
        {
            
        }

        public DateTimeValidator(string errorMessageResourceName, Type errorMessageResourceType) : base(errorMessageResourceName, errorMessageResourceType)
        {
        }

        public DateTimeValidator(string errorMessage) : base(errorMessage)
        {
        }

        public DateTimeValidator(Expression<Func<string>> errorMessageResourceSelector) : base(errorMessageResourceSelector)
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var value = (string) context.PropertyValue;
            if (value.HasValue())
            {
                return value.ToNullableDateTime().HasValue;
            }
            return true;
        }
    }
}