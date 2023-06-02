using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IBaseService<T>
    {
        Task<IDataResults<List<T>>> GetAll(Expression<Func<T, bool>> filter = null);
        Task<IDataResults<T>> GetById(int id);
        Task<IResult> Add(T entity);
        Task<IResult> Update(T entity);
        Task<IResult> UpdateAll(T entity);
        Task<IResult> Delete(T entity);
       // Task<IResult> GetAllQuery(T entity);
    }
}
