using SharpDX.XInput;
using System;

namespace RhinoVR {
  public class XController {
    private float _deadzoneOfLeftThumb = 0.2395398f;
    private float _deadzoneOfRightThumb = 0.2651753f;
    private Controller _controller;
    private Vibration _vibration;
    private bool _leftVib_Active;
    private bool _rightVib_Active;
    private TimeSpan _timeSpan;
    private DateTime _now;
    private float _leftThumbX;
    private float _leftThumbY;
    private float _rightThumbX;
    private float _rightThumbY;
    private float _leftTrigger;
    private float _rightTrigger;
    private GamepadButtonFlags _button;
    private bool _sw_LeftShoulder;
    private bool _sw_RightShoulder;
    private bool _sw_LeftThumb;
    private bool _sw_RightThumb;
    private bool _sw_DPadLeft;
    private bool _sw_DPadRight;
    private bool _sw_DPadUp;
    private bool _sw_DPadDown;
    private bool _sw_A;
    private bool _sw_B;
    private bool _sw_X;
    private bool _sw_Y;
    private bool _sw_Start;
    private bool _sw_Back;

    public bool isConnected {
      get {
        return this._controller.IsConnected;
      }
    }

    public XController() {
      this._controller = new Controller(UserIndex.One);
      this._vibration = new Vibration();
    }

    public void GetStateOfXController() {
      if (!this.isConnected)
        return;
      State state = this._controller.GetState();
      float num1 = Math.Max(-1f, (float)state.Gamepad.LeftThumbX / (float)short.MaxValue);
      float num2 = Math.Max(-1f, (float)state.Gamepad.LeftThumbY / (float)short.MaxValue);
      float num3 = Math.Max(-1f, (float)state.Gamepad.RightThumbX / (float)short.MaxValue);
      float num4 = Math.Max(-1f, (float)state.Gamepad.RightThumbY / (float)short.MaxValue);
      this._leftThumbX = (double)Math.Abs(num1) < (double)this._deadzoneOfLeftThumb ? 0.0f : num1;
      this._leftThumbY = (double)Math.Abs(num2) < (double)this._deadzoneOfLeftThumb ? 0.0f : num2;
      this._rightThumbX = (double)Math.Abs(num3) < (double)this._deadzoneOfRightThumb ? 0.0f : num3;
      this._rightThumbY = (double)Math.Abs(num4) < (double)this._deadzoneOfRightThumb ? 0.0f : num4;
      this._leftTrigger = Math.Max(0.0f, (float)state.Gamepad.LeftTrigger / (float)byte.MaxValue);
      this._rightTrigger = Math.Max(0.0f, (float)state.Gamepad.RightTrigger / (float)byte.MaxValue);
      this._button = state.Gamepad.Buttons;
    }

    public bool LeftThumbX_Active(out float leftThumbX) {
      if ((double)this._leftThumbX != 0.0) {
        leftThumbX = this._leftThumbX;
        return true;
      }
      leftThumbX = this._leftThumbX;
      return false;
    }

    public bool LeftThumbY_Active(out float leftThumbY) {
      if ((double)this._leftThumbY != 0.0) {
        leftThumbY = this._leftThumbY;
        return true;
      }
      leftThumbY = this._leftThumbY;
      return false;
    }

    public bool RightThumbX_Active(out float rightThumbX) {
      if ((double)this._rightThumbX != 0.0) {
        rightThumbX = this._rightThumbX;
        return true;
      }
      rightThumbX = this._rightThumbX;
      return false;
    }

    public bool RightThumbY_Active(out float rightThumbY) {
      if ((double)this._rightThumbY != 0.0) {
        rightThumbY = this._rightThumbY;
        return true;
      }
      rightThumbY = this._rightThumbY;
      return false;
    }

    public bool LeftTrigger_Active(out float leftTrigger) {
      if ((double)this._leftTrigger != 0.0) {
        leftTrigger = this._leftTrigger;
        return true;
      }
      leftTrigger = this._leftTrigger;
      return false;
    }

    public bool RightTrigger_Active(out float rightTrigger) {
      if ((double)this._rightTrigger != 0.0) {
        rightTrigger = this._rightTrigger;
        return true;
      }
      rightTrigger = this._rightTrigger;
      return false;
    }

    public bool LeftShoulder_Keydown() {
      if ((this._button & GamepadButtonFlags.LeftShoulder) != GamepadButtonFlags.None && !this._sw_LeftShoulder) {
        this._sw_LeftShoulder = true;
        return true;
      }
      if ((this._button & GamepadButtonFlags.LeftShoulder) == GamepadButtonFlags.None)
        this._sw_LeftShoulder = false;
      return false;
    }

    public bool RightShoulder_Keydown() {
      if ((this._button & GamepadButtonFlags.RightShoulder) != GamepadButtonFlags.None && !this._sw_RightShoulder) {
        this._sw_RightShoulder = true;
        return true;
      }
      if ((this._button & GamepadButtonFlags.RightShoulder) == GamepadButtonFlags.None)
        this._sw_RightShoulder = false;
      return false;
    }

    public bool LeftThumb_Keydown() {
      if ((this._button & GamepadButtonFlags.LeftThumb) != GamepadButtonFlags.None && !this._sw_LeftThumb) {
        this._sw_LeftThumb = true;
        return true;
      }
      if ((this._button & GamepadButtonFlags.LeftThumb) == GamepadButtonFlags.None)
        this._sw_LeftThumb = false;
      return false;
    }

    public bool RightThumb_Keydown() {
      if ((this._button & GamepadButtonFlags.RightThumb) != GamepadButtonFlags.None && !this._sw_RightThumb) {
        this._sw_RightThumb = true;
        return true;
      }
      if ((this._button & GamepadButtonFlags.RightThumb) == GamepadButtonFlags.None)
        this._sw_RightThumb = false;
      return false;
    }

    public bool DPadLeft_Keydown() {
      if ((this._button & GamepadButtonFlags.DPadLeft) != GamepadButtonFlags.None && !this._sw_DPadLeft) {
        this._sw_DPadLeft = true;
        return true;
      }
      if ((this._button & GamepadButtonFlags.DPadLeft) == GamepadButtonFlags.None)
        this._sw_DPadLeft = false;
      return false;
    }

    public bool DPadLeft_KeyOn() {
      return (this._button & GamepadButtonFlags.DPadLeft) != GamepadButtonFlags.None;
    }

    public bool DPadRight_Keydown() {
      if ((this._button & GamepadButtonFlags.DPadRight) != GamepadButtonFlags.None && !this._sw_DPadRight) {
        this._sw_DPadRight = true;
        return true;
      }
      if ((this._button & GamepadButtonFlags.DPadRight) == GamepadButtonFlags.None)
        this._sw_DPadRight = false;
      return false;
    }

    public bool DPadRight_KeyOn() {
      return (this._button & GamepadButtonFlags.DPadRight) != GamepadButtonFlags.None;
    }

    public bool DPadUp_Keydown() {
      if ((this._button & GamepadButtonFlags.DPadUp) != GamepadButtonFlags.None && !this._sw_DPadUp) {
        this._sw_DPadUp = true;
        return true;
      }
      if ((this._button & GamepadButtonFlags.DPadUp) == GamepadButtonFlags.None)
        this._sw_DPadUp = false;
      return false;
    }

    public bool DPadUp_KeyOn() {
      return (this._button & GamepadButtonFlags.DPadUp) != GamepadButtonFlags.None;
    }

    public bool DPadDown_Keydown() {
      if ((this._button & GamepadButtonFlags.DPadDown) != GamepadButtonFlags.None && !this._sw_DPadDown) {
        this._sw_DPadDown = true;
        return true;
      }
      if ((this._button & GamepadButtonFlags.DPadDown) == GamepadButtonFlags.None)
        this._sw_DPadDown = false;
      return false;
    }

    public bool DPadDown_KeyOn() {
      return (this._button & GamepadButtonFlags.DPadDown) != GamepadButtonFlags.None;
    }

    public bool A_Keydown() {
      if ((this._button & GamepadButtonFlags.A) != GamepadButtonFlags.None && !this._sw_A) {
        this._sw_A = true;
        return true;
      }
      if ((this._button & GamepadButtonFlags.A) == GamepadButtonFlags.None)
        this._sw_A = false;
      return false;
    }

    public bool B_Keydown() {
      if ((this._button & GamepadButtonFlags.B) != GamepadButtonFlags.None && !this._sw_B) {
        this._sw_B = true;
        return true;
      }
      if ((this._button & GamepadButtonFlags.B) == GamepadButtonFlags.None)
        this._sw_B = false;
      return false;
    }

    public bool X_Keydown() {
      if ((this._button & GamepadButtonFlags.X) != GamepadButtonFlags.None && !this._sw_X) {
        this._sw_X = true;
        return true;
      }
      if ((this._button & GamepadButtonFlags.X) == GamepadButtonFlags.None)
        this._sw_X = false;
      return false;
    }

    public bool Y_Keydown() {
      if ((this._button & GamepadButtonFlags.Y) != GamepadButtonFlags.None && !this._sw_Y) {
        this._sw_Y = true;
        return true;
      }
      if ((this._button & GamepadButtonFlags.Y) == GamepadButtonFlags.None)
        this._sw_Y = false;
      return false;
    }

    public bool Start_Keydown() {
      if ((this._button & GamepadButtonFlags.Start) != GamepadButtonFlags.None && !this._sw_Start) {
        this._sw_Start = true;
        return true;
      }
      if ((this._button & GamepadButtonFlags.Start) == GamepadButtonFlags.None)
        this._sw_Start = false;
      return false;
    }

    public bool Back_Keydown() {
      if ((this._button & GamepadButtonFlags.Back) != GamepadButtonFlags.None && !this._sw_Back) {
        this._sw_Back = true;
        return true;
      }
      if ((this._button & GamepadButtonFlags.Back) == GamepadButtonFlags.None)
        this._sw_Back = false;
      return false;
    }

    public void SetVibration(bool leftVib, bool rightVib, int milliseconds) {
      this._leftVib_Active = leftVib;
      this._rightVib_Active = rightVib;
      this._timeSpan = new TimeSpan(0, 0, 0, 0, milliseconds);
    }

    public void Vibration_Active() {
      if (!(this._timeSpan != TimeSpan.Zero))
        return;
      if (this._leftVib_Active) {
        this._now = DateTime.Now;
        this._vibration.LeftMotorSpeed = ushort.MaxValue;
        this._controller.SetVibration(this._vibration);
        this._leftVib_Active = false;
      } else if (this._rightVib_Active) {
        this._now = DateTime.Now;
        this._vibration.RightMotorSpeed = ushort.MaxValue;
        this._controller.SetVibration(this._vibration);
        this._rightVib_Active = false;
      }
      if (!(DateTime.Now - this._now > this._timeSpan))
        return;
      this._vibration.LeftMotorSpeed = (ushort)0;
      this._vibration.RightMotorSpeed = (ushort)0;
      this._controller.SetVibration(this._vibration);
      this._timeSpan = TimeSpan.Zero;
    }
  }
}
