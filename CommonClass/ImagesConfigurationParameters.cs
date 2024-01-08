using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRevitCommands
{
    class ImagesConfigurationParameters
    {
        public double GSD { get; set; }

        public double overlapU { get; set; }

        public double overlapV { get; set; }

        public ImagesConfigurationParameters()
        {
        }

        public ImagesConfigurationParameters(double GSD, double overlapU, double overlapV)
        {
            this.GSD = GSD;
            this.overlapU = overlapU;
            this.overlapV = overlapV;
        }
    }
}
