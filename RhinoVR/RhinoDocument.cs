// Decompiled with JetBrains decompiler
// Type: RhinoVR.RhinoDocument
// Assembly: RhinoVR, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0F066D18-B920-40E4-BC83-5E6F0AA166E5
// Assembly location: C:\Program Files\Rhinoceros 5 (64-bit)\Plug-ins\RhinoVR.dll

using Rhino;

namespace RhinoVR {
  internal class RhinoDocument {
    private static RhinoDoc _rhinoDocument = RhinoDoc.ActiveDoc;

    public static RhinoDoc ActiveDoc {
      get {
        return RhinoDocument._rhinoDocument;
      }
    }
  }
}
