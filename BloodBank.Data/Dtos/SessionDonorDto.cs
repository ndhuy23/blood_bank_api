using BloodBank.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Data.Dtos
{
    public class SessionDonorDto
    {
        public Guid DonorId { get; set; }

        public Guid ActivityId { get; set; }

        public StatusSession Status {  get; set; }
    }
}
