using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebsitePerformance.Models
{
    public class UrlValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var testedSite = (TestedSite)validationContext.ObjectInstance;
            return Uri.TryCreate(testedSite.Url, UriKind.Absolute, out var resulUri) 
                   && (resulUri.Scheme == Uri.UriSchemeHttp || resulUri.Scheme == Uri.UriSchemeHttps) ?
                   ValidationResult.Success : new ValidationResult("You should enter a valid URL. Example: https://www.example.com/");

        }
    }
}