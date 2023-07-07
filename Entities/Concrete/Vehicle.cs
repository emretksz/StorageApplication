﻿using Core.Entities.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    /// <summary>
    /// araç
    /// </summary>
    public class Vehicle:BaseEntity,IEntity
    {
        public string Konum { get; set; }
        public int VehicleSize { get; set; }
        public int Maliyet { get; set; }
       public int ProductId { get; set; }
        //public List<Product> Product { get; set; }
   
    }
}
