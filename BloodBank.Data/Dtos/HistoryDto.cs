using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Data.Dtos
{
    public class HistoryDto
    {
        public Guid DonorId { get; set; }

        public int Quantity { get; set; }

        public Guid HospitalId { get; set; }

        public string HospitalName { get; set; }

    }
}
