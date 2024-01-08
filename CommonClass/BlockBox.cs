using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace LaptopRevitCommands
{
    public class BlockBox
    {
        // Properties
        public List<CameraViewpoint> CameraViewpointList { get; set; }

        public List<XYZ> PositionList { get; set; }
        public XYZ Min { get; }
        public XYZ Max { get; }
        public XYZ center { get; }

        public int quantity { get; }

        // Constructor
        public BlockBox(List<CameraViewpoint> mCameraViewpoints)
        {
            this.CameraViewpointList = mCameraViewpoints;
            List<XYZ> XYZlist = new List<XYZ>();
            foreach(CameraViewpoint cameraViewpoint in mCameraViewpoints)
            {
                XYZlist.Add(cameraViewpoint.position);
            }
            this.PositionList = XYZlist;
            this.Min=new XYZ(PositionList.Min(e => e.X), PositionList.Min(e => e.Y), PositionList.Min(e => e.Z));
            this.Max = new XYZ(PositionList.Max(e => e.X), PositionList.Max(e => e.Y), PositionList.Max(e => e.Z));
            this.center = (Max+Min) / 2;
            quantity = mCameraViewpoints.Count();
        }

        // Methods
        // Method 1: DivideBlockInVertical
        public List<BlockBox> DivideBlockInVertical(double rate)
        {

            List<BlockBox> result = new List<BlockBox>();
            List<CameraViewpoint> CameraViewpointList = this.CameraViewpointList;
                        
            List<CameraViewpoint> outCameraViewpoints = new List<CameraViewpoint>();
            var initialGroup = from cameraViewpoint in CameraViewpointList
                               orderby cameraViewpoint.position.Z
                               group cameraViewpoint by cameraViewpoint.position.Z;

            if (initialGroup.Count() == 1)
            {
                result.Add(new BlockBox(CameraViewpointList));
                return result;
            }

            double avg = (this.Max - this.Min).Z / (initialGroup.Count() - 1);

            for (int i = 0; i < initialGroup.Count(); i++)
            {
                if (i == 0)
                {
                    foreach (var ele in initialGroup.ElementAt(i))
                    {
                        outCameraViewpoints.Add(ele);
                    }
                }
                else
                {
                    double interval = initialGroup.ElementAt(i).Key - initialGroup.ElementAt(i - 1).Key;
                    if (interval < avg * rate)
                    {
                        foreach (var ele in initialGroup.ElementAt(i))
                        {
                            outCameraViewpoints.Add(ele);
                        }

                        if (i == initialGroup.Count() - 1)
                        {
                            result.Add(new BlockBox(outCameraViewpoints));
                        }
                    }
                    else
                    {
                        result.Add(new BlockBox(outCameraViewpoints));
                        outCameraViewpoints = new List<CameraViewpoint>();
                        foreach (var ele in initialGroup.ElementAt(i))
                        {
                            outCameraViewpoints.Add(ele);
                        }

                        if(i == initialGroup.Count() - 1)
                        {
                            result.Add(new BlockBox(outCameraViewpoints));
                        }
                    }
                }
            }

            return result;
        }

        // Method 2: DivideBlockInLateral
        public List<BlockBox> DivideBlockInLateral(double rate)
        {
            List<BlockBox> result = new List<BlockBox>();
            List<CameraViewpoint> CameraViewpointList = this.CameraViewpointList;

            List<CameraViewpoint> outCameraViewpoints = new List<CameraViewpoint>();
            var initialGroup = from cameraViewpoint in CameraViewpointList
                               orderby cameraViewpoint.position.Y
                               group cameraViewpoint by cameraViewpoint.position.Y;

            if (initialGroup.Count() == 1)
            {
                result.Add(new BlockBox(CameraViewpointList));
                return result;
            }

            double avg = (this.Max - this.Min).Y / (initialGroup.Count() - 1);

            for (int i = 0; i < initialGroup.Count(); i++)
            {
                if (i == 0)
                {
                    foreach (var ele in initialGroup.ElementAt(i))
                    {
                        outCameraViewpoints.Add(ele);
                    }
                }
                else
                {
                    double interval = initialGroup.ElementAt(i).Key - initialGroup.ElementAt(i - 1).Key;
                    if (interval < avg * rate)
                    {
                        foreach (var ele in initialGroup.ElementAt(i))
                        {
                            outCameraViewpoints.Add(ele);
                        }

                        if (i == initialGroup.Count() - 1)
                        {
                            result.Add(new BlockBox(outCameraViewpoints));
                        }
                    }
                    else
                    {
                        result.Add(new BlockBox(outCameraViewpoints));
                        outCameraViewpoints = new List<CameraViewpoint>();
                        foreach (var ele in initialGroup.ElementAt(i))
                        {
                            outCameraViewpoints.Add(ele);
                        }

                        if (i == initialGroup.Count() - 1)
                        {
                            result.Add(new BlockBox(outCameraViewpoints));
                        }
                    }
                }
            }

            return result;
        }

        // Method 3: DivideBlockInLongitudinal
        public List<BlockBox> DivideBlockInLongitudinal(double rate)
        {
            List<BlockBox> result = new List<BlockBox>();
            List<CameraViewpoint> CameraViewpointList = this.CameraViewpointList;

            List<CameraViewpoint> outCameraViewpoints = new List<CameraViewpoint>();
            var initialGroup = from cameraViewpoint in CameraViewpointList
                               orderby cameraViewpoint.position.X
                               group cameraViewpoint by cameraViewpoint.position.X;

            if (initialGroup.Count() == 1)
            {
                result.Add(new BlockBox(CameraViewpointList));
                return result;
            }

            double avg = (this.Max - this.Min).X / (initialGroup.Count() - 1);

            for (int i = 0; i < initialGroup.Count(); i++)
            {
                if (i == 0)
                {
                    foreach (var ele in initialGroup.ElementAt(i))
                    {
                        outCameraViewpoints.Add(ele);
                    }
                }
                else
                {
                    double interval = initialGroup.ElementAt(i).Key - initialGroup.ElementAt(i - 1).Key;
                    if (interval < avg * rate)
                    {
                        foreach (var ele in initialGroup.ElementAt(i))
                        {
                            outCameraViewpoints.Add(ele);
                        }

                        if (i == initialGroup.Count() - 1)
                        {
                            result.Add(new BlockBox(outCameraViewpoints));
                        }
                    }
                    else
                    {
                        result.Add(new BlockBox(outCameraViewpoints));
                        outCameraViewpoints = new List<CameraViewpoint>();
                        foreach (var ele in initialGroup.ElementAt(i))
                        {
                            outCameraViewpoints.Add(ele);
                        }

                        if (i == initialGroup.Count() - 1)
                        {
                            result.Add(new BlockBox(outCameraViewpoints));
                        }
                    }
                }
            }

            return result;
        }

        // Method 4: Calculate the distance between two block
        public XYZ Distanceto(BlockBox blockBoundingBox2)
        {
            XYZ xyz =new XYZ(Math.Abs((blockBoundingBox2.center-this.center).X),Math.Abs((blockBoundingBox2.center - this.center).Y), Math.Abs((blockBoundingBox2.center - this.center).Z));

            return xyz;
        }

        // Method 5: Calculate the nearest block box
        public BlockBox ObtainNearestBlockBox(IList<BlockBox> inputBlockBoxList)
        {
            return inputBlockBoxList.OrderBy(q => q.Distanceto(this).GetLength()).ElementAt(0);
        }

    }
}
