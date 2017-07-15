using Rhino;
using Rhino.Display;
using Rhino.Geometry;
using System.Drawing;
using System.Windows.Forms;

namespace RhinoVR {
  internal class RiftViewportConduit : DisplayConduit {
    private static System.Drawing.Point _cursorInLeftViewport = new System.Drawing.Point();
    private static System.Drawing.Point _cursorInRightViewport = new System.Drawing.Point();
    private int _mirrorClosed;
    private int recSide;
    private int mirrorView;
    private int leftEyeView;
    private int RightEyeView;
    private bool _miniCursorOn;
    private bool _mirrorCursorOn;

    public int MirrorClosed {      get {        return this._mirrorClosed;      }
      set {        this._mirrorClosed = value;      }    }

    public static System.Drawing.Point CursorInLeftViewport {      get {        return RiftViewportConduit._cursorInLeftViewport;      }    }

    public static System.Drawing.Point CursorInRightViewport {      get {        return RiftViewportConduit._cursorInRightViewport;      }    }

    public RiftViewportConduit() {    }

    protected virtual void CalculateBoundingBox(CalculateBoundingBoxEventArgs e) {
      OculusTracking.StartTracking(RhinoDocument.ActiveDoc);
      Viewports.UpdateEyeViewports(OculusTracking.CamLoc, OculusTracking.CamDir, OculusTracking.CamUp);
      this._mirrorClosed = Viewports.UpdateMirrorViewport(OculusTracking.CamLoc, OculusTracking.CamDir, OculusTracking.CamUp);
      base.CalculateBoundingBox(e);
    }

    protected virtual void DrawForeground(DrawEventArgs e) {
      if (UserInput.MiniViewportsOn) {
        string commandPrompt = RhinoApp.CommandPrompt;
        if (e.Viewport.Name == Viewports.MiniViewportTitles[this.mirrorView]) {
          if (MyoController.MyoUnlocked) {
            if (!this._miniCursorOn) {
              Cursor.Clip = Viewports.MiniViews[this.mirrorView].ScreenRectangle;
              this._miniCursorOn = true;
              this._mirrorCursorOn = false;
            }
            RiftViewportConduit._cursorInLeftViewport = Viewports.MiniViews[this.mirrorView].MainViewport.ScreenToClient(Cursor.Position);
          } else
            this._miniCursorOn = false;
          if (commandPrompt != "Command:") {
            Point2d point2d = new Point2d((double)(e.Viewport.Bounds.Width / 2 + 12), 17.0);
            e.Display.Draw2dText(commandPrompt, Color.White, point2d, true, 15);
          }
        }
        if (e.Viewport.Name == Viewports.MiniViewportTitles[this.leftEyeView]) {
          if (MyoController.MyoUnlocked) {
            Rectangle rectangle = new Rectangle(new System.Drawing.Point(RiftViewportConduit._cursorInLeftViewport.X - this.recSide / 2, RiftViewportConduit._cursorInLeftViewport.Y - this.recSide / 2), new Size(this.recSide, this.recSide));
            e.Display.Draw2dRectangle(rectangle, Color.Black, 1, Color.DarkRed);
          }
          if (commandPrompt != "Command:") {
            Point2d point2d = new Point2d((double)(e.Viewport.Bounds.Width / 2 + 12), 17.0);
            e.Display.Draw2dText(commandPrompt, Color.White, point2d, true, 15);
          }
        }
        if (!(e.Viewport.Name == Viewports.MiniViewportTitles[this.RightEyeView]))
          return;
        if (MyoController.MyoUnlocked) {
          RiftViewportConduit._cursorInRightViewport = new System.Drawing.Point(RiftViewportConduit._cursorInLeftViewport.X - 25, RiftViewportConduit._cursorInLeftViewport.Y);
          Rectangle rectangle = new Rectangle(new System.Drawing.Point(RiftViewportConduit._cursorInRightViewport.X - this.recSide / 2, RiftViewportConduit._cursorInRightViewport.Y - this.recSide / 2), new Size(this.recSide, this.recSide));
          e.Display.Draw2dRectangle(rectangle, Color.Black, 1, Color.DarkRed);
        }
        if (!(commandPrompt != "Command:"))
          return;
        Point2d point2d1 = new Point2d((double)(e.Viewport.Bounds.Width / 2 - 12), 17.0);
        e.Display.Draw2dText(commandPrompt, Color.White, point2d1, true, 15);
      } else {
        string commandPrompt = RhinoApp.CommandPrompt;
        if (e.Viewport.Name == Viewports.RiftViewportTitles[this.mirrorView]) {
          if (MyoController.MyoUnlocked) {
            if (!this._mirrorCursorOn) {
              Cursor.Clip = Viewports.RiftViews[this.mirrorView].ScreenRectangle;
              this._mirrorCursorOn = true;
              this._miniCursorOn = false;
            }
            RiftViewportConduit._cursorInLeftViewport = Viewports.RiftViews[this.mirrorView].MainViewport.ScreenToClient(Cursor.Position);
          } else
            this._mirrorCursorOn = false;
          if (commandPrompt != "Command:") {
            Point2d point2d = new Point2d((double)(e.Viewport.Bounds.Width / 2 + 12), 400.0);
            e.Display.Draw2dText(commandPrompt, Color.White, point2d, true, 15);
          }
        }
        if (e.Viewport.Name == Viewports.RiftViewportTitles[this.leftEyeView]) {
          if (MyoController.MyoUnlocked) {
            Rectangle rectangle = new Rectangle(new System.Drawing.Point(RiftViewportConduit._cursorInLeftViewport.X - this.recSide / 2, RiftViewportConduit._cursorInLeftViewport.Y - this.recSide / 2), new Size(this.recSide, this.recSide));
            e.Display.Draw2dRectangle(rectangle, Color.Black, 1, Color.DarkRed);
          }
          if (commandPrompt != "Command:") {
            Point2d point2d = new Point2d((double)(e.Viewport.Bounds.Width / 2 + 12), 400.0);
            e.Display.Draw2dText(commandPrompt, Color.White, point2d, true, 15);
          }
        }
        if (!(e.Viewport.Name == Viewports.RiftViewportTitles[this.RightEyeView]))
          return;
        if (MyoController.MyoUnlocked) {
          RiftViewportConduit._cursorInRightViewport = new System.Drawing.Point(RiftViewportConduit._cursorInLeftViewport.X - 25, RiftViewportConduit._cursorInLeftViewport.Y);
          Rectangle rectangle = new Rectangle(new System.Drawing.Point(RiftViewportConduit._cursorInRightViewport.X - this.recSide / 2, RiftViewportConduit._cursorInRightViewport.Y - this.recSide / 2), new Size(this.recSide, this.recSide));
          e.Display.Draw2dRectangle(rectangle, Color.Black, 1, Color.DarkRed);
        }
        if (!(commandPrompt != "Command:"))
          return;
        Point2d point2d1 = new Point2d((double)(e.Viewport.Bounds.Width / 2 - 12), 400.0);
        e.Display.Draw2dText(commandPrompt, Color.White, point2d1, true, 15);
      }
    }
  }
}
