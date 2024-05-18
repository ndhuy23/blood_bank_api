using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Data.Dtos.Donor
{
    public class DonorRegisterDto
    {
        public string Phone { get; set; }

        public string? Avarta { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public string Password { get; set; }

    }
}
