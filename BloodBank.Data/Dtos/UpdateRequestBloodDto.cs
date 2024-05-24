using BloodBank.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Data.Dtos
{
    public class UpdateRequestBloodDto
    {
        public Guid HospitalAccept { get; set; }

        public StatusRequestBlood Status { get; set; }

    }
}
