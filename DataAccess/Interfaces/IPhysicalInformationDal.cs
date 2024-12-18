﻿using Core.DataAccess.EntityFramework.Repository;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IPhysicalInformationDal : IRepository<PhysicalInformation>
    {
        public Task<List<StateFunctionDto>> GetState(State state);
    }
}
