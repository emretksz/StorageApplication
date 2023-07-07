using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface  IStockServices:IBaseService<Stock>
    {

        public Task<string>RemoveStockForStore(List<RemoveStockDto> productList);
        public Task<List<RemoveStockDto>> GetRemoveStockProductAndCount(List<ProductAndAmountDto> sayfadaGosterilecekListe, List<ProductAndAmountDto> stoktanSilinecekler);


    }
}
