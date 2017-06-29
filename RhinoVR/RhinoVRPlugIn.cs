using Rhino.PlugIns;
using Rhino.UI;
using RhinoVR.Properties;

namespace RhinoVR {
  public class RhinoVRPlugIn : PlugIn {
    public static RhinoVRPlugIn Instance { get; private set; }

    public RhinoVRPlugIn() {

      RhinoVRPlugIn.Instance = this;
    }

    protected virtual LoadReturnCode OnLoad(ref string errorMessage) {
      Panels.RegisterPanel((PlugIn)this, typeof(VR_PANEL), "lingMatrix", MainIcon.ling);
      return (LoadReturnCode)1;
    }
  }
}
