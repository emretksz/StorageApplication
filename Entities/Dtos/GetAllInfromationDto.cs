using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class GetAllInfromationDto
    {
        public string DepoKapasitesi { get; set; }
        public string AracKapasitesi { get; set; }
        public string IsterAdeti { get; set; }
        public string AlinanIster { get; set; }
        public string AracIleDepoArasindakiMesafe { get; set; }
        public string DeponunFizikiDurumu { get; set; }
        public string OncelikAgirligi { get; set; }
        public string Mod { get; set; }
        public string Zaman { get; set; }
    }
}
