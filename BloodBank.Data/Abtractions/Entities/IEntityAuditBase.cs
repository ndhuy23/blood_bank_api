using BloodBank.Data.Abtractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Data.Abtractions.Interfaces
{
    public interface IEntityAuditBase<T> : IEntityBase<T>, IAuditable, ISoftDelete
    {

    }
}
