using Application.Interface;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Interfaces;
using Entities.Concrete;
using System.Linq.Expressions;

namespace Application.Services
{
    public class InformationManager : IInformationServies
    {
        private readonly IInformationDal _informationRepository;

        public InformationManager(IInformationDal informationRepository)
        {
            _informationRepository = informationRepository;
        }

        public  async Task<IResult> Add(Information entity)
        {
            if (entity != null)
            {
                await _informationRepository.CreateAsync(entity);
                return new SuccessResult(ConstMessages.InformationAdded);
            }
            return new ErrorResult(ConstMessages.InformationError);
        }

        public  async Task<IResult> Delete(Information entity)
        {
            throw new NotImplementedException();
        }

        public  async Task<IDataResults<List<Information>>> GetAll(Expression<Func<Information, bool>> filter = null)
        {
            try
            {
                return new SuccessDataResult<List<Information>>(await _informationRepository.GetAllAsync());
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<Information>>();
            }
        }

        public  async Task<IDataResults<Information>> GetById(int id)
        {
            try
            {
                return new SuccessDataResult<Information>((await _informationRepository.GetByFilterAsync(a => a.Id == id)));
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<Information>();
            }
        }

        public  async Task<IResult> Update(Information entity)
        {
            throw new NotImplementedException();
        }

        public  async Task<IResult> UpdateAll(Entities.Concrete.Information entity)
        {
            if (entity != null)
            {
                await _informationRepository.UpdateAll(entity);
                return new SuccessResult(ConstMessages.InformationUpdate);
            }
            return new ErrorResult(ConstMessages.InformationUpdateError);
        }
    }
}
