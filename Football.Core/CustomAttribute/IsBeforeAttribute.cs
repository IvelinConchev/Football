namespace Football.Core.CustomAttribute
{
    using System.ComponentModel.DataAnnotations;
    public class IsBeforeAttribute : ValidationAttribute
    {
        private readonly string propertyCompare;
        public IsBeforeAttribute(string _propertyCompare, string errorMessage = "")
        {
            propertyCompare = _propertyCompare;
            this.ErrorMessage = errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return null;

            try
            {
                DateTime dateToCompare = (DateTime)validationContext
               .ObjectType.GetProperty(propertyCompare).GetValue(validationContext.ObjectInstance);

                if ((DateTime)value < dateToCompare)
                {
                    return ValidationResult.Success;
                }
            }
            catch (Exception)
            {
            }
            return new ValidationResult(ErrorMessage);
        }
    }
}
