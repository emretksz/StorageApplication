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
        public double ?Height { get; set; }
        public double? Width { get; set; }
        public double? Length { get; set; }
        public Type Type { get; set; }
    }
}
