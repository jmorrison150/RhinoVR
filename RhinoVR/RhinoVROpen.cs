using Rhino;
using Rhino.Commands;
using Rhino.UI;
using System.Runtime.InteropServices;

namespace RhinoVR {
  [System.Runtime.InteropServices.Guid("f700db5e-87bb-45d8-b7e5-d20e5cabf1d2")]
  public class RhinoVROpen : Rhino.Commands.Command {

    public RhinoVROpen() { RhinoVROpen.Instance = this; }
    public static RhinoVROpen Instance { get; private set; }
    public override string EnglishName { get { return "RhinoVROpen"; } }
    protected override Rhino.Commands.Result RunCommand(Rhino.RhinoDoc doc, Rhino.Commands.RunMode mode) {
      var type = typeof(VR_PANEL);
      Rhino.UI.Panels.OpenPanel(type.GUID);
      return Rhino.Commands.Result.Success;
    }
  }
}
