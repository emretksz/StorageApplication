using Core.Entities.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    //stok
    public class Stock : BaseEntity, IEntity
    {

       // public int  ProductId { get; set; }
        public Product Product { get; set; }
        public DateTime StockTime { get; set; }
        public int StockAmount { get; set; }
        public Store Store { get; set; }
    }
}
