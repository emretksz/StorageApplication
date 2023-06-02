using Core.Entities.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    /// <summary>
    /// özellikler
    /// </summary>
    public class Property:BaseEntity,IEntity
    {
        public int ?Height { get; set; }
        public int? Width { get; set; }
        public int ?Length { get; set; }
        //public State State { get; set; }
    }
}
