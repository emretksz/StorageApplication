using Core.Entities;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class StateFunctionDto:IDto
    {
        public State State  { get; set; }
        public Concrete.Type Type { get; set; }
        public Property Property { get; set; }
    }
}
