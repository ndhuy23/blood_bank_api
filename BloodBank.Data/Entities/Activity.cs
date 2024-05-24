using BloodBank.Data.Abtractions;
using BloodBank.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Data.Entities
{
    public class Activity : EntityAuditBase<Guid>
    {

        public Guid HospitalId {  get; set; }

        public Hospital Hospital { get; set; }

        public DateTime DateActivity { get; set; }

        public string OperatingHour { get; set; }

        public int Quantity { get; set; }

        public int NumberIsRegistration { get; set; } = 0;

        public List<SessionDonor> SessionDonors { get; set; } = new List<SessionDonor>();

        public StatusActivity Status { get; set; }
        
    }
}
