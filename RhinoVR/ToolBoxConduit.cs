using Rhino;
using Rhino.Display;
using Rhino.Geometry;
using RhinoVR.Properties;
using System;
using System.Drawing;

namespace RhinoVR {
  internal class ToolBoxConduit : DisplayConduit {
    private RhinoTool[] _menuGroup;
    private RhinoTool[][] _toolgroups;
    private RhinoTool[] _selectedToolGroup;
    private int _selectedIndex;
    private System.Drawing.Point _popUpPosLeft;
    private System.Drawing.Point _popUpPosRight;
    private int _menuLayoutRadius;
    private int _menuTotalAmount;
    private int _toolGroupArcRadius;
    private int _toolLayoutRadius;
    private int _dividenOfSpan;
    private Color _voiceCommandColor;
    private int _fontSize;
    private bool _voiceOn;

    public System.Drawing.Point PopUpPosLeft {      set {        this._popUpPosLeft = value;      }    }

    public System.Drawing.Point PopUpPosRight {      set {        this._popUpPosRight = value;      }    }

    public ToolBoxConduit() {    }

    protected virtual void DrawOverlay(DrawEventArgs e) {
      if (this._voiceOn) {
        VoiceController.StartVoice();
        this._voiceOn = false;
      }
      if (UserInput.MiniViewportsOn) {
        if (e.Viewport.Name == Viewports.MiniViewportTitles[2]) {
          this.InitToolBar(e, this._popUpPosLeft);
          this.SelectionBox(e, RiftViewportConduit.CursorInLeftViewport);
          this.MenuLayout(e);
          this.ToolLayout(e);
          Point2d point2d = new Point2d ((double)(this._popUpPosLeft.X - 30), (double)(this._popUpPosLeft.Y - 150));
          e.Display.Draw2dText(string.Format("Voice: {0}", (object)VoiceController.RecognizedText), this._voiceCommandColor, point2d, false, this._fontSize);
        }
        if (e.Viewport.Name == Viewports.MiniViewportTitles[0]) {
          this.InitToolBar(e, this._popUpPosLeft);
          this.SelectionBox(e, RiftViewportConduit.CursorInLeftViewport);
          this.MenuLayout(e);
          this.ToolLayout(e);
          Point2d point2d = new Point2d((double)(this._popUpPosLeft.X - 30), (double)(this._popUpPosLeft.Y - 150));
          e.Display.Draw2dText(string.Format("Voice: {0}", (object)VoiceController.RecognizedText), this._voiceCommandColor, point2d, false, this._fontSize);
        }
        if (!(e.Viewport.Name == Viewports.MiniViewportTitles[1]))
          return;
        this.InitToolBar(e, this._popUpPosRight);
        this.SelectionBox(e, RiftViewportConduit.CursorInRightViewport);
        this.MenuLayout(e);
        this.ToolLayout(e);
        Point2d point2d1 = new Point2d((double)(this._popUpPosRight.X - 30), (double)(this._popUpPosRight.Y - 150));
        e.Display.Draw2dText(string.Format("Voice: {0}", (object)VoiceController.RecognizedText), this._voiceCommandColor, point2d1, false, this._fontSize);
      } else {
        if (e.Viewport.Name == Viewports.RiftViewportTitles[2]) {
          this.InitToolBar(e, this._popUpPosLeft);
          this.SelectionBox(e, RiftViewportConduit.CursorInLeftViewport);
          this.MenuLayout(e);
          this.ToolLayout(e);
          Point2d point2d = new Point2d((double)(this._popUpPosLeft.X - 30), (double)(this._popUpPosLeft.Y - 150));
          e.Display.Draw2dText(string.Format("Voice: {0}", (object)VoiceController.RecognizedText), this._voiceCommandColor, point2d, false, this._fontSize);
        }
        if (e.Viewport.Name == Viewports.RiftViewportTitles[0]) {
          this.InitToolBar(e, this._popUpPosLeft);
          this.SelectionBox(e, RiftViewportConduit.CursorInLeftViewport);
          this.MenuLayout(e);
          this.ToolLayout(e);
          Point2d point2d = new Point2d((double)(this._popUpPosLeft.X - 30), (double)(this._popUpPosLeft.Y - 150));
          e.Display.Draw2dText(string.Format("Voice: {0}", (object)VoiceController.RecognizedText), this._voiceCommandColor, point2d, false, this._fontSize);
        }
        if (!(e.Viewport.Name == Viewports.RiftViewportTitles[1]))
          return;
        this.InitToolBar(e, this._popUpPosRight);
        this.SelectionBox(e, RiftViewportConduit.CursorInRightViewport);
        this.MenuLayout(e);
        this.ToolLayout(e);
        Point2d point2d1 = new Point2d((double)(this._popUpPosRight.X - 30), (double)(this._popUpPosRight.Y - 150));
        e.Display.Draw2dText(string.Format("Voice: {0}", (object)VoiceController.RecognizedText), this._voiceCommandColor, point2d1, false, this._fontSize);
      }
    }

    private void InitToolBar(DrawEventArgs e, System.Drawing.Point popUpPosition) {
      e.Display.DrawSprite(new DisplayBitmap(ToolIcons.ToolBoxCanvas), new Point2d((double)popUpPosition.X, (double)popUpPosition.Y), 300f);
      for (int index1 = 0; index1 < this._menuTotalAmount; ++index1) {
        double num1 = (double)popUpPosition.X + Math.Sin(2.0 * Math.PI / (double)this._menuTotalAmount * (double)index1) * (double)this._menuLayoutRadius;
        double num2 = (double)popUpPosition.Y + Math.Cos(2.0 * Math.PI / (double)this._menuTotalAmount * (double)index1) * (double)this._menuLayoutRadius;
        System.Drawing.Point location1 = new System.Drawing.Point((int)(num1 - (double)(ToolGroup.MenuIconSize.Width / 2)), (int)(num2 - (double)(ToolGroup.MenuIconSize.Height / 2)));
        this._menuGroup[index1].IconCenterPoint = new Point2d(num1, num2);
        this._menuGroup[index1].IconBound = new Rectangle(location1, ToolGroup.MenuIconSize);
        double num3 = 2.0 * Math.PI / (double)this._menuTotalAmount * (double)index1 - 2.0 * Math.PI / (double)this._dividenOfSpan * (double)this._toolgroups[index1].Length / 2.0;
        double num4 = (double)popUpPosition.X + Math.Sin(num3) * (double)this._toolGroupArcRadius;
        double num5 = (double)popUpPosition.Y + Math.Cos(num3) * (double)this._toolGroupArcRadius;
        RhinoTool rhinoTool1 = this._menuGroup[index1];
        Line world1 = e.Viewport.ClientToWorld(new System.Drawing.Point((int)num4, (int)num5));
        // ISSUE: explicit reference operation
        Point3d from1 = world1.From;
        rhinoTool1.ArcPoint0 = from1;
        double num6 = (double)popUpPosition.X + Math.Sin(num3 + 2.0 * Math.PI / (double)this._dividenOfSpan * (double)(this._toolgroups[index1].Length / 2)) * (double)this._toolGroupArcRadius;
        double num7 = (double)popUpPosition.Y + Math.Cos(num3 + 2.0 * Math.PI / (double)this._dividenOfSpan * (double)(this._toolgroups[index1].Length / 2)) * (double)this._toolGroupArcRadius;
        RhinoTool rhinoTool2 = this._menuGroup[index1];
        Line world2 = e.Viewport.ClientToWorld(new System.Drawing.Point((int)num6, (int)num7));
        // ISSUE: explicit reference operation
        Point3d from2 = world2.From;
        rhinoTool2.ArcPoint1 = from2;
        double num8 = (double)popUpPosition.X + Math.Sin(num3 + 2.0 * Math.PI / (double)this._dividenOfSpan * (double)(this._toolgroups[index1].Length - 1)) * (double)this._toolGroupArcRadius;
        double num9 = (double)popUpPosition.Y + Math.Cos(num3 + 2.0 * Math.PI / (double)this._dividenOfSpan * (double)(this._toolgroups[index1].Length - 1)) * (double)this._toolGroupArcRadius;
        RhinoTool rhinoTool3 = this._menuGroup[index1];
        Line world3 = e.Viewport.ClientToWorld(new System.Drawing.Point((int)num8, (int)num9));
        // ISSUE: explicit reference operation
        Point3d from3 = world3.From;
        rhinoTool3.ArcPoint2 = from3;
        for (int index2 = 0; index2 < this._toolgroups[index1].Length; ++index2) {
          double num10 = (double)popUpPosition.X + Math.Sin(num3 + 2.0 * Math.PI / (double)this._dividenOfSpan * (double)index2) * (double)this._toolLayoutRadius;
          double num11 = (double)popUpPosition.Y + Math.Cos(num3 + 2.0 * Math.PI / (double)this._dividenOfSpan * (double)index2) * (double)this._toolLayoutRadius;
          System.Drawing.Point location2 = new System.Drawing.Point((int)(num10 - (double)(ToolGroup.ToolIconSize.Width / 2)), (int)(num11 - (double)(ToolGroup.ToolIconSize.Height / 2)));
          this._toolgroups[index1][index2].IconCenterPoint = new Point2d(num10, num11);
          this._toolgroups[index1][index2].IconBound = new Rectangle(location2, ToolGroup.ToolSelectionBoxSize);
        }
      }
    }

    private void SelectionBox(DrawEventArgs e, System.Drawing.Point mouseCursor) {
      for (int index = 0; index < this._menuTotalAmount; ++index) {
        if (this._menuGroup[index].IconBound.Contains(mouseCursor)) {
          e.Display.DrawSprite(ToolGroup.SelectionBox32.Bitmap, this._menuGroup[index].IconCenterPoint, (float)ToolGroup.MenuIconSize.Width);
          this._selectedToolGroup = this._toolgroups[index];
          this._selectedIndex = index;
        }
      }
      if (this._selectedToolGroup == null)
        return;
      for (int index = 0; index < this._selectedToolGroup.Length; ++index) {
        if (this._selectedToolGroup[index].IconBound.Contains(mouseCursor))
          e.Display.DrawSprite(ToolGroup.SelectionBox24.Bitmap, this._selectedToolGroup[index].IconCenterPoint, (float)ToolGroup.ToolSelectionBoxSize.Width);
      }
    }

    private void MenuLayout(DrawEventArgs e) {
      for (int index = 0; index < this._menuTotalAmount; ++index)
        e.Display.DrawSprite(this._menuGroup[index].Bitmap, this._menuGroup[index].IconCenterPoint, (float)ToolGroup.MenuIconSize.Width);
    }

    private void ToolLayout(DrawEventArgs e) {
      if (this._selectedToolGroup == null)
        return;
      for (int index = 0; index < this._selectedToolGroup.Length; ++index)
        e.Display.DrawSprite(this._selectedToolGroup[index].Bitmap, this._selectedToolGroup[index].IconCenterPoint, (float)ToolGroup.ToolIconSize.Width);
      e.Display.DrawArc(new Arc(this._menuGroup[this._selectedIndex].ArcPoint0, this._menuGroup[this._selectedIndex].ArcPoint1, this._menuGroup[this._selectedIndex].ArcPoint2), Color.Gray, 4);
    }

    public bool ExecuteCommand() {
      if (VoiceController.IsEnabled && VoiceController.RecognizedText != "") {
        RhinoApp.SendKeystrokes(VoiceController.RecognizedText, true);
        VoiceController.RecognizedText = "";
      } else {
        if (this._selectedToolGroup == null)
          return false;
        for (int index = 0; index < this._selectedToolGroup.Length; ++index) {
          if (this._selectedToolGroup[index].IconBound.Contains(RiftViewportConduit.CursorInLeftViewport))
            RhinoApp.SendKeystrokes(this._selectedToolGroup[index].EnglishName, true);
        }
      }
      this._selectedToolGroup = (RhinoTool[])null;
      this._voiceOn = true;
      VoiceController.StopVoice();
      return true;
    }
  }
}
