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
    public class SessionDonor : EntityBase<Guid>
    {
        public Guid DonorId { get; set; }

        public Donor Donor { get; set; }

        public Guid ActivityId { get; set; }

        public Activity Activity { get; set; }

        public StatusSession Status {  get; set; }
    }
}
