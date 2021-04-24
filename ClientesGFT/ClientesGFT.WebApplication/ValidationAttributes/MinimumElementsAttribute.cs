using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClientesGFT.WebApplication.ValidationAttributes
{
    public class MinimumElementsAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly int minElements;

        public string[] ErrorMessageList = {
            $"O campo requer um número mínimo de elementos."
        };

        public MinimumElementsAttribute(int minElements)
        {
            this.minElements = minElements;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var list = value as IList;

                bool isValid = list?.Count >= minElements;

                if (isValid)
                {
                    return ValidationResult.Success;

                }
            }

            return GetValidationResultErrorMessage(ErrorMessage ?? ErrorMessageList[0],
                        validationContext?.DisplayName);
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-minimum-elements", GetValidationClientErrorMessage(context));
        }

        private string GetValidationClientErrorMessage(ClientModelValidationContext context)
        {
            var str = (!string.IsNullOrEmpty(ErrorMessage)) ? ErrorMessage : ErrorMessageList[0];
            return string.Format(str, context.ModelMetadata?.GetDisplayName());
        }

        private ValidationResult GetValidationResultErrorMessage(string message, string param1 = "", string param2 = "")
        {
            return new ValidationResult(string.Format(message, param1, param2));
        }
    }
}
