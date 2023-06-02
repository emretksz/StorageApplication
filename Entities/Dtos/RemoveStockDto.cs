using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class RemoveStockDto:IDto
    {
        public int ProductId { get; set; }
        public int StoreId { get; set; }
        public int Amount { get; set; }
    }
}
