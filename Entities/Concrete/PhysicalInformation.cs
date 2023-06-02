using Core.Entities.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    /// <summary>
    /// Fiziki Bilgi
    /// </summary>
    public class PhysicalInformation :BaseEntity, IEntity 
    {
        //public int StateId { get; set; }
        public State State { get; set; }
    }
}
