using BloodBank.Data.Abtractions;
using BloodBank.Data.Abtractions.Entities;
using BloodBank.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Data.Entities
{
    public class Account : EntityAuditBase<Guid>, IAccount
    {
        public string FullName { get ; set ; }

        public string Username { get ; set ; }

        public string Password { get ; set; }

        public Role Role { get ; set ; }

        public Donor Donor { get; set; }

        public Hospital Hospital { get; set; }
    }
}
