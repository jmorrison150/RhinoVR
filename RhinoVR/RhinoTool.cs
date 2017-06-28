// Decompiled with JetBrains decompiler
// Type: RhinoVR.RhinoTool
// Assembly: RhinoVR, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0F066D18-B920-40E4-BC83-5E6F0AA166E5
// Assembly location: C:\Program Files\Rhinoceros 5 (64-bit)\Plug-ins\RhinoVR.dll

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
