using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.DocObjects.Tables;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;
using Rhino.Input;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace RhinoVR {
  public class UserInput {
    private static XController _controller = new XController();
    private static float _rotationAngle = 0.0f;
    private static int _zIndex = 0;
    private static bool _superSpeedActive = true;
    private static bool _shadowOn = false;
    private static bool _sunSliderOn = false;
    private static float _speedX1 = 1f;
    private static float _speedX2 = 1f;
    private static float _speedX10 = 1f;
    private static bool _miniViewportsOn = false;
    private static double _norm = 0.005;
    private static double _norm2 = 0.01;
    private static double _dateParam = -1.0;
    private static double _timeParam = -1.0;
    private static Point3d _target = Point3d.Unset;
    private static Point3d _location = Point3d.Unset;
    private static Vector3d _cameraX = Vector3d.Unset;
    private static Vector3d _cameraY = Vector3d.Unset;
    private static Point3d _startPos = Point3d.Unset;
    private static readonly SunControllerConduit _sunControllerConduit = new SunControllerConduit();
    private static double _scaleFactor;

    public static bool MiniViewportsOn {
      get {
        return UserInput._miniViewportsOn;
      }
    }

    public static Point3d StartPos {
      get {
        return UserInput._startPos;
      }
    }

    public static bool XBoxIsConnected() {
      return UserInput._controller.isConnected;
    }

    private static void SpeedParameter() {
      float leftTrigger;
      if (UserInput._controller.LeftTrigger_Active(out leftTrigger))
        UserInput._speedX1 = leftTrigger + 1f;
      float rightTrigger;
      if (UserInput._controller.RightTrigger_Active(out rightTrigger))
        UserInput._speedX2 = rightTrigger + 1f;
      if (!UserInput._controller.X_Keydown())
        return;
      if (UserInput._superSpeedActive) {
        UserInput._speedX10 = 10f;
        UserInput._superSpeedActive = false;
      } else {
        UserInput._speedX10 = 1f;
        UserInput._superSpeedActive = true;
      }
    }

    public static void ChangeDisplayMode(ref int selectedIndex, int numOfItems) {
      if (!UserInput._controller.isConnected)
        return;
      if (UserInput._controller.Start_Keydown()) {
        if (selectedIndex < numOfItems - 1)
          ++selectedIndex;
        else
          selectedIndex = 0;
        UserInput._controller.SetVibration(false, true, 200);
      }
      if (UserInput._controller.Back_Keydown()) {
        if (selectedIndex > 0)
          --selectedIndex;
        else
          selectedIndex = numOfItems - 1;
        UserInput._controller.SetVibration(false, true, 200);
      }
      UserInput._controller.Vibration_Active();
    }

    public static void MovementAndOrientation(ref Point3d startPos, ref Transform startDir, Transform rollPitchYaw, Transform oculusToRhino, RhinoDoc rhinoDocument) {
      if (!UserInput._controller.isConnected || UserInput._sunSliderOn || UserInput._miniViewportsOn)
        return;
      List<double> zPositions = new List<double>();
      UserInput.FloorOrientPosition(startPos, ref zPositions, rhinoDocument);
      UserInput._controller.GetStateOfXController();
      UserInput.SpeedParameter();
      if (zPositions.Count < 1) {
        // ISSUE: explicit reference operation
        zPositions.Add(((Point3d)@startPos).Z);
      } else if (zPositions.Count > 1) {
        if (UserInput._controller.LeftShoulder_Keydown()) {
          ++UserInput._zIndex;
          if (UserInput._zIndex >= zPositions.Count)
            UserInput._zIndex = 0;
          UserInput._controller.SetVibration(true, false, 600);
        }
        if (UserInput._controller.RightShoulder_Keydown()) {
          --UserInput._zIndex;
          if (UserInput._zIndex < 0)
            UserInput._zIndex = zPositions.Count - 1;
          UserInput._controller.SetVibration(true, false, 600);
        }
      } else
        UserInput._zIndex = 0;
      UserInput._controller.Vibration_Active();
      float rightThumbX;
      if (UserInput._controller.RightThumbX_Active(out rightThumbX)) {
        UserInput._rotationAngle += (float)(-(double)rightThumbX / 50.0) * UserInput._speedX1 * UserInput._speedX2;
        startDir = Transform.Rotation((double)UserInput._rotationAngle, new Vector3d(0.0, 0.0, 1.0), new Point3d(0.0, 0.0, 0.0));
      }
      float leftThumbX;
      if (UserInput._controller.LeftThumbX_Active(out leftThumbX)) {
        Vector3d vector3d = (startDir * (oculusToRhino * (rollPitchYaw *  (new Vector3d((double)leftThumbX / 20.0 * (double)UserInput._speedX1 * (double)UserInput._speedX2 * (double)UserInput._speedX10, 0.0, 0.0)))));
        Point3d local = startPos;
        // ISSUE: explicit reference operation
        Point3d point3d = local +( new Vector3d(((Vector3d)@vector3d).X, ((Vector3d)@vector3d).Y, 0.0));
         local = point3d;
      }
      float leftThumbY;
      if (UserInput._controller.LeftThumbY_Active(out leftThumbY)) {
        Vector3d vector3d = startDir * (oculusToRhino *(rollPitchYaw* (new Vector3d(0.0, 0.0, -((double)leftThumbY / 20.0) * (double)UserInput._speedX1 * (double)UserInput._speedX2 * (double)UserInput._speedX10))));
        Point3d  local = startPos;
        // ISSUE: explicit reference operation
        Point3d point3d = local +( new Vector3d(((Vector3d)@vector3d).X, ((Vector3d)@vector3d).Y, 0.0));
         local = point3d;
      }
      // ISSUE: explicit reference operation
      // ISSUE: explicit reference operation
      startPos = new Point3d(((Point3d)@startPos).X, ((Point3d)@startPos).Y, zPositions[UserInput._zIndex]);
      UserInput._startPos = startPos;
    }

    public static void SunActive() {
      if (!UserInput._controller.isConnected || UserInput._miniViewportsOn)
        return;
      UserInput._controller.GetStateOfXController();
      UserInput.SpeedParameter();
      if (UserInput._controller.Y_Keydown()) {
        if (!UserInput._shadowOn) {
          UserInput._sunControllerConduit.SunlightSettings(RhinoDocument.ActiveDoc, VR_PANEL.Latitude, VR_PANEL.Longitude);
          UserInput._sunControllerConduit.SunSetToNow(ref UserInput._dateParam, ref UserInput._timeParam);
          UserInput._sunControllerConduit.SunLightOn();
          UserInput._controller.SetVibration(true, true, 400);
          UserInput._shadowOn = true;
        } else {
          UserInput._sunControllerConduit.SunLightOff();
          UserInput._sunControllerConduit.Enabled = (false);
          UserInput._controller.SetVibration(true, true, 400);
          UserInput._sunSliderOn = false;
          UserInput._shadowOn = false;
        }
      }
      if (UserInput._controller.LeftThumb_Keydown() && UserInput._shadowOn) {
        if (!UserInput._sunSliderOn) {
          UserInput._sunSliderOn = true;
          UserInput._sunControllerConduit.Enabled = (true);
        } else {
          UserInput._sunSliderOn = false;
          UserInput._sunControllerConduit.Enabled = (false);
        }
      }
      if (UserInput._sunSliderOn && !UserInput._miniViewportsOn) {
        float leftThumbX;
        if (UserInput._controller.LeftThumbX_Active(out leftThumbX)) {
          UserInput._dateParam += (double)leftThumbX * UserInput._norm * (double)UserInput._speedX1 * (double)UserInput._speedX2;
          if (UserInput._dateParam > 1.0)
            UserInput._dateParam = 0.0;
          if (UserInput._dateParam < 0.0)
            UserInput._dateParam = 1.0;
        }
        float rightThumbX;
        if (UserInput._controller.RightThumbX_Active(out rightThumbX)) {
          UserInput._timeParam += (double)rightThumbX * UserInput._norm * (double)UserInput._speedX1 * (double)UserInput._speedX2;
          if (UserInput._timeParam > 1.0)
            UserInput._timeParam = 0.0;
          if (UserInput._timeParam < 0.0)
            UserInput._timeParam = 1.0;
        }
        UserInput._sunControllerConduit.SunPosition(UserInput._dateParam, UserInput._timeParam);
      }
      UserInput._controller.Vibration_Active();
    }

    public static void MiniViewportsActive() {
      if (UserInput._controller.isConnected) {
        UserInput._controller.GetStateOfXController();
        UserInput.SpeedParameter();
        if (UserInput._controller.A_Keydown()) {
          if (!UserInput._miniViewportsOn) {
            if (Viewports.RiftViews[2] != null && Viewports.RiftViews[2].Document != null) {
              Viewports.CreateMiniViewports();
              UserInput._target = Viewports.MiniViews[2].MainViewport.CameraTarget;
              UserInput._location = Viewports.MiniViews[2].MainViewport.CameraLocation;
              UserInput._cameraX = Viewports.MiniViews[2].MainViewport.CameraX;
              UserInput._cameraY = Viewports.MiniViews[2].MainViewport.CameraY;
              UserInput._miniViewportsOn = true;
              UserInput._controller.SetVibration(true, true, 400);
              // ISSUE: explicit reference operation
              UserInput._scaleFactor = Math.PI / 2.0 / ((Point3d)@UserInput._location).DistanceTo(UserInput._target);
            } else
              UserInput._controller.SetVibration(false, true, 1000);
          } else {
            Viewports.CloseMiniViewports();
            UserInput._miniViewportsOn = false;
            UserInput._controller.SetVibration(true, true, 400);
          }
        }
        if (UserInput._miniViewportsOn) {
          float leftThumbX;
          if (UserInput._controller.LeftThumbX_Active(out leftThumbX)) {
            Transform transform = Transform.Rotation((double)leftThumbX * UserInput._norm2 * (double)UserInput._speedX1 * (double)UserInput._speedX2, new Vector3d(0.0, 0.0, 1.0), UserInput._target);
            (UserInput._location).Transform(transform);
            (UserInput._cameraX).Transform(transform);
            (UserInput._cameraY).Transform(transform);
            Viewports.MiniViews[2].MainViewport.SetCameraLocations(UserInput._target, UserInput._location);
            Viewports.MiniViews[2].MainViewport.CameraUp = UserInput._cameraY;
          }
          float leftThumbY;
          if (UserInput._controller.LeftThumbY_Active(out leftThumbY)) {
            Transform transform = Transform.Rotation((double)leftThumbY * UserInput._norm2 * (double)UserInput._speedX1 * (double)UserInput._speedX2, -(UserInput._cameraX), UserInput._target);
            (UserInput._location).Transform(transform);
            (UserInput._cameraX).Transform(transform);
            (UserInput._cameraY).Transform(transform);
            Viewports.MiniViews[2].MainViewport.SetCameraLocations(UserInput._target, UserInput._location);
            Viewports.MiniViews[2].MainViewport.CameraUp = UserInput._cameraY;
          }
          float rightThumbX;
          if (UserInput._controller.RightThumbX_Active(out rightThumbX)) {
            Transform transform = Transform.Translation(Vector3d.Multiply(Vector3d.Multiply(Vector3d.Multiply(UserInput._cameraX, (double)rightThumbX), (double)UserInput._speedX1), (double)UserInput._speedX2));
            (UserInput._target).Transform(transform);
            (UserInput._location).Transform(transform);
            Viewports.MiniViews[2].MainViewport.SetCameraLocations(UserInput._target, UserInput._location);
          }
          float rightThumbY;
          if (UserInput._controller.RightThumbY_Active(out rightThumbY)) {
            Transform transform = Transform.Translation(Vector3d.Multiply(Vector3d.Multiply(Vector3d.Multiply(UserInput._cameraY, (double)rightThumbY), (double)UserInput._speedX1), (double)UserInput._speedX2));
            (UserInput._target).Transform(transform);
            (UserInput._location).Transform(transform);
            Viewports.MiniViews[2].MainViewport.SetCameraLocations(UserInput._target, UserInput._location);
          }
          if (UserInput._controller.DPadUp_KeyOn()) {
            double a = Math.Atan(UserInput._scaleFactor * (UserInput._location).DistanceTo(UserInput._target)) * 2.0;
            Transform transform = Transform.Scale(UserInput._target, 1.0 - Math.Sin(a) * UserInput._norm2 * (double)UserInput._speedX1 * (double)UserInput._speedX2);
            (UserInput._location).Transform(transform);
            Viewports.MiniViews[2].MainViewport.SetCameraLocations(UserInput._target, UserInput._location);
          }
          if (UserInput._controller.DPadDown_KeyOn()) {
            double a = Math.Atan(UserInput._scaleFactor * (UserInput._location).DistanceTo(UserInput._target)) * 2.0;
            Transform transform = Transform.Scale(UserInput._target, Math.Sin(a) * UserInput._norm2 * (double)UserInput._speedX1 * (double)UserInput._speedX2 + 1.0);
            (UserInput._location).Transform(transform);
            Viewports.MiniViews[2].MainViewport.SetCameraLocations(UserInput._target, UserInput._location);
          }
          if (UserInput._controller.DPadLeft_KeyOn()) {
            Transform transform = Transform.Translation(Vector3d.Multiply(Vector3d.Multiply(Vector3d.Multiply(UserInput._cameraX, -0.5), (double)UserInput._speedX1), (double)UserInput._speedX2));
            (UserInput._target).Transform(transform);
            (UserInput._location).Transform(transform);
            Viewports.MiniViews[2].MainViewport.SetCameraLocations(UserInput._target, UserInput._location);
          }
          if (UserInput._controller.DPadRight_KeyOn()) {
            Transform transform = Transform.Translation(Vector3d.Multiply(Vector3d.Multiply(Vector3d.Multiply(UserInput._cameraX, 0.5), (double)UserInput._speedX1), (double)UserInput._speedX2));
            (UserInput._target).Transform(transform);
            (UserInput._location).Transform(transform);
            Viewports.MiniViews[2].MainViewport.SetCameraLocations(UserInput._target, UserInput._location);
          }
        }
        UserInput._controller.Vibration_Active();
      }
      Viewports.UpdateMiniViewports();
    }

    public static Result GetStartPosition() {
      return RhinoGet.GetPoint("Please set a start position", true, out UserInput._startPos);
    }

    public static Result GetFloors(RhinoDoc rhinoDocument) {
      ObjRef[] objRefArray;
      Result multipleObjects = RhinoGet.GetMultipleObjects("Please select surfaces", false, (ObjectType)2097160, out objRefArray);
      if (multipleObjects != null)
        return multipleObjects;
      if (objRefArray == null || objRefArray.Length < 1)
        return (Result)3;
      string str = "#VR User Defined Floors";
      int num = rhinoDocument.Layers.Find(str, true);
      if (num < 0) {
        LayerTable layers = rhinoDocument.Layers;
        num = layers.Add(str, Color.Black);
        if (num < 0) {
          RhinoApp.WriteLine("Unable to add <{0}> layer.", (object)str);
          return (Result)3;
        }
        Layer layer = layers[num];
        layer.Color = (Color.Blue);
        layer.CommitChanges();
      }
      for (int index = 0; index < objRefArray.Length; ++index) {
        RhinoObject rhinoObject = objRefArray[index].Object();
        rhinoObject.Attributes.LayerIndex = num;
        rhinoObject.CommitChanges();
      }
      rhinoDocument.Views.Redraw();
      return (Result)0;
    }

    public static void FloorOrientPosition(Point3d startPos, ref List<double> zPositions, RhinoDoc rhinoDocument) {
      RhinoObject[] byLayer = rhinoDocument.Objects.FindByLayer("#VR User Defined Floors");
      if (byLayer == null || byLayer.Length < 1)
        return;
      List<Brep> brepList1 = new List<Brep>();
      for (int index = 0; index < byLayer.Length; ++index) {
        ObjRef objRef = new ObjRef(byLayer[index]);
        if (objRef != null) {
          Brep brep = objRef.Brep();
          if (brep != null)
            brepList1.Add(brep);
        }
      }
      List<Brep> brepList2 = brepList1;
      List<Point3d> point3dList1 = new List<Point3d>();
      point3dList1.Add(startPos);
      List<Point3d> point3dList2 = point3dList1;
      Vector3d vector3d = new Vector3d(0.0, 0.0, -1.0);
      double absoluteTolerance = rhinoDocument.ModelAbsoluteTolerance;
      Point3d[] breps = Intersection.ProjectPointsToBreps((IEnumerable<Brep>)brepList2, (IEnumerable<Point3d>)point3dList2, vector3d, absoluteTolerance);
      if (breps != null && breps.Length > 0) {
        foreach (Point3d point3d in breps) {
          // ISSUE: explicit reference operation
          double z = ((Point3d)@point3d).Z;
          zPositions.Add(z + VR_PANEL.EyeHeight);
        }
      } else {
        // ISSUE: explicit reference operation
        zPositions.Add(((Point3d)@startPos).Z);
      }
    }
  }
}
