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
