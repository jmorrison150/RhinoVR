// Decompiled with JetBrains decompiler
// Type: RhinoVR.MyoController
// Assembly: RhinoVR, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0F066D18-B920-40E4-BC83-5E6F0AA166E5
// Assembly location: C:\Program Files\Rhinoceros 5 (64-bit)\Plug-ins\RhinoVR.dll

using MyoSharp.Communication;
using MyoSharp.Device;
using MyoSharp.Exceptions;
using MyoSharp.Poses;
using Rhino;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RhinoVR {
  public class MyoController {
    private MouseMover _mouse = new MouseMover();
    private ToolBoxConduit _toolBoxConduit = new ToolBoxConduit();
    private const int MOUSEEVENTF_LEFTDOWN = 2;
    private const int MOUSEEVENTF_LEFTUP = 4;
    private const int MOUSEEVENTF_MIDDLEDOWN = 32;
    private const int MOUSEEVENTF_MIDDLEUP = 64;
    private const int MOUSEEVENTF_RIGHTDOWN = 8;
    private const int MOUSEEVENTF_RIGHTUP = 16;
    private const int MOUSEEVENTF_MOVE = 1;
    private IChannel _myoChannel;
    private IHub _myoHub;
    private bool _fingerSpreadDone;
    private static bool _myoUnlocked;

    public static bool MyoUnlocked {
      get {
        return MyoController._myoUnlocked;
      }
    }

    [DllImport("user32.dll")]
    public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

    public void StartMyo() {
      this._myoChannel = Channel.Create(ChannelDriver.Create(ChannelBridge.Create(), MyoErrorHandlerDriver.Create(MyoErrorHandlerBridge.Create())));
      this._myoHub = Hub.Create((IChannelListener)this._myoChannel);
      this._myoHub.MyoConnected += new EventHandler<MyoEventArgs>(this._myoHub_MyoConnected);
      this._myoHub.MyoDisconnected += new EventHandler<MyoEventArgs>(this._myoHub_MyoDisconnected);
      this._myoChannel.StartListening();
    }

    public void StopMyo() {
      this._myoChannel.StopListening();
      this._myoHub.Dispose();
      this._myoChannel.Dispose();
    }

    private void _myoHub_MyoConnected(object sender, MyoEventArgs e) {
      RhinoApp.WriteLine("Myo has Connected!");
      e.Myo.Vibrate(VibrationType.Medium);
      e.Myo.Locked += new EventHandler<MyoEventArgs>(this.Myo_Locked);
      e.Myo.Unlocked += new EventHandler<MyoEventArgs>(this.Myo_Unlocked);
      e.Myo.PoseChanged += new EventHandler<PoseEventArgs>(this.Myo_PoseChanged);
      e.Myo.OrientationDataAcquired += new EventHandler<OrientationDataEventArgs>(this.Myo_OrientationDataAcquired);
      e.Myo.GyroscopeDataAcquired += new EventHandler<GyroscopeDataEventArgs>(this.Myo_GyroscopeDataAcquired);
    }

    private void _myoHub_MyoDisconnected(object sender, MyoEventArgs e) {
      RhinoApp.WriteLine("Myo has Disconnected!");
      e.Myo.Locked -= new EventHandler<MyoEventArgs>(this.Myo_Locked);
      e.Myo.Unlocked -= new EventHandler<MyoEventArgs>(this.Myo_Unlocked);
      e.Myo.PoseChanged -= new EventHandler<PoseEventArgs>(this.Myo_PoseChanged);
      e.Myo.OrientationDataAcquired -= new EventHandler<OrientationDataEventArgs>(this.Myo_OrientationDataAcquired);
      e.Myo.GyroscopeDataAcquired -= new EventHandler<GyroscopeDataEventArgs>(this.Myo_GyroscopeDataAcquired);
    }

    private void Myo_Locked(object sender, MyoEventArgs e) {
      e.Myo.Vibrate(VibrationType.Short);
      Cursor.Clip = Screen.PrimaryScreen.Bounds;
      MyoController._myoUnlocked = false;
    }

    private void Myo_Unlocked(object sender, MyoEventArgs e) {
      e.Myo.Unlock(UnlockType.Hold);
      e.Myo.Vibrate(VibrationType.Short);
      if (Viewports.RiftViews[2] != null && Viewports.RiftViews[2].Document != null) {
        MyoController._myoUnlocked = true;
      } else {
        e.Myo.Lock();
        MyoController._myoUnlocked = false;
      }
    }

    private void Myo_OrientationDataAcquired(object sender, OrientationDataEventArgs e) {
      this._mouse.onOrientation(e.Orientation);
    }

    private void Myo_GyroscopeDataAcquired(object sender, GyroscopeDataEventArgs e) {
      this._mouse.onGyroscope(e.Gyroscope);
      if (!e.Myo.IsUnlocked)
        return;
      MyoController.mouse_event(1, (int)this._mouse.dx, (int)this._mouse.dy, 0, 0);
    }

    private void Myo_PoseChanged(object sender, PoseEventArgs e) {
      RhinoApp.WriteLine("Pose Changed: {0}", (object)e.Pose.ToString());
      if (e.Pose == Pose.DoubleTap)
        e.Myo.Lock();
      if (e.Pose == Pose.Fist) {
        MyoController.mouse_event(2, Cursor.Position.X, Cursor.Position.Y, 0, 0);
        MyoController.mouse_event(4, Cursor.Position.X, Cursor.Position.Y, 0, 0);
      }
      if (e.Pose == Pose.FingersSpread) {
        this._fingerSpreadDone = true;
        this._toolBoxConduit.Enabled = (true);
        this._toolBoxConduit.PopUpPosLeft = RiftViewportConduit.CursorInLeftViewport;
        this._toolBoxConduit.PopUpPosRight = RiftViewportConduit.CursorInRightViewport;
      }
      if (e.Pose == Pose.FingersSpread || !this._fingerSpreadDone)
        return;
      this._fingerSpreadDone = false;
      this._toolBoxConduit.Enabled = (false);
      if (!this._toolBoxConduit.ExecuteCommand()) {
        MyoController.mouse_event(8, Cursor.Position.X, Cursor.Position.Y, 0, 0);
        MyoController.mouse_event(16, Cursor.Position.X, Cursor.Position.Y, 0, 0);
      }
      if (!UserInput.MiniViewportsOn)
        return;
      for (int index = 0; index < 3; ++index) {
        if (Viewports.MiniViews[index] != null && Viewports.MiniViews[index].Document != null)
          Viewports.MiniViews[index].Redraw();
      }
    }
  }
}
