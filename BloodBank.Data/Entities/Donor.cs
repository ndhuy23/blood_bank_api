using BloodBank.Data.Abtractions;
using BloodBank.Data.Abtractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Data.Entities
{
    public class Donor : EntityBase<Guid>
    {
        public Guid AccountId { get; set; }

        public Account Account { get; set; }

        public string? BloodType { get; set; }

        public string Phone { get; set; }

        public string? Avarta { get; set; }

        public bool IsActive { get; set; }

        public List<History> Histories { get; set; } = new List<History>();

        public List<SessionDonor> SessionDonors { get; set; } = new List<SessionDonor>();
    }
}
