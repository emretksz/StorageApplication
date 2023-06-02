using Core.DataAccess.EntityFramework.Repository;
using DataAccess.EntityFramework;
using DataAccess.Interfaces;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class StoreRepository : Repository<Store, StorageApplicationContext>, IStoreDal
    {

    }
}
