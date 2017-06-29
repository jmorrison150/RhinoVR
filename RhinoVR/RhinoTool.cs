using Rhino.Display;
using Rhino.Geometry;
using System.Drawing;

namespace RhinoVR {
  public class RhinoTool {
    public Point3d ArcPoint0 { get; set; }

    public Point3d ArcPoint1 { get; set; }

    public Point3d ArcPoint2 { get; set; }

    public DisplayBitmap Bitmap { get; private set; }

    public string EnglishName { get; private set; }

    public Rectangle IconBound { get; set; }

    public Point2d IconCenterPoint { get; set; }

    public RhinoTool(DisplayBitmap icon, string englishName) {
      this.Bitmap = icon;
      this.EnglishName = englishName;
    }
  }
}
