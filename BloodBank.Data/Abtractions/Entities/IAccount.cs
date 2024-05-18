using BloodBank.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Data.Abtractions.Entities
{
    public interface IAccount
    {
        string FullName { get; set; }

        string Email { get; set; }

        string Password { get; set; }


    }
}
