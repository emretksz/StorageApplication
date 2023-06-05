using Core.Entities.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Amount: BaseEntity, IEntity
    {
        public int Value { get; set; }
        public string Name { get; set; }
    }
}
