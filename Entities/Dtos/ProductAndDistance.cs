﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class ProductAndDistance:IDto
    {
        public int UrunId { get; set; }
        public string UrunAdi { get; set; }
        public int Miktar { get; set; }
        public string Konum { get; set; }

        public string DepoAdi { get; set; }
        public int DepoId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double KM { get; set; }
    }
}
