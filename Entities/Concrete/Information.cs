using Core.Entities.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    /// <summary>
    /// bilgi
    /// </summary>
    public class Information:BaseEntity,IEntity
    {
       // public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }        
        //public int StoreId { get; set; }
        public Store Store { get; set; }
       // public int PhysicalInformationId { get; set; }
        public PhysicalInformation PhysicalInformation { get; set; }
    }
}
