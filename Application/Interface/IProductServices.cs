using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface  IProductServices:IBaseService<Product>
    {
        public Task<List<ProductAndAmountDto>> GetProductAndAmount(List<ProductAndAmountDto> productAndAmountDtos);   
        public Task<List<ProductAndAmountDto>> GetProductAndAmountForBestStores(List<ProductAndAmountDto> productAndAmountDtos);
        public  Task<ProductAmountAndLocation> FindBestCordinat( List<ProductAndAmountDto> productAndAmountDtos);
    }
}
