using Application.Interface;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class VehicleManager : IVehicleServices
    {
        private readonly IVehicleDal _vehicleRepository;

        public VehicleManager(IVehicleDal vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<IResult> Add(Vehicle entity)
        {
            if (entity != null)
            {
                await _vehicleRepository.CreateAsync(entity);
                return new SuccessResult(ConstMessages.VehicleAdded);
            }
            return new ErrorResult(ConstMessages.VehicleError);
        }

        public async Task<IResult> Delete(Vehicle entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IDataResults<List<Vehicle>>> GetAll(Expression<Func<Vehicle, bool>> filter = null)
        {
            try
            {
                return new SuccessDataResult<List<Vehicle>>(await _vehicleRepository.GetAllAsync());
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<Vehicle>>();
            }
        }

        public async Task<IDataResults<Vehicle>> GetById(int id)
        {
            try
            {
                return new SuccessDataResult<Vehicle>((await _vehicleRepository.GetByFilterAsync(a => a.Id == id)));
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<Vehicle>();
            }
        }

        public async Task<IResult> Update(Vehicle entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult> UpdateAll(Vehicle entity)
        {
            if (entity != null)
            {
                await _vehicleRepository.UpdateAll(entity);
                return new SuccessResult(ConstMessages.VehicleUpdate);
            }
            return new ErrorResult(ConstMessages.VehicleUpdateError);
        }
    }
}
