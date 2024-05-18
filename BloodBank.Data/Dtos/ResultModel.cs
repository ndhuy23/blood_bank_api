using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Data.Dtos
{
    public class ResultModel
    {
        public object? Data { get; set; }

        public bool IsSuccess { get; set; }

        public string? Message { get; set; }

    }
}
