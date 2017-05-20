using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace ASUVP.Core.Business.Identity
{
    public class IdentityPasswordValidator : IIdentityValidator<string>
    {
        public const string Pattern = @"(?=.*[a-zA-Z])((?=.*\d)|(?=.*[^a-zA-Z0-9])).{6,}";
        public const int PasswordLength = 6;

        public Task<IdentityResult> ValidateAsync(string item)
        {
            return Task.FromResult(!Regex.IsMatch(item, Pattern)
                ? IdentityResult.Failed("Password does not match minimum level of complexity")
                : IdentityResult.Success);
        }
    }
}