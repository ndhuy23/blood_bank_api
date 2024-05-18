using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Data.Abtractions.Interfaces
{
    public interface IDateTracking
    {
        DateTimeOffset CreatedDate { get; set; }

        DateTimeOffset ModifiedDate { get; set; }
    }
}
