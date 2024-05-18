using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Data.Dtos.Activity
{
    public class ActivityDto
    {
        public Guid? HospitalId { get; set; }

        public DateTime DateActivity { get; set; }

        public string OperatingHour { get; set; }

        public int Quantity { get; set; }

    }
}
