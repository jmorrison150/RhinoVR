using Rhino;
using Rhino.Commands;
using Rhino.UI;
using System.Runtime.InteropServices;
using System;

namespace RhinoVR {
  [Guid("3ceae358-bd8d-44e4-87c0-c22d5d7c1e3d")]
  public class RhinoVRClose : Rhino.Commands.Command {


    public RhinoVRClose() {
      RhinoVRClose.Instance = this;
    }

    public static RhinoVRClose Instance { get; private set; }


    public override string EnglishName {
      get {
        return "RhinoVRClose";
      }
    }

    protected override Result RunCommand(RhinoDoc doc, RunMode mode) {
      Panels.ClosePanel(typeof(VR_PANEL).GUID);
      return Rhino.Commands.Result.Success;
    }
  }
}
