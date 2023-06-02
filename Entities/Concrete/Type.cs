using Core.Entities.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{/// <summary>
/// tip
/// </summary>
    public class Type:BaseEntity,IEntity
    {
        public bool ?Tpye1 { get; set; }
        public bool ?Tpye2 { get; set; }
        public bool ?Tpye3 { get; set; }
        public bool ?Tpye4 { get; set; }
        public bool? Tpye5 { get; set; }


    }
}
