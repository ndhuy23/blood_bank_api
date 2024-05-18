using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Data.Dtos
{
    public class PagingModel
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public int? TotalCount { get; set; }

        public object? Data {  get; set; }
        
    }
}
