using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    
    public class Prediction
    {
      
        public double Confidence { get; set; }
        public string Class { get; set; }
    }

    public class PredictionsResult
    {
        public List<Prediction> Predictions { get; set; }
    }
}
