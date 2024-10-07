using BloodBank.Data.Abtractions;
using BloodBank.Data.Abtractions.Entities;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Data.Entities
{
    public class Hospital : EntityBase<Guid>
    {
        public Guid AccountId { get; set; }

        public Account Account { get; set; }
        public string Address { get; set; }
        public List<Blood> Bloods { get; set; } = new List<Blood>();

        public List<Activity> Activities {  get; set; } = new List<Activity>();
        
        public List<RequestBlood> RequestBloods { get; set; } = new List<RequestBlood>();

        public string Avatar { get; set; }

    }
}
