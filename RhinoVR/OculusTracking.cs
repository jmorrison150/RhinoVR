using OculusWrap;
using Rhino;
using Rhino.Geometry;
using System;
using System.Windows.Forms;

namespace RhinoVR {
  public class OculusTracking {
    private static OVR.Vector3f[] _hmdToEyeViewOffsets = new OVR.Vector3f[2];
    private static Point3d _startPos = Point3d.Unset;
    private static Transform _startDir = Transform.Unset;
    private static double[] _fovL = new double[2];
    private static double[] _fovR = new double[2];
    private static Point3d[] _camLoc = new Point3d[2];
    private static Vector3d[] _camDir = new Vector3d[2];
    private static Vector3d[] _camUp = new Vector3d[2];
    private static Wrap _oculus;
    private static Hmd _hmd;

    public static double[] FovL { get { return OculusTracking._fovL; } }
    public static double[] FovR { get { return OculusTracking._fovR; } }
    public static Point3d[] CamLoc { get { return OculusTracking._camLoc; } }
    public static Vector3d[] CamDir { get { return OculusTracking._camDir; } }
    public static Vector3d[] CamUp { get { return OculusTracking._camUp; } }

    private static Transform Matrix(Quaternion q) {
      double num1 = q.A * q.A;
      double num2 = q.B * q.B;
      double num3 = q.C * q.C;
      double num4 = q.D * q.D;
      Transform transform = Transform.Identity;
      transform.M00 = (num1 + num2 - num3 - num4);
      transform.M01 = (2.0 * (q.B * q.C - q.A * q.D));
      transform.M02 = (2.0 * (q.B * q.D + q.A * q.C));
      transform.M03 = (0.0);
      transform.M10 = (2.0 * (q.B * q.C + q.A * q.D));
      transform.M11 = (num1 - num2 + num3 - num4);
      transform.M12 = (2.0 * (q.C * q.D - q.A * q.B));
      transform.M13 = (0.0);
      transform.M20 = (2.0 * (q.B * q.D - q.A * q.C));
      transform.M21 = (2.0 * (q.C * q.D + q.A * q.B));
      transform.M22 = (num1 - num2 - num3 + num4);
      transform.M23 = (0.0);
      transform.M30 = (0.0);
      transform.M31 = (0.0);
      transform.M32 = (0.0);
      transform.M33 = (1.0);
      return transform;
    }

    public static bool CreatOculusHmd() {
      OculusTracking._oculus = (Wrap)null;
      OculusTracking._hmd = (Hmd)null;
      OculusTracking._oculus = new Wrap();
      if (!OculusTracking._oculus.Initialize((OVR.ovrInitParams)null)) {
        int num = (int)MessageBox.Show("Failed to initialize the Oculus runtime library.", "Runtime Warning", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        OculusTracking._oculus = (Wrap)null;
        return false;
      }
      if (OculusTracking._oculus.Hmd_Detect() > 0) {
        OculusTracking._hmd = OculusTracking._oculus.Hmd_Create(0);
        if (OculusTracking._hmd == null) {
          int num = (int)MessageBox.Show("Oculus Rift not detected.", "Connection Warning", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          OculusTracking._oculus.Dispose();
          OculusTracking._oculus = (Wrap)null;
          return false;
        }
        if (!(OculusTracking._hmd.ProductName == string.Empty))
          return true;
        int num1 = (int)MessageBox.Show("The HMD is not enabled.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        OculusTracking._oculus.Dispose();
        OculusTracking._oculus = (Wrap)null;
        OculusTracking._hmd.Dispose();
        OculusTracking._hmd = (Hmd)null;
        return false;
      }
      int num2 = (int)MessageBox.Show("Oculus Rift not connected.", "Connection Warning", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      OculusTracking._oculus.Dispose();
      OculusTracking._oculus = (Wrap)null;
      return false;
    }

    public static void SetupTracking() {
      OculusTracking._hmd.SetEnabledCaps(OVR.HmdCaps.ovrHmdCap_Writable_Mask);
      OculusTracking._hmd.ConfigureTracking(OVR.TrackingCaps.ovrTrackingCap_Orientation | OVR.TrackingCaps.ovrTrackingCap_MagYawCorrection | OVR.TrackingCaps.ovrTrackingCap_Position, OVR.TrackingCaps.None);
      OculusTracking.EyeViewport[] eyeViewportArray = new OculusTracking.EyeViewport[2];
      OVR.FovPort[] fovPortArray = new OVR.FovPort[2];
      for (int index = 0; index < 2; ++index) {
        OVR.EyeType eyeType = (OVR.EyeType)index;
        OculusTracking.EyeViewport eyeViewport = new OculusTracking.EyeViewport();
        eyeViewport.FieldOFView = OculusTracking._hmd.DefaultEyeFov[index];
        eyeViewport.ViewportSize = OculusTracking._hmd.GetFovTextureSize(eyeType, OculusTracking._hmd.DefaultEyeFov[index], 1f);
        eyeViewport.RenderDescription = OculusTracking._hmd.GetRenderDesc(eyeType, OculusTracking._hmd.DefaultEyeFov[index]);
        eyeViewport.HmdToEyeViewOffset = eyeViewport.RenderDescription.HmdToEyeViewOffset;
        fovPortArray[index] = eyeViewport.FieldOFView;
        eyeViewportArray[index] = eyeViewport;
      }
      OVR.Vector3f[] vector3fArray = new OVR.Vector3f[2]
      {
        eyeViewportArray[0].HmdToEyeViewOffset,
        eyeViewportArray[1].HmdToEyeViewOffset
      };
      double[] numArray1 = new double[2]
      {
        Math.Atan((double) fovPortArray[0].LeftTan),
        Math.Atan((double) fovPortArray[1].LeftTan)
      };
      double[] numArray2 = new double[2]
      {
        Math.Atan((double) fovPortArray[0].RightTan),
        Math.Atan((double) fovPortArray[1].RightTan)
      };
      OculusTracking._startPos = new Point3d(0.0, 0.0, 0.0);
      OculusTracking._startDir = Transform.Rotation(new Vector3d(0.0, 1.0, 0.0), new Vector3d(0.0, 1.0, 0.0), new Point3d(0.0, 0.0, 0.0));
      OculusTracking._hmdToEyeViewOffsets = vector3fArray;
      OculusTracking._fovL = numArray1;
      OculusTracking._fovR = numArray2;
    }

    public static void StartTracking(RhinoDoc rhinoDocument) {
      OVR.TrackingState trackingState = OculusTracking._hmd.GetTrackingState(0.0);
      Transform rollPitchYaw = OculusTracking.Matrix(new Quaternion((double)trackingState.HeadPose.ThePose.Orientation.W, (double)trackingState.HeadPose.ThePose.Orientation.X, (double)trackingState.HeadPose.ThePose.Orientation.Y, (double)trackingState.HeadPose.ThePose.Orientation.Z));
      Transform plane = Transform.PlaneToPlane(Plane.WorldZX, Plane.WorldXY);
      OculusTracking._startPos = UserInput.StartPos;
      UserInput.MovementAndOrientation(ref OculusTracking._startPos, ref OculusTracking._startDir, rollPitchYaw, plane, rhinoDocument);
      OVR.Posef[] outEyePoses = new OVR.Posef[2];
      OculusTracking._oculus.CalcEyePoses(trackingState.HeadPose.ThePose, OculusTracking._hmdToEyeViewOffsets, ref outEyePoses);
      for (int index = 0; index < 2; ++index) {
        Point3d point3d1;
        point3d1 = new Point3d((double)outEyePoses[index].Position.X, (double)outEyePoses[index].Position.Y, (double)outEyePoses[index].Position.Z);
        Transform transform = OculusTracking.Matrix(new Quaternion((double)outEyePoses[index].Orientation.W, (double)outEyePoses[index].Orientation.X, (double)outEyePoses[index].Orientation.Y, (double)outEyePoses[index].Orientation.Z));
        Vector3d vector3d1 = (OculusTracking._startDir * (plane * (transform * new Vector3d(0.0, 0.0, -1.0))));
        Vector3d vector3d2 = (OculusTracking._startDir * (plane * (transform * new Vector3d(0.0, 1.0, 0.0))));
        Point3d point3d2 = (OculusTracking._startPos + (OculusTracking._startDir * (plane * point3d1)));
        OculusTracking._camLoc[index] = point3d2;
        OculusTracking._camDir[index] = vector3d1;
        OculusTracking._camUp[index] = vector3d2;
      }
    }

    public static void StopTracking() {
      if (OculusTracking._hmd != null)
        OculusTracking._hmd.Dispose();
      if (OculusTracking._oculus == null)
        return;
      OculusTracking._oculus.Dispose();
    }

    public struct EyeViewport {
      public OVR.EyeRenderDesc RenderDescription { get; set; }

      public OVR.FovPort FieldOFView { get; set; }

      public OVR.Sizei ViewportSize { get; set; }

      public OVR.Vector3f HmdToEyeViewOffset { get; set; }
    }
  }
}
