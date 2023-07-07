using Application.Interface;
using Core.Utilities.Results;
using DataAccess.Interfaces;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PhysicalInformationManager : IPhysicalInformationServices
    {
        IPhysicalInformationDal _physicalInformation;
        IStateDal _state;
        public PhysicalInformationManager(IPhysicalInformationDal physicalInformation, IStateDal state)
        {
            _physicalInformation = physicalInformation;
            _state = state;
            
        }



        public Task<IResult> Add(PhysicalInformation entity)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> Delete(PhysicalInformation entity)
        {
            throw new NotImplementedException();
        }

        public async Task<StateFunctionDto> FizikselBilgiler(List<State> stateList)
        {
            ///durumlar gelecek. 
            ///durumların tipleri olacak. bir durumun birden fazla tipi olabilir.
            ///tip'inde kendi içinde özellikleri var.
            ///

            //fiziki bilgilerin agırlıkları olacak.
            //

            List<StateFunctionDto> list = new List<StateFunctionDto>();
            foreach (var physical in stateList)
            {
                var states = await _state.GetByFilterAsync(a => a.Name == physical.Name);
                if (states!=null)
                {
                    physical.Id = states.Id;
                    if (Enums.PyhsicalWeight.Yol.ToString()== states.Name)
                    {
                     
                        var result = await _physicalInformation.GetState(physical);
                        list.Add(result);
                    }
                    else if (Enums.PyhsicalWeight.Orman.ToString()==states.Name)
                    {
                        var result = await _physicalInformation.GetState(physical);
                        list.Add(result);
                    }
                    else if (Enums.PyhsicalWeight.Su.ToString() == states.Name)
                    {
                        var result = await _physicalInformation.GetState(physical);
                        list.Add(result);
                    }
                    else if (Enums.PyhsicalWeight.Dag.ToString() == states.Name)
                    {
                        var result = await _physicalInformation.GetState(physical);
                        list.Add(result);
                    }
                    else
                    {
                        //kontrol eklenebilir
                        continue;
                    }
                   
                }
            }
       
            StateFunctionDto lastState = new();

            var firstEx= list.Where(a=>a!=null).FirstOrDefault(a => a.State.Name == "Yol");
            if (firstEx!=null)
            {
                return firstEx;
            }

            foreach (var item in list.Where(a=>a.State.Name!="Yol"))
            {
                switch (item.State.Name)
                {
                    case "Orman"://2
                        return item;

                    case "Dag"://3
                        var counter2 = list.Where(a => a.State.Name == "Orman");
                        if (counter2.Count() > 0)
                            break;
                        lastState = item;
                        break;

                    case "Su"://4
                        var counter3 = list.Where(a => a.State.Name == "Orman" || a.State.Name == "Dag");
                        if (counter3.Count() > 0)
                            break;
                        lastState = item;
                        break;

                    default:
                        break;
                }

            }
          

            return lastState;
        }

        public Task<IDataResults<List<PhysicalInformation>>> GetAll(Expression<Func<PhysicalInformation, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResults<PhysicalInformation>> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> Update(PhysicalInformation entity)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> UpdateAll(PhysicalInformation entity)
        {
            throw new NotImplementedException();
        }
    }
}
