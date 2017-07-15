using MyoSharp.Math;
using Rhino.Geometry;

namespace RhinoVR {
  public class MouseMover {
    private const float pixelDensity = 0.83f;
    private const float frameRate = 60f;
    private const float vMax = 3.141593f;
    private const float vMin = 0.1745329f;
    private const float CDMax = 8747.155f;
    private const float CDMin = 58.28081f;
    private const float inflectionRatioMin = 0.4f;
    private const float inflectionRatioMax = 0.7f;
    private const float lambdaMin = 1.348136f;
    private const float lambdaMax = 1.68517f;
    private const float defaultAcceleration = 0.005f;
    private const float defaultSensitivity = 0.01f;
    private float _dx;
    private float _dy;
    private float _dxFractional;
    private float _dyFractional;
    private QuaternionF _quat;
    private bool _XTowardsWrist;
    private LinearParameter _acceleration;
    private LinearParameter _sensitivity;

    public float acceleration {
      get {
        return this._acceleration.input;
      }
      set {
        this._acceleration.setInput(value);
      }
    }
    public float sensitivity {
      get {
        return this._sensitivity.input;
      }
      set {
        this._sensitivity.setInput(value);
      }
    }
    public float dx {
      get {
        return this._dx;
      }
    }
    public float dy {
      get {
        return this._dy;
      }
    }
    public MouseMover() {
      this._dx = 0.0f;
      this._dy = 0.0f;
      this._dxFractional = 0.0f;
      this._dyFractional = 0.0f;
      this._XTowardsWrist = false;
      this._acceleration = new LinearParameter(1.348136f, 1.68517f, 0.005f);
      this._sensitivity = new LinearParameter(0.7f, 0.4f, 0.01f);
    }
    private float getGain(float deviceSpeed, float sensitivity, float acceleration) {
      float num = (float)((double)sensitivity * 2.96705985069275 + 0.174532920122147);
      return (float)(58.2808074951172 + 8688.875 / (1.0 + System.Math.Exp(-(double)acceleration * ((double)deviceSpeed - (double)num)))) * 0.83f;
    }
    private float deg2rad(float deg) {
      return (float)((double)deg * 3.14159274101257 / 180.0);
    }
    private void updateMouseDeltas(float dx, float dy) {
      float num = 0.01666667f;
      float gain = this.getGain((float)System.Math.Sqrt((double)dx * (double)dx + (double)dy * (double)dy), this._sensitivity.output(), this._acceleration.output());
      this._dx = dx * gain * num;
      this._dy = dy * gain * num;
      this._dxFractional += utils.extractFractional(this._dx);
      this._dyFractional += utils.extractFractional(this._dy);
      this._dx = (float)System.Math.Truncate((double)this._dx);
      this._dy = (float)System.Math.Truncate((double)this._dy);
      if ((double)System.Math.Abs(this._dxFractional) > 1.0) {
        this._dx += (float)System.Math.Truncate((double)this._dxFractional);
        this._dxFractional = utils.extractFractional(this._dxFractional);
      }
      if ((double)System.Math.Abs(this._dyFractional) <= 1.0)
        return;
      this._dy += (float)System.Math.Truncate((double)this._dyFractional);
      this._dyFractional = utils.extractFractional(this._dyFractional);
    }
    public void onOrientation(QuaternionF quat) {
      this._quat = quat;
    }
    public void onGyroscope(Vector3F gyro) {
      Vector3d vector3d1 = new Vector3d(deg2rad(gyro.X), deg2rad(gyro.Y), deg2rad(gyro.Z));
      Quaternion quaternion1 = new Quaternion(_quat.W, _quat.X, _quat.Y, _quat.Z);
      Vector3d vector3d2 = ((Quaternion)@quaternion1).Rotate(vector3d1);
      Vector3d vector3d3 = this._XTowardsWrist ? new Vector3d(1.0, 0.0, 0.0) : new Vector3d(-1.0, 0.0, 0.0);
      Vector3d vector3d4 = Vector3d.CrossProduct(((Quaternion)@quaternion1).Rotate(vector3d3), new Vector3d(0.0, 0.0, -1.0));
      Vector3d vector3d5 = new Vector3d(0.0, 1.0, 0.0);
      Plane plane1 = new Plane(new Point3d(0.0, 0.0, 0.0), vector3d4);
      Plane plane2 = new Plane(new Point3d(0.0, 0.0, 0.0), vector3d5);
      Quaternion quaternion2 = Quaternion.Rotation(plane1, plane2);
      Vector3d vector3d6 = ((Quaternion)@quaternion2).Rotate(vector3d2);
      this.updateMouseDeltas((float)-((Vector3d)@vector3d2).Z, (float)((Vector3d)@vector3d6).Y);
    }
  }
}
