using BloodBank.Data.Abtractions;
using BloodBank.Data.Abtractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Data.Entities
{
    public class Donor : User
    {
        public List<History> Histories { get; set; } = new List<History>();

        public List<SessionDonor> SessionDonors { get; set; } = new List<SessionDonor>();
    }
}
