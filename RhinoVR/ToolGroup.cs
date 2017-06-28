// Decompiled with JetBrains decompiler
// Type: RhinoVR.ToolGroup
// Assembly: RhinoVR, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0F066D18-B920-40E4-BC83-5E6F0AA166E5
// Assembly location: C:\Program Files\Rhinoceros 5 (64-bit)\Plug-ins\RhinoVR.dll

using Rhino.Display;
using RhinoVR.Properties;
using System.Drawing;

namespace RhinoVR {
  public class ToolGroup {
    public static RhinoTool SelectionBox24 = new RhinoTool(new DisplayBitmap(ToolIcons.SelectionBox24), "SelectionBox24");
    public static RhinoTool SelectionBox32 = new RhinoTool(new DisplayBitmap(ToolIcons.SelectionBox32), "SelectionBox32");
    public static Size ToolIconSize = new Size(24, 24);
    public static Size ToolSelectionBoxSize = new Size(26, 26);
    public static Size MenuIconSize = new Size(36, 36);
    public static RhinoTool[] MenuGroup = new RhinoTool[6]
    {
      new RhinoTool(new DisplayBitmap(ToolIcons.ModifyTool), "ModifyTools"),
      new RhinoTool(new DisplayBitmap(ToolIcons.TransformTool), "TransformTools"),
      new RhinoTool(new DisplayBitmap(ToolIcons.SnapTool), "SnapTools"),
      new RhinoTool(new DisplayBitmap(ToolIcons.SelectTool), "SelectTools"),
      new RhinoTool(new DisplayBitmap(ToolIcons.CurveTool), "CurveTools"),
      new RhinoTool(new DisplayBitmap(ToolIcons.SurfaceTool), "SurfaceTools")
    };
    public static RhinoTool[] SelectTools = new RhinoTool[7]
    {
      new RhinoTool(new DisplayBitmap(ToolIcons.Hide), "Hide"),
      new RhinoTool(new DisplayBitmap(ToolIcons.Show), "Show"),
      new RhinoTool(new DisplayBitmap(ToolIcons.Group), "Group"),
      new RhinoTool(new DisplayBitmap(ToolIcons.Ungroup), "Ungroup"),
      new RhinoTool(new DisplayBitmap(ToolIcons.SelLast), "SelLast"),
      new RhinoTool(new DisplayBitmap(ToolIcons.SelAll), "SelAll"),
      new RhinoTool(new DisplayBitmap(ToolIcons.SelNone), "SelNone")
    };
    public static RhinoTool[] CurveTools = new RhinoTool[7]
    {
      new RhinoTool(new DisplayBitmap(ToolIcons.Curve), "Curve"),
      new RhinoTool(new DisplayBitmap(ToolIcons.Polyline), "Polyline"),
      new RhinoTool(new DisplayBitmap(ToolIcons.Rectangle), "Rectangle"),
      new RhinoTool(new DisplayBitmap(ToolIcons.Polygon), "Polygon"),
      new RhinoTool(new DisplayBitmap(ToolIcons.Circle), "Circle"),
      new RhinoTool(new DisplayBitmap(ToolIcons.Ellipse), "Ellipse"),
      new RhinoTool(new DisplayBitmap(ToolIcons.Arc), "Arc")
    };
    public static RhinoTool[] SurfaceTools = new RhinoTool[11]
    {
      new RhinoTool(new DisplayBitmap(ToolIcons.Box), "Box"),
      new RhinoTool(new DisplayBitmap(ToolIcons.Sphere), "Sphere"),
      new RhinoTool(new DisplayBitmap(ToolIcons.Cylinder), "Cylinder"),
      new RhinoTool(new DisplayBitmap(ToolIcons.ExtrudeCrv), "ExtrudeCrv"),
      new RhinoTool(new DisplayBitmap(ToolIcons.ExtrudeSrf), "ExtrudeSrf"),
      new RhinoTool(new DisplayBitmap(ToolIcons.SrfPt), "SrfPt"),
      new RhinoTool(new DisplayBitmap(ToolIcons.PlanarSrf), "PlanarSrf"),
      new RhinoTool(new DisplayBitmap(ToolIcons.Pipe), "Pipe"),
      new RhinoTool(new DisplayBitmap(ToolIcons.BooleanUnion), "BooleanUnion"),
      new RhinoTool(new DisplayBitmap(ToolIcons.BooleanDifference), "BooleanDifference"),
      new RhinoTool(new DisplayBitmap(ToolIcons.BooleanIntersection), "BooleanIntersection")
    };
    public static RhinoTool[] ModifyTools = new RhinoTool[6]
    {
      new RhinoTool(new DisplayBitmap(ToolIcons.MoveFace), "MoveFace"),
      new RhinoTool(new DisplayBitmap(ToolIcons.OffsetSrf), "OffsetSrf"),
      new RhinoTool(new DisplayBitmap(ToolIcons.Join), "Join"),
      new RhinoTool(new DisplayBitmap(ToolIcons.Explode), "Explode"),
      new RhinoTool(new DisplayBitmap(ToolIcons.Trim), "Trim"),
      new RhinoTool(new DisplayBitmap(ToolIcons.Split), "Split")
    };
    public static RhinoTool[] TransformTools = new RhinoTool[9]
    {
      new RhinoTool(new DisplayBitmap(ToolIcons.Move), "Move"),
      new RhinoTool(new DisplayBitmap(ToolIcons.Copy), "Copy"),
      new RhinoTool(new DisplayBitmap(ToolIcons.Scale1D), "Scale1D"),
      new RhinoTool(new DisplayBitmap(ToolIcons.Scale2D), "Scale2D"),
      new RhinoTool(new DisplayBitmap(ToolIcons.Scale), "Scale"),
      new RhinoTool(new DisplayBitmap(ToolIcons.Rotate), "Rotate"),
      new RhinoTool(new DisplayBitmap(ToolIcons.Orient), "Orient"),
      new RhinoTool(new DisplayBitmap(ToolIcons.Array), "Array"),
      new RhinoTool(new DisplayBitmap(ToolIcons.Mirror), "Mirror")
    };
    public static RhinoTool[] SnapTools = new RhinoTool[7]
    {
      new RhinoTool(new DisplayBitmap(ToolIcons.PointSnap), "PointSnap"),
      new RhinoTool(new DisplayBitmap(ToolIcons.NearSnap), "NearSnap"),
      new RhinoTool(new DisplayBitmap(ToolIcons.EndSnap), "EndSnap"),
      new RhinoTool(new DisplayBitmap(ToolIcons.MidPoint), "MidPoint"),
      new RhinoTool(new DisplayBitmap(ToolIcons.IntersectionSnap), "IntersectionSnap"),
      new RhinoTool(new DisplayBitmap(ToolIcons.CenterSnap), "CenterSnap"),
      new RhinoTool(new DisplayBitmap(ToolIcons.Perpendicular), "Perpendicular")
    };
  }
}
