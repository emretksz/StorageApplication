using Core.Entities.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    /// <summary>
    /// Durumlar
    /// </summary>
    public class State:BaseEntity,IEntity
    {
     
        public bool ThereIsAWay { get; set; }
        public Type Type { get; set; }
        public  Property Property { get; set; }
    }
}
