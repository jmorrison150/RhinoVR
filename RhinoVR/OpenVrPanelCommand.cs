// Decompiled with JetBrains decompiler
// Type: RhinoVR.OpenVrPanelCommand
// Assembly: RhinoVR, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0F066D18-B920-40E4-BC83-5E6F0AA166E5
// Assembly location: C:\Program Files\Rhinoceros 5 (64-bit)\Plug-ins\RhinoVR.dll

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
