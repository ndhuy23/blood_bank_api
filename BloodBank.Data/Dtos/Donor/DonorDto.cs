using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Data.Dtos.Donor
{
    public class DonorDto
    {
        public string Phone { get; set; }

        public string? Avarta { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }

        public string Password { get; set; }

        public string? BloodType { get; set; }

    }
}
