using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace LaptopRevitCommands
{
    public class CameraViewpoint
    {
        public XYZ position { get; set; }

        public Orientation orientation{get; set;}

        public CameraViewpoint(XYZ position, Orientation orientation)
        {
            this.position = position;
            this.orientation = orientation;
        }

        public CameraViewpoint ObtainNearestViewpoint(IList<CameraViewpoint> mCameraViewpointList)
        {
            CameraViewpoint result= mCameraViewpointList.OrderBy(q => q.position.DistanceTo(this.position)).ElementAt(0);
            return result;
        }

        public IList<CameraViewpoint> ObtainShortestViewpointList(IList<CameraViewpoint> mCameraViewpointList) 
        {
            int number = mCameraViewpointList.Count();
            mCameraViewpointList=mCameraViewpointList.OrderBy(q => q.position.X).ToList();
            IList<CameraViewpoint> result = new List<CameraViewpoint>();
            CameraViewpoint cameraViewpoint;

            double disToStart =this.position.DistanceTo(mCameraViewpointList.First().position);
            double disToEnd = this.position.DistanceTo(mCameraViewpointList.Last().position);
            if (disToStart < disToEnd)
            {
                cameraViewpoint = mCameraViewpointList.First();
            }
            else
            {
                cameraViewpoint = mCameraViewpointList.Last();
            }

            result.Add(cameraViewpoint);
            mCameraViewpointList.Remove(cameraViewpoint);

            for (int i=1; i<number; i++)
            {
                cameraViewpoint = result.Last().ObtainNearestViewpoint(mCameraViewpointList);
                result.Add(cameraViewpoint);
                mCameraViewpointList.Remove(cameraViewpoint);
            }
            return result;
        }
    }

    public class Orientation
    {
        public double Yaw { get; set; }
        public double Pitch { get; set; }
        public double Roll { get; set; }

        public Orientation(double yaw, double pitch, double roll)
        {
            Yaw = yaw;
            Pitch = pitch;
            Roll = roll;
        }
    }

}
