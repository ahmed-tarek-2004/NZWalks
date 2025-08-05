using NZWalk.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZWalk.DataAccess.IRepository
{
    public interface IUnitOfWork
    {
        Task SaveChanges();
      public IRegionRepository region { get; }
    }
}
