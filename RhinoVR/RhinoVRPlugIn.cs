using Rhino.PlugIns;
using Rhino.UI;
using RhinoVR.Properties;

namespace RhinoVR {
  public class RhinoVRPlugIn : Rhino.PlugIns.PlugIn {
    public static RhinoVRPlugIn Instance { get; private set; }

    public RhinoVRPlugIn() {      Instance = this;    }
    
    protected override LoadReturnCode OnLoad(ref string errorMessage) {
      System.Type panelType = typeof(VR_PANEL);
      Rhino.UI.Panels.RegisterPanel(this, panelType, "RhinoVR", MainIcon.ling);
      return Rhino.PlugIns.LoadReturnCode.Success;
    }
  }
}
