using Core.Entities;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class ProductAmountAndLocation : IDto
    {
       public List<Coordinate> Cordinate { get; set; }
        public List<ProductAndAmountDto> ProductAndAmount { get; set; }
    }
}
