using BloodBank.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Data.Dtos.Users
{
    public class UserDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
