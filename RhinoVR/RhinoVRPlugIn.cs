// Decompiled with JetBrains decompiler
// Type: RhinoVR.RhinoVRPlugIn
// Assembly: RhinoVR, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0F066D18-B920-40E4-BC83-5E6F0AA166E5
// Assembly location: C:\Program Files\Rhinoceros 5 (64-bit)\Plug-ins\RhinoVR.dll

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
