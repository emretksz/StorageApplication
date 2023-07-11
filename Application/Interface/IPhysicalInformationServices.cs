using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IPhysicalInformationServices:IBaseService<PhysicalInformation>
    {

        public Task<StateFunctionDto> FizikselBilgiler(List<Prediction> result);
    }
}
