using Core.Entities.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{/// <summary>
/// depo
/// </summary>
    public class Store:BaseEntity,IEntity
    {
        public string Konum { get; set; }
        public string Name { get; set; }
        public int StoreSize { get; set; }
        //public int StockId { get; set; }
        //public List<Stock> Stock { get; set; }
    }
}
