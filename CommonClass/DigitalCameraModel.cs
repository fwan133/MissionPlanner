using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopRevitCommands
{
    public class DigitalCameraModel
    {
        public string Name { get; set; }
        public double FocalLength { get; set; }
        public double SensorSizeU { get; set; }
        public double SensorSizeV { get; set; }
        public int PixelNumU { get; set; }
        public int PixelNumV { get; set; }
        public bool FixedFocalLens { get; set; }

        public DigitalCameraModel()
        {
        }

        public DigitalCameraModel(double focalLength, double sensorSizeU, double sensorSizeV, int pixelNumU, int pixelNumV)
        {
            FocalLength = focalLength;
            SensorSizeU = sensorSizeU;
            SensorSizeV = sensorSizeV;
            PixelNumU = pixelNumU;
            PixelNumV = pixelNumV;
        }

        public DigitalCameraModel(string name, double focalLength, double sensorSizeU, double sensorSizeV, int pixelNumU, int pixelNumV)
        {
            Name = name;
            FocalLength = focalLength;
            SensorSizeU = sensorSizeU;
            SensorSizeV = sensorSizeV;
            PixelNumU = pixelNumU;
            PixelNumV = pixelNumV;
        }

        public DigitalCameraModel(string name, bool fixedFocalLens, double focalLength, double sensorSizeU, double sensorSizeV, int pixelNumU, int pixelNumV)
        {
            Name = name;
            FixedFocalLens = fixedFocalLens;
            FocalLength = focalLength;
            SensorSizeU = sensorSizeU;
            SensorSizeV = sensorSizeV;
            PixelNumU = pixelNumU;
            PixelNumV = pixelNumV;
        }

        public double ObtainWorkingDistance(double GSD)
        {
            double PixelSize = this.SensorSizeU / this.PixelNumU;

            double workingDistance= GSD / PixelSize * FocalLength;

            return workingDistance;
        }

        public double ObtainGSD(double workingDistance)
        {
            double PixelSize = this.SensorSizeU / this.PixelNumU;

            double GSD = Math.Round(workingDistance / FocalLength * PixelSize,3);

            return GSD;
        }

        public double ObtainRelativeDistanceLength(double GSD,double sideOverlap)
        {
            double PixelSize = this.SensorSizeU / this.PixelNumU;

            double relativeDistanceLength = Math.Round((1 - sideOverlap) * SensorSizeU * GSD / PixelSize,3);

            return relativeDistanceLength;
        }

        public double ObtainRelativeDistanceWidth(double GSD, double forwardOverlap)
        {
            double PixelSize = this.SensorSizeU / this.PixelNumU;

            double relativeDistanceWidth= Math.Round((1 - forwardOverlap) * SensorSizeV * GSD / PixelSize,3);

            return relativeDistanceWidth;
        }
    }
}
