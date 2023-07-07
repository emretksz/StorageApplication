using Core.Entities.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class LogModel : BaseEntity, IEntity
    {
        public int BaseLoggerId { get; set; }
        public int? AracId { get; set; }
        public int? UrunId { get; set; }
        public string? UrunAdi { get; set; }
        public int? UrunMiktarı { get; set; }
        public int? AracKapasitesi { get; set; }
        public int? DepoId { get; set; }
        public int? DepoSiparisOncesiMiktar { get; set; }
        public int? DepoSiparisSonrasiMiktar { get; set; }
        public DateTime IslemZamani { get; set; }
        public string? Konum { get; set; }
        public string? YolculukUzunlugu { get; set; }
        public string?  YolculukSuresi { get; set; }
        public int? Agirlik { get; set; }
        public string? Mod { get; set; }
        public string? Zaman { get; set; }
        public string? DepoKapasitesi { get; set; }
    }
}
