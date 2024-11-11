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
    public class Role
    {
        public const string Donor = "Donor";
        public const string Hospital = "Hospital";
        public const string Admin = "Admin";
    }
    public class User : EntityAuditBase<Guid>, IAccount
    {
        public string FullName { get ; set ; }

        public string Username { get ; set ; }

        public string Password { get ; set; }

        public string Role { get ; set ; }

        public string? Address { get; set; }

        public string Phone { get; set; }

        public string? Avatar { get; set; }

        public string? BloodType { get; set; }

        public bool IsActive { get; set; }

        public List<SessionDonor> SessionDonors { get; set; }
    }
}
