using Core.Entities;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class LogAndStateDto :IDto
    {
        public StateFunctionDto StateFunctionDto { get; set; }
        public List<LogModel> LogModel { get; set; }

    }
}
