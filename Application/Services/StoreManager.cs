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
    public class StoreManager : IStoreServices
    {
        private readonly IStoreDal _storeRepository;

        public StoreManager(IStoreDal storeRepository)
        {
            _storeRepository = storeRepository;
        }

        public async Task<IResult> Add(Store entity)
        {
         
            if (entity != null)
            {
                await _storeRepository.CreateAsync(entity);
                return new SuccessResult(ConstMessages.StoreAdded);
            }
            return new ErrorResult(ConstMessages.StoreError);
        }

        public Task<IResult> Delete(Store entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IDataResults<List<Store>>> GetAll(Expression<Func<Store, bool>> filter = null)
        {
            try
            {
                return new SuccessDataResult<List<Store>>(await _storeRepository.GetAllAsync());
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<Store>>();
            }
        }
        public async Task<IDataResults<Store>> GetById(int id)
        {
            try
            {
                return new SuccessDataResult<Store>((await _storeRepository.GetByFilterAsync(a => a.Id == id)));
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<Store>();
            }
        }

        public Task<IResult> Update(Store entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult> UpdateAll(Store entity)
        {
            if (entity != null)
            {
                await _storeRepository.UpdateAll(entity);
                return new SuccessResult(ConstMessages.StoreUpdate);
            }
            return new ErrorResult(ConstMessages.StoreUpdateError);
        }
    }
}
