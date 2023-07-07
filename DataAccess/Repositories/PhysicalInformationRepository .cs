using Core.DataAccess.EntityFramework.Repository;
using DataAccess.EntityFramework;
using DataAccess.Interfaces;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class PhysicalInformationRepository : Repository<PhysicalInformation, StorageApplicationContext>, IPhysicalInformationDal
    {
        public async Task<StateFunctionDto> GetState(State state)
        {
            using (StorageApplicationContext db= new StorageApplicationContext())
            {

                var deneme = await (from t in db.Types.Include(a => a.State)
                                    where t.State.Id == state.Id
                                    select new StateFunctionDto
                                    {
                                        Type = t
                                    }
                                    ).FirstOrDefaultAsync();         
                
                var deneme2 = await (from t in db.Types
                                    join s in db.States on t.State.Id equals s.Id
                                    where t.State.Id == state.Id
                                    select new StateFunctionDto
                                    {
                                        State = s,
                                        Type = t
                                    }
                                    ).FirstOrDefaultAsync();
                var result = await (from t in db.Types.Include(a => a.State)
                                    join s in db.States on t.State.Id equals s.Id
                                    join p in db.Properties.Include(a => a.Type) on t.Id equals p.Type.Id
                                    where t.State.Id == state.Id
                                    select new StateFunctionDto
                                    {
                                        Property=p,
                                        State=s,
                                        Type=t
                                    }
                                    ).FirstOrDefaultAsync();

                var result2 = await (from t in db.Types.Include(a => a.State)
                                    join s in db.States on t.State.Id equals s.Id
                                    join p in db.Properties.Include(a => a.Type) on t.Id equals p.Type.Id
                                    where t.State.Id == state.Id
                                    select new StateFunctionDto
                                    {
                                        Property = p,
                                        State = s,
                                        Type = t
                                    }
                                  ).ToListAsync();


                return result;
            }
        }
    }
}
