using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRevitCommands
{
    public class ViewpointConfigurationParameters
    {
        public double Udistance { get; set; }
        public double Vdistance { get; set; }
        public double workingdistance { get; set; }

        public ViewpointConfigurationParameters(double udistance, double vdistance, double workingdistance)
        {
            Udistance = udistance;
            Vdistance = vdistance;
            this.workingdistance = workingdistance;
        }

    }
}
