using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Data.Dtos
{
    public class RequestBloodDto
    {
        public Guid HospitalId { get; set; }

        public string BloodType { get; set; }

        public int Quantity { get; set; }
         

    }
}
