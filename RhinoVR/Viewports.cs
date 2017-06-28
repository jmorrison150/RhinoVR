// Decompiled with JetBrains decompiler
// Type: RhinoVR.Viewports
// Assembly: RhinoVR, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0F066D18-B920-40E4-BC83-5E6F0AA166E5
// Assembly location: C:\Program Files\Rhinoceros 5 (64-bit)\Plug-ins\RhinoVR.dll

using Rhino;
using Rhino.Display;
using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RhinoVR {
  internal class Viewports {
    private static string[] _riftViewportTitles = new string[3];
    private static RhinoView[] _riftViews = new RhinoView[3];
    private static ViewportInfo[] _riftViewportInfo = new ViewportInfo[3];
    private static string[] _miniViewportTitles = new string[3];
    private static RhinoView[] _miniViews = new RhinoView[3];
    private static ViewportInfo[] _miniViewportInfo = new ViewportInfo[3];
    private static bool _objLocked = false;
    private static int _riftHor = 1920;
    private static int _riftVer = 1080;
    private static int _miniHor = 600;
    private static int _miniVer = 400;
    private static int _eyeScreenX = 0;
    private static int _eyeScreenY = 0;

    public static string[] RiftViewportTitles {
      get {
        return Viewports._riftViewportTitles;
      }
    }

    public static RhinoView[] RiftViews {
      get {
        return Viewports._riftViews;
      }
    }

    public static string[] MiniViewportTitles {
      get {
        return Viewports._miniViewportTitles;
      }
    }

    public static RhinoView[] MiniViews {
      get {
        return Viewports._miniViews;
      }
    }

    public static ViewportInfo[] MiniViewportInfo {
      get {
        return Viewports._miniViewportInfo;
      }
    }

    public static void InitViewports() {
      for (int index = 0; index < 3; ++index) {
        Viewports._riftViews[index] = (RhinoView)null;
        Viewports._miniViews[index] = (RhinoView)null;
        Viewports._riftViewportInfo[index] = (ViewportInfo)null;
        Viewports._miniViewportInfo[index] = (ViewportInfo)null;
        switch (index) {
          case 0:
            Viewports._riftViewportTitles[0] = "Left Eye";
            Viewports._miniViewportTitles[0] = "Mini Left Eye";
            break;
          case 1:
            Viewports._riftViewportTitles[1] = "Right Eye";
            Viewports._miniViewportTitles[1] = "Mini Right Eye";
            break;
          case 2:
            Viewports._riftViewportTitles[2] = "Mirror";
            Viewports._miniViewportTitles[2] = "Mini Mirror Eye";
            break;
        }
      }
    }

    public static void UpdateDisplayMode(int indexOfDisplayMode, DisplayModeDescription[] displayModes) {
      if (indexOfDisplayMode == -1)
        return;
      for (int index = 0; index < 2; ++index)
        Viewports._riftViews[index].MainViewport.DisplayMode = (displayModes[indexOfDisplayMode]);
      if (Viewports._riftViews[2] == null)
        return;
      Viewports._riftViews[2].MainViewport.DisplayMode = (displayModes[indexOfDisplayMode]);
    }

    public static bool CreateEyeViewports(double[] fov_L, double[] fov_R) {
      if (Screen.AllScreens.Length == 1) {
        int num = (int)MessageBox.Show("Please check if Oculus is connected in EXTENDED mode, If it was, try RESTART Rhino", "Only one Screen", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (Screen.AllScreens.Length == 3) {
        int num = (int)MessageBox.Show("Too many screens are extended", "Too many screens", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (Screen.AllScreens.Length == 2) {
        foreach (Screen allScreen in Screen.AllScreens) {
          if (!allScreen.Primary) {
            Viewports._eyeScreenX = Convert.ToInt32(allScreen.WorkingArea.Left);
            Viewports._eyeScreenY = Convert.ToInt32(allScreen.WorkingArea.Top);
          }
        }
      }
      for (int index = 0; index < 2; ++index) {
        Viewports._riftViews[index] = RhinoDocument.ActiveDoc.Views.Find(Viewports._riftViewportTitles[index], false);
        if (Viewports._riftViews[index] == null)
          Viewports._riftViews[index] = RhinoDocument.ActiveDoc.Views.Add(Viewports._riftViewportTitles[index], (DefinedViewportProjection)7, new Rectangle(new System.Drawing.Point(Viewports._eyeScreenX + Viewports._riftHor / 2 * index, Viewports._eyeScreenY), new Size(Viewports._riftHor / 2, Viewports._riftVer)), true);
        Viewports._riftViews[index].TitleVisible = (false);
        Viewports._riftViews[index].MainViewport.WorldAxesVisible = (true);
        Viewports._riftViewportInfo[index] = new ViewportInfo(Viewports._riftViews[index].MainViewport);
        Viewports._riftViewportInfo[index].Camera35mmLensLength = (50.0);
        Viewports._riftViewportInfo[index].CameraAngle = (fov_L[index] + fov_R[index]) / 2.0;
        double num1;
        double num2;
        double num3;
        double num4;
        double num5;
        double num6;
        if (!Viewports._riftViewportInfo[index].GetFrustum(out num1, out num2, out num3, out num4, out num5, out num6))
          return false;
        int num7 = 1;
        int num8 = 1;
        switch (index) {
          case 0:
            num7 = -1;
            num8 = 1;
            break;
          case 1:
            num7 = 1;
            num8 = -1;
            break;
        }
        double num9 = (fov_L[index] * num1 + fov_R[index] * num2) / (fov_L[index] + fov_R[index]) * (double)num7;
        num1 += num9 * (double)num8;
        num2 += num9 * (double)num8;
        Viewports._riftViewportInfo[index].UnlockFrustumSymmetry();
        Viewports._riftViewportInfo[index].SetFrustum(num1, num2, num3, num4, num5, num6);
      }
      return true;
    }

    public static void CreateMirrorViewport(double[] fov_L, double[] fov_R) {
      if (Viewports._riftViews[2] != null)
        return;
      int x = 0;
      int y = 0;
      Viewports._riftViews[2] = RhinoDocument.ActiveDoc.Views.Find(Viewports._riftViewportTitles[2], false);
      if (Viewports._riftViews[2] == null)
        Viewports._riftViews[2] = RhinoDocument.ActiveDoc.Views.Add(Viewports._riftViewportTitles[2], (DefinedViewportProjection)7, new Rectangle(new System.Drawing.Point(x, y), new Size(Viewports._riftHor / 2, Viewports._riftVer)), true);
      Viewports._riftViews[2].TitleVisible = (false);
      Viewports._riftViews[2].MainViewport.WorldAxesVisible = (true);
      Viewports._riftViewportInfo[2] = new ViewportInfo(Viewports._riftViews[2].MainViewport);
      Viewports._riftViewportInfo[2].Camera35mmLensLength = (50.0);
      Viewports._riftViewportInfo[2].CameraAngle = ((fov_L[0] + fov_R[0]) / 2.0);
      double num1;
      double num2;
      double num3;
      double num4;
      double num5;
      double num6;
      if (!Viewports._riftViewportInfo[2].GetFrustum(out num1, out num2, out num3, out num4, out num5, out num6))
        return;
      int num7 = -1;
      int num8 = 1;
      double num9 = (fov_L[0] * num1 + fov_R[0] * num2) / (fov_L[0] + fov_R[0]) * (double)num7;
      double num10 = num1 + num9 * (double)num8;
      num2 += num9 * (double)num8;
      Viewports._riftViewportInfo[2].UnlockFrustumSymmetry();
      Viewports._riftViewportInfo[2].SetFrustum(num10, num2, num3, num4, num5, num6);
      Viewports._riftViews[2].MainViewport.DisplayMode = (Viewports._riftViews[0].MainViewport.DisplayMode);
    }

    public static void CreateMiniViewports() {
      DisplayModeDescription displayModeDescription = (DisplayModeDescription)null;
      foreach (DisplayModeDescription displayMode in VR_PANEL.DisplayModes) {
        if (displayMode.EnglishName == "VrForModeling")
          displayModeDescription = displayMode;
      }
      for (int index = 0; index < 3; ++index) {
        if (Viewports._miniViews[index] == null) {
          Viewports._miniViews[index] = RhinoDocument.ActiveDoc.Views.Find(Viewports._miniViewportTitles[index], false);
          if (index < 2) {
            if (Viewports._miniViews[index] == null)
              Viewports._miniViews[index] = RhinoDocument.ActiveDoc.Views.Add(Viewports._miniViewportTitles[index], (DefinedViewportProjection)7, new Rectangle(new System.Drawing.Point(Viewports._eyeScreenX + (Viewports._riftHor / 2 - Viewports._miniHor) / 2 + Viewports._riftHor / 2 * index, Viewports._eyeScreenY + (Viewports._riftVer - Viewports._miniVer) / 2), new Size(Viewports._miniHor, Viewports._miniVer)), true);
          } else if (Viewports._miniViews[index] == null)
            Viewports._miniViews[index] = RhinoDocument.ActiveDoc.Views.Add(Viewports._miniViewportTitles[index], (DefinedViewportProjection)7, new Rectangle(new System.Drawing.Point((Viewports._riftHor / 2 - Viewports._miniHor) / 2, (Viewports._riftVer - Viewports._miniVer) / 2), new Size(Viewports._miniHor, Viewports._miniVer)), true);
          Viewports._miniViews[index].TitleVisible = (false);
          Viewports._miniViews[index].MainViewport.WorldAxesVisible = (true);
          Viewports._miniViews[index].MainViewport.DisplayMode = (displayModeDescription);
          Viewports._miniViewportInfo[index] = new ViewportInfo(Viewports._miniViews[index].MainViewport);
        }
      }
      if (!Viewports._miniViews[2].MainViewport.ZoomExtentsSelected()) {
        Viewports._miniViews[2].MainViewport.ZoomExtents();
      } else {
        Viewports._objLocked = true;
        RhinoApp.SendKeystrokes("_invert _lock", true);
      }
    }

    public static void UpdateEyeViewports(Point3d[] camLoc, Vector3d[] camDir, Vector3d[] camUp) {
      for (int index = 0; index < 2; ++index) {
        if (Viewports._riftViews[index] != null && Viewports._riftViews[index].Document != null) {
          Viewports._riftViewportInfo[index].SetCameraLocation(camLoc[index]);
          Viewports._riftViewportInfo[index].SetCameraDirection(camDir[index]);
          Viewports._riftViewportInfo[index].SetCameraUp(camUp[index]);
          if (!Viewports._riftViews[index].MainViewport.SetViewProjection(Viewports._riftViewportInfo[index], true))
            break;
        }
      }
    }

    public static int UpdateMirrorViewport(Point3d[] camLoc, Vector3d[] camDir, Vector3d[] camUp) {
      if (Viewports._riftViews[2] != null && Viewports._riftViews[2].Document != null) {
        Viewports._riftViewportInfo[2].SetCameraLocation(camLoc[0]);
        Viewports._riftViewportInfo[2].SetCameraDirection(camDir[0]);
        Viewports._riftViewportInfo[2].SetCameraUp(camUp[0]);
        return !Viewports._riftViews[2].MainViewport.SetViewProjection(Viewports._riftViewportInfo[2], true) ? -2 : 1;
      }
      return Viewports._riftViews[2] != null && Viewports._riftViews[2].Document == null ? 0 : -1;
    }

    public static void UpdateMiniViewports() {
      for (int index = 0; index < 2; ++index) {
        if (Viewports._miniViews[index] != null && Viewports._miniViews[index].Document != null) {
          Viewports._miniViewportInfo[index].SetCameraLocation(Viewports._miniViews[2].MainViewport.CameraLocation);
          Viewports._miniViewportInfo[index].SetCameraDirection(Viewports._miniViews[2].MainViewport.CameraDirection);
          Viewports._miniViewportInfo[index].SetCameraUp(Viewports._miniViews[2].MainViewport.CameraUp);
          if (!Viewports._miniViews[index].MainViewport.SetViewProjection(Viewports._miniViewportInfo[index], true))
            break;
        }
      }
    }

    public static void CloseEyeViewports() {
      for (int index = 0; index < 2; ++index) {
        if (Viewports._riftViews[index] != null) {
          if (Viewports._riftViews[index].Document != null)
            Viewports._riftViews[index].Close();
          Viewports._riftViewportInfo[index].Dispose();
          Viewports._riftViewportInfo[index] = (ViewportInfo)null;
          Viewports._riftViews[index] = (RhinoView)null;
        }
      }
    }

    public static void CloseMirrorViewport() {
      if (Viewports._riftViews[2] == null)
        return;
      if (Viewports._riftViews[2].Document != null)
        Viewports._riftViews[2].Close();
      Viewports._riftViewportInfo[2].Dispose();
      Viewports._riftViewportInfo[2] = (ViewportInfo)null;
      Viewports._riftViews[2] = (RhinoView)null;
    }

    public static void CloseMiniViewports() {
      for (int index = 0; index < 3; ++index) {
        if (Viewports._miniViews[index] != null) {
          if (Viewports._miniViews[index].Document != null)
            Viewports._miniViews[index].Close();
          if (Viewports._objLocked) {
            RhinoApp.SendKeystrokes("unlock", true);
            Viewports._objLocked = false;
          }
          Viewports._miniViewportInfo[index].Dispose();
          Viewports._miniViewportInfo[index] = (ViewportInfo)null;
          Viewports._miniViews[index] = (RhinoView)null;
        }
      }
    }
  }
}
