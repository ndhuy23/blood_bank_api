using BloodBank.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Data.Dtos.Authentication
{
    public class AuthenticationResponse
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string JwtToken { get; set; }
        public int ExpiresIn { get; set; }

        public Role Role { get; set; }
    }
}
