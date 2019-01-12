using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace GigHub.Core.ViewModels
{
    public class FutureDate: ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime datetime;

            //value can be null so avoid using value.tostring()
            bool IsValid = DateTime.TryParseExact(Convert.ToString(value), 
                "d MMM yyyy", 
                CultureInfo.CurrentCulture, 
                DateTimeStyles.None, 
                out datetime);

            return (IsValid && datetime > DateTime.Now);
            
        }
    }
}