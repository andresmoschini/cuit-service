using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;

namespace CuitService.Security
{
    public class SecurityOptions
    {
        public bool SkipLifetimeValidation { get; set; }
        public IEnumerable<SecurityKey> SigningKeys { get; set; }
    }
}
