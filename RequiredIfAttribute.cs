using System.ComponentModel.DataAnnotations;

namespace BloodDonationBE.ValidationAttributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class RequiredIfAttribute : ValidationAttribute
{
    private readonly string _dependentProperty;
    private readonly object _targetValue;

    public RequiredIfAttribute(string dependentProperty, object targetValue)
    {
        _dependentProperty = dependentProperty;
        _targetValue = targetValue;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var dependentPropertyInfo = validationContext.ObjectType.GetProperty(_dependentProperty);

        if (dependentPropertyInfo == null)
        {
            return new ValidationResult($"Unknown property: {_dependentProperty}");
        }

        var dependentPropertyValue = dependentPropertyInfo.GetValue(validationContext.ObjectInstance);

        if (Equals(dependentPropertyValue, _targetValue))
        {
            if (value == null)
            {
                return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} is required.");
            }
        }

        return ValidationResult.Success;
    }
}
