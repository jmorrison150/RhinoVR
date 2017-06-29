using Rhino;
using Rhino.Commands;
using Rhino.UI;
using System.Runtime.InteropServices;

namespace RhinoVR {
  [Guid("f700db5e-87bb-45d8-b7e5-d20e5cabf1d2")]
  public class OpenVrPanelCommand : Command {
    public static OpenVrPanelCommand Instance { get; private set; }

    public override string EnglishName {
      get {
        return "OpenlingMatrix";
      }
    }

    public OpenVrPanelCommand() {

      OpenVrPanelCommand.Instance = this;
    }

    protected override Result RunCommand(RhinoDoc doc, RunMode mode) {
      Panels.OpenPanel(typeof(VR_PANEL).GUID);
      return (Result)0;
    }
  }
}
