// Decompiled with JetBrains decompiler
// Type: RhinoVR.CloseVrPanelCommand
// Assembly: RhinoVR, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0F066D18-B920-40E4-BC83-5E6F0AA166E5
// Assembly location: C:\Program Files\Rhinoceros 5 (64-bit)\Plug-ins\RhinoVR.dll

using Rhino;
using Rhino.Commands;
using Rhino.UI;
using System.Runtime.InteropServices;
using System;

namespace RhinoVR {
  [Guid("3ceae358-bd8d-44e4-87c0-c22d5d7c1e3d")]
  public class CloseVrPanelCommand : Rhino.Commands.Command {


    public CloseVrPanelCommand() {
      CloseVrPanelCommand.Instance = this;
    }

    public static CloseVrPanelCommand Instance { get; private set; }


    public override string EnglishName {
      get {
        return "CloselingMatrix";
      }
    }

    protected override Result RunCommand(RhinoDoc doc, RunMode mode) {
      Panels.ClosePanel(typeof(VR_PANEL).GUID);
      return (Result)0;
    }
  }
}
