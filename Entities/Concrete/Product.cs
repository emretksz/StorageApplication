using Core.Entities.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    /// <summary>
    /// İstekler
    /// </summary>
    public class Product:BaseEntity,IEntity
    {
        public string Name { get; set; }
        public DateTime RegisterDate { get; set; }

    }
}
