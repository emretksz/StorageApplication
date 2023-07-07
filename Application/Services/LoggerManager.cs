using Application.Interface;
using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    internal class LoggerManager : ILoggerServices
    {
        public Task<IResult> Add(LogModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> Delete(LogModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResults<List<LogModel>>> GetAll(Expression<Func<LogModel, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResults<LogModel>> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> Update(LogModel entity)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> UpdateAll(LogModel entity)
        {
            throw new NotImplementedException();
        }
    }
}
