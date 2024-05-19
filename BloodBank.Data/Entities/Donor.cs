using BloodBank.Data.Abtractions;
using BloodBank.Data.Abtractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Data.Entities
{
    public class Donor : EntityBase<Guid>, IAccount
    {


        public string? BloodType { get; set; }

        public string Phone { get; set; }

        public string? Avarta { get; set; }

        public bool IsActive { get; set; }

        public List<History> Histories { get; set; }

        public List<SessionDonor> SessionDonors { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
