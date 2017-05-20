using System;
using System.ComponentModel.DataAnnotations;

namespace ASUVP.Online.Web.Attributes
{
    public class ValidGuidAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return false;

            Guid result;
            Guid.TryParse(value.ToString(), out result);

            return result != Guid.Empty;
        }
    }
}