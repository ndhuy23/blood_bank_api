using BloodBank.Data.Abtractions;
using BloodBank.Data.Abtractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Data.Entities
{
    public class Hospital : EntityBase<Guid>, IAccount
    {
        public string Address { get; set; }
        public List<Blood> Bloods { get; set; } = new List<Blood>();

        public List<Activity> Activities {  get; set; } = new List<Activity>();
        
        public List<RequestBlood> RequestBloods { get; set; } = new List<RequestBlood>();

        public string Avatar { get; set; }
        public string FullName { get; set ; }
        public string Email { get ; set ; }
        public string Password { get ; set ; }
    }
}
