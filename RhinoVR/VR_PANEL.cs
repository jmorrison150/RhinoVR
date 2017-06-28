// Decompiled with JetBrains decompiler
// Type: RhinoVR.VR_PANEL
// Assembly: RhinoVR, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0F066D18-B920-40E4-BC83-5E6F0AA166E5
// Assembly location: C:\Program Files\Rhinoceros 5 (64-bit)\Plug-ins\RhinoVR.dll

using Rhino;
using Rhino.Display;
//using Rhino.Geometry;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RhinoVR {
  [Guid("A57419F7-2F26-4560-95F2-E32256A7D5F1")]
  public class VR_PANEL : UserControl {
    private static double _eyeHeight = 1.6;
    private readonly RiftViewportConduit _riftViewportConduit = new RiftViewportConduit();
    private MyoController _myoController = new MyoController();
    private bool _displayModeChanged = true;
    private int _indexOfDisplayMode = -1;
    private bool _run;
    private static DisplayModeDescription[] _displayModes;
    private static double _latitude;
    private static double _longitude;
    private IContainer components;
    private Button START_VR;
    private Button STOP_VR;
    private Label LBL_CAM_LOC_R;
    private Label LBL_CAM_LOC_L;
    private Label CAM_LOC_R;
    private Label CAM_LOC_L;
    private Label CAM_DIR_R;
    private Label CAM_DIR_L;
    private Label CAM_UP_R;
    private Label CAM_UP_L;
    private Label FIELD_OF_VIEW_R;
    private Label FIELD_OF_VIEW_L;
    private Label LBL_CAM_DIR_L;
    private Label LBL_CAM_DIR_R;
    private Label LBL_CAM_UP_L;
    private Label LBL_CAM_UP_R;
    private Label LBL_FIELD_OF_VIEW_L;
    private Label LBL_FIELD_OF_VIEW_R;
    private GroupBox GROP_TRAC_INFO_CNSL;
    private TableLayoutPanel Tb_TRAC_INFO_CNSL;
    private Label LB_CAM_LOC;
    private Label LB_CAM_DIR;
    private Label LB_CAM_UP;
    private Label LB_CAM_FOV;
    private GroupBox GROP_VR_OPTN;
    private TableLayoutPanel TB_VR_OPTN;
    private Label LB_EYE_HEIGHT;
    private Label LB_START_POSITION;
    private Label LB_FLOORS;
    private Button START_POSITION;
    private Button START_FLOORS;
    private TextBox EYE_HEIGHT;
    private CheckBox CB_MIRROR;
    private Label LB_MIRROR;
    private Label LB_VOICE;
    private CheckBox CB_VOICE;
    private CheckBox CB_MYO;
    private Label LB_MYO;
    private Label LB_XBOX;
    private CheckBox CB_XBOX;
    private Label LB_DISPLAY_MODE;
    private ComboBox DISPLAY_MODE;
    private Label LB_USER_OPTN;
    private Label LB_INPT_OPTN;
    private Label LB_DSPL_OPTN;
    private LinkLabel LB_LING_DOT_WORLD;
    private Label label1;
    private Label LB_LONGITUDE;
    private Label LB_GEO_LOCATION;
    private Label LB_LATITUDE;
    private TextBox LATITUDE;
    private TextBox LONGITUDE;

    public static double EyeHeight {
      get {
        return VR_PANEL._eyeHeight;
      }
    }

    public static DisplayModeDescription[] DisplayModes {
      get {
        return VR_PANEL._displayModes;
      }
    }

    public static double Latitude {
      get {
        return VR_PANEL._latitude;
      }
    }

    public static double Longitude {
      get {
        return VR_PANEL._longitude;
      }
    }

    public VR_PANEL() {
      this.InitializeComponent();
      VR_PANEL._displayModes = DisplayModeDescription.GetDisplayModes();
      foreach (DisplayModeDescription displayMode in VR_PANEL._displayModes)
        this.DISPLAY_MODE.Items.Add((object)displayMode.EnglishName);
      this.CB_XBOX.Checked = UserInput.XBoxIsConnected();
      VR_PANEL._latitude = double.Parse(this.LATITUDE.Text);
      VR_PANEL._longitude = double.Parse(this.LONGITUDE.Text);
      VoiceController.InitVoiceController();
    }

    private void ConsoleDefault() {
      this.START_VR.Text = "Start VR";
      this.START_VR.ForeColor = Control.DefaultForeColor;
      this.CAM_LOC_L.Text = "0.000, 0.000, 0.000";
      this.CAM_LOC_R.Text = "0.000, 0.000, 0.000";
      this.CAM_DIR_L.Text = "0.000, 0.000, 0.000";
      this.CAM_DIR_R.Text = "0.000, 0.000, 0.000";
      this.CAM_UP_L.Text = "0.000, 0.000, 0.000";
      this.CAM_UP_R.Text = "0.000, 0.000, 0.000";
      this.FIELD_OF_VIEW_L.Text = "0.000, 0.000";
      this.FIELD_OF_VIEW_R.Text = "0.000, 0.000";
    }

    private void ConsoleDisplay(Rhino.Geometry.Point3d[] camLoc, Rhino.Geometry.Vector3d[] camDir, Rhino.Geometry.Vector3d[] camUp, double[] fovL, double[] fovR) {

      this.CAM_LOC_L.Text = string.Format("{0:F3}, {1:F3}, {2:F3}", (object)((Rhino.Geometry.Point3d)@camLoc[0]).X, (object)((Rhino.Geometry.Point3d)@camLoc[0]).Y, (object)((Rhino.Geometry.Point3d)@camLoc[0]).Z);
      this.CAM_LOC_R.Text = string.Format("{0:F3}, {1:F3}, {2:F3}", (object)((Rhino.Geometry.Point3d)@camLoc[1]).X, (object)((Rhino.Geometry.Point3d)@camLoc[1]).Y, (object)((Rhino.Geometry.Point3d)@camLoc[1]).Z);
      this.CAM_DIR_L.Text = string.Format("{0:F3}, {1:F3}, {2:F3}", (object)((Rhino.Geometry.Vector3d)@camDir[0]).X, (object)((Rhino.Geometry.Vector3d)@camDir[0]).Y, (object)((Rhino.Geometry.Vector3d)@camDir[0]).Z);
      this.CAM_DIR_R.Text = string.Format("{0:F3}, {1:F3}, {2:F3}", (object)((Rhino.Geometry.Vector3d)@camDir[1]).X, (object)((Rhino.Geometry.Vector3d)@camDir[1]).Y, (object)((Rhino.Geometry.Vector3d)@camDir[1]).Z);
      this.CAM_UP_L.Text = string.Format("{0:F3}, {1:F3}, {2:F3}", (object)((Rhino.Geometry.Vector3d)@camUp[0]).X, (object)((Rhino.Geometry.Vector3d)@camUp[0]).Y, (object)((Rhino.Geometry.Vector3d)@camUp[0]).Z);
      this.CAM_UP_R.Text = string.Format("{0:F3}, {1:F3}, {2:F3}", (object)((Rhino.Geometry.Vector3d)@camUp[1]).X, (object)((Rhino.Geometry.Vector3d)@camUp[1]).Y, (object)((Rhino.Geometry.Vector3d)@camUp[1]).Z);
      this.FIELD_OF_VIEW_L.Text = string.Format("{0:F3}, {1:F3}", (object)fovL[0], (object)fovR[0]);
      this.FIELD_OF_VIEW_R.Text = string.Format("{0:F3}, {1:F3}", (object)fovL[1], (object)fovR[1]);
    }

    private void START_VR_Click(object sender, EventArgs e) {
      if (this.START_VR.Text != "Running...") {
        this.START_VR.Text = "Running...";
        this.START_VR.ForeColor = Color.Red;
        this._run = true;
        if (!OculusTracking.CreatOculusHmd())
          return;
        OculusTracking.SetupTracking();
        Viewports.InitViewports();
        if (!Viewports.CreateEyeViewports(OculusTracking.FovL, OculusTracking.FovR))
          return;
        this._riftViewportConduit.Enabled = (true);
      }
      while (this._run) {
        UserInput.SunActive();
        UserInput.MiniViewportsActive();
        if (this.CB_MIRROR.Checked)
          Viewports.CreateMirrorViewport(OculusTracking.FovL, OculusTracking.FovR);
        this._indexOfDisplayMode = this.DISPLAY_MODE.SelectedIndex;
        UserInput.ChangeDisplayMode(ref this._indexOfDisplayMode, VR_PANEL._displayModes.Length);
        this.DISPLAY_MODE.SelectedIndex = this._indexOfDisplayMode;
        if (this._displayModeChanged) {
          Viewports.UpdateDisplayMode(this._indexOfDisplayMode, VR_PANEL._displayModes);
          this._displayModeChanged = false;
        }
        if (this.CB_MIRROR.Checked && this._riftViewportConduit.MirrorClosed == 0)
          this.CB_MIRROR.Checked = false;
        for (int index = 0; index < Viewports.RiftViews.Length; ++index) {
          if (Viewports.RiftViews[index] != null && Viewports.RiftViews[index].Document != null)
            Viewports.RiftViews[index].Redraw();
        }
        if (UserInput.MiniViewportsOn) {
          for (int index = 0; index < 3; ++index) {
            if (Viewports.MiniViews[index] != null && Viewports.MiniViews[index].Document != null)
              Viewports.MiniViews[index].Redraw();
          }
        }
        this.ConsoleDisplay(OculusTracking.CamLoc, OculusTracking.CamDir, OculusTracking.CamUp, OculusTracking.FovL, OculusTracking.FovR);
        RhinoApp.Wait();
      }
      OculusTracking.StopTracking();
      Viewports.CloseEyeViewports();
      Viewports.CloseMirrorViewport();
      Viewports.CloseMiniViewports();
    }

    private void STOP_VR_Click(object sender, EventArgs e) {
      this._run = false;
      this._riftViewportConduit.Enabled = (false);
      this.START_VR.Text = "Start VR";
      this.ConsoleDefault();
    }

    private void START_POSITION_Click(object sender, EventArgs e) {
      UserInput.GetStartPosition();
    }

    private void START_FLOORS_Click(object sender, EventArgs e) {
      UserInput.GetFloors(RhinoDocument.ActiveDoc);
    }

    private void DISPLAY_MODE_SelectedIndexChanged(object sender, EventArgs e) {
      this._displayModeChanged = true;
    }

    private void EYE_HEIGHT_TextChanged(object sender, EventArgs e) {
      VR_PANEL._eyeHeight = double.Parse(this.EYE_HEIGHT.Text);
    }

    private void CB_MIRROR_CheckedChanged(object sender, EventArgs e) {
      if (this.CB_MIRROR.Checked)
        return;
      Viewports.CloseMirrorViewport();
    }

    private void CB_MYO_CheckedChanged(object sender, EventArgs e) {
      if (this.CB_MYO.Checked)
        this._myoController.StartMyo();
      if (this.CB_MYO.Checked)
        return;
      this._myoController.StopMyo();
    }

    private void CB_VOICE_CheckedChanged(object sender, EventArgs e) {
      if (this.CB_VOICE.Checked)
        VoiceController.InitVoiceController();
      if (this.CB_VOICE.Checked)
        return;
      VoiceController.DinitVoiceController();
    }

    private void LATITUDE_TextChanged(object sender, EventArgs e) {
      VR_PANEL._latitude = double.Parse(this.LATITUDE.Text);
    }

    private void LONGITUDE_TextChanged(object sender, EventArgs e) {
      VR_PANEL._longitude = double.Parse(this.LONGITUDE.Text);
    }

    protected override void Dispose(bool disposing) {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      this.START_VR = new Button();
      this.STOP_VR = new Button();
      this.LBL_CAM_LOC_R = new Label();
      this.LBL_CAM_LOC_L = new Label();
      this.CAM_LOC_R = new Label();
      this.CAM_LOC_L = new Label();
      this.CAM_DIR_R = new Label();
      this.CAM_DIR_L = new Label();
      this.CAM_UP_R = new Label();
      this.CAM_UP_L = new Label();
      this.FIELD_OF_VIEW_R = new Label();
      this.FIELD_OF_VIEW_L = new Label();
      this.LBL_CAM_DIR_L = new Label();
      this.LBL_CAM_DIR_R = new Label();
      this.LBL_CAM_UP_L = new Label();
      this.LBL_CAM_UP_R = new Label();
      this.LBL_FIELD_OF_VIEW_L = new Label();
      this.LBL_FIELD_OF_VIEW_R = new Label();
      this.GROP_TRAC_INFO_CNSL = new GroupBox();
      this.Tb_TRAC_INFO_CNSL = new TableLayoutPanel();
      this.LB_CAM_LOC = new Label();
      this.LB_CAM_UP = new Label();
      this.LB_CAM_DIR = new Label();
      this.LB_CAM_FOV = new Label();
      this.GROP_VR_OPTN = new GroupBox();
      this.TB_VR_OPTN = new TableLayoutPanel();
      this.LB_LONGITUDE = new Label();
      this.LB_USER_OPTN = new Label();
      this.LB_VOICE = new Label();
      this.CB_VOICE = new CheckBox();
      this.LB_MYO = new Label();
      this.EYE_HEIGHT = new TextBox();
      this.CB_MYO = new CheckBox();
      this.LB_XBOX = new Label();
      this.CB_XBOX = new CheckBox();
      this.LB_INPT_OPTN = new Label();
      this.CB_MIRROR = new CheckBox();
      this.LB_MIRROR = new Label();
      this.LB_DISPLAY_MODE = new Label();
      this.DISPLAY_MODE = new ComboBox();
      this.LB_EYE_HEIGHT = new Label();
      this.LB_FLOORS = new Label();
      this.START_FLOORS = new Button();
      this.START_POSITION = new Button();
      this.LB_START_POSITION = new Label();
      this.LB_DSPL_OPTN = new Label();
      this.LB_GEO_LOCATION = new Label();
      this.LB_LATITUDE = new Label();
      this.LATITUDE = new TextBox();
      this.LONGITUDE = new TextBox();
      this.LB_LING_DOT_WORLD = new LinkLabel();
      this.label1 = new Label();
      this.GROP_TRAC_INFO_CNSL.SuspendLayout();
      this.Tb_TRAC_INFO_CNSL.SuspendLayout();
      this.GROP_VR_OPTN.SuspendLayout();
      this.TB_VR_OPTN.SuspendLayout();
      this.SuspendLayout();
      this.START_VR.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.START_VR.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.START_VR.Location = new Point(5, 5);
      this.START_VR.Name = "START_VR";
      this.START_VR.Size = new Size(190, 30);
      this.START_VR.TabIndex = 0;
      this.START_VR.Text = "Start VR";
      this.START_VR.UseVisualStyleBackColor = true;
      this.START_VR.Click += new EventHandler(this.START_VR_Click);
      this.STOP_VR.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.STOP_VR.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.STOP_VR.Location = new Point(5, 40);
      this.STOP_VR.Name = "STOP_VR";
      this.STOP_VR.Size = new Size(190, 30);
      this.STOP_VR.TabIndex = 1;
      this.STOP_VR.Text = "Stop VR";
      this.STOP_VR.UseVisualStyleBackColor = true;
      this.STOP_VR.Click += new EventHandler(this.STOP_VR_Click);
      this.LBL_CAM_LOC_R.AutoSize = true;
      this.LBL_CAM_LOC_R.Dock = DockStyle.Fill;
      this.LBL_CAM_LOC_R.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
      this.LBL_CAM_LOC_R.ForeColor = SystemColors.ControlDarkDark;
      this.LBL_CAM_LOC_R.Location = new Point(4, 31);
      this.LBL_CAM_LOC_R.Name = "LBL_CAM_LOC_R";
      this.LBL_CAM_LOC_R.Size = new Size(57, 14);
      this.LBL_CAM_LOC_R.TabIndex = 13;
      this.LBL_CAM_LOC_R.Text = "Right Eye";
      this.LBL_CAM_LOC_R.TextAlign = ContentAlignment.MiddleLeft;
      this.LBL_CAM_LOC_L.AutoSize = true;
      this.LBL_CAM_LOC_L.Dock = DockStyle.Fill;
      this.LBL_CAM_LOC_L.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
      this.LBL_CAM_LOC_L.ForeColor = SystemColors.ControlDarkDark;
      this.LBL_CAM_LOC_L.Location = new Point(4, 16);
      this.LBL_CAM_LOC_L.Name = "LBL_CAM_LOC_L";
      this.LBL_CAM_LOC_L.Size = new Size(57, 14);
      this.LBL_CAM_LOC_L.TabIndex = 12;
      this.LBL_CAM_LOC_L.Text = "Left Eye";
      this.LBL_CAM_LOC_L.TextAlign = ContentAlignment.MiddleLeft;
      this.CAM_LOC_R.AutoSize = true;
      this.CAM_LOC_R.Dock = DockStyle.Fill;
      this.CAM_LOC_R.Location = new Point(68, 31);
      this.CAM_LOC_R.Name = "CAM_LOC_R";
      this.CAM_LOC_R.Size = new Size(112, 14);
      this.CAM_LOC_R.TabIndex = 11;
      this.CAM_LOC_R.Text = "0.000, 0.000, 0.000";
      this.CAM_LOC_R.TextAlign = ContentAlignment.MiddleLeft;
      this.CAM_LOC_L.AutoSize = true;
      this.CAM_LOC_L.Dock = DockStyle.Fill;
      this.CAM_LOC_L.Location = new Point(68, 16);
      this.CAM_LOC_L.Name = "CAM_LOC_L";
      this.CAM_LOC_L.Size = new Size(112, 14);
      this.CAM_LOC_L.TabIndex = 10;
      this.CAM_LOC_L.Text = "0.000, 0.000, 0.000";
      this.CAM_LOC_L.TextAlign = ContentAlignment.MiddleLeft;
      this.CAM_DIR_R.AutoSize = true;
      this.CAM_DIR_R.Dock = DockStyle.Fill;
      this.CAM_DIR_R.Location = new Point(68, 76);
      this.CAM_DIR_R.Name = "CAM_DIR_R";
      this.CAM_DIR_R.Size = new Size(112, 14);
      this.CAM_DIR_R.TabIndex = 11;
      this.CAM_DIR_R.Text = "0.000, 0.000, 0.000";
      this.CAM_DIR_R.TextAlign = ContentAlignment.MiddleLeft;
      this.CAM_DIR_L.AutoSize = true;
      this.CAM_DIR_L.Dock = DockStyle.Fill;
      this.CAM_DIR_L.Location = new Point(68, 61);
      this.CAM_DIR_L.Name = "CAM_DIR_L";
      this.CAM_DIR_L.Size = new Size(112, 14);
      this.CAM_DIR_L.TabIndex = 10;
      this.CAM_DIR_L.Text = "0.000, 0.000, 0.000";
      this.CAM_DIR_L.TextAlign = ContentAlignment.MiddleLeft;
      this.CAM_UP_R.AutoSize = true;
      this.CAM_UP_R.Dock = DockStyle.Fill;
      this.CAM_UP_R.Location = new Point(68, 121);
      this.CAM_UP_R.Name = "CAM_UP_R";
      this.CAM_UP_R.Size = new Size(112, 14);
      this.CAM_UP_R.TabIndex = 11;
      this.CAM_UP_R.Text = "0.000, 0.000, 0.000";
      this.CAM_UP_R.TextAlign = ContentAlignment.MiddleLeft;
      this.CAM_UP_L.AutoSize = true;
      this.CAM_UP_L.Dock = DockStyle.Fill;
      this.CAM_UP_L.Location = new Point(68, 106);
      this.CAM_UP_L.Name = "CAM_UP_L";
      this.CAM_UP_L.Size = new Size(112, 14);
      this.CAM_UP_L.TabIndex = 10;
      this.CAM_UP_L.Text = "0.000, 0.000, 0.000";
      this.CAM_UP_L.TextAlign = ContentAlignment.MiddleLeft;
      this.FIELD_OF_VIEW_R.AutoSize = true;
      this.FIELD_OF_VIEW_R.Dock = DockStyle.Fill;
      this.FIELD_OF_VIEW_R.Location = new Point(68, 166);
      this.FIELD_OF_VIEW_R.Name = "FIELD_OF_VIEW_R";
      this.FIELD_OF_VIEW_R.Size = new Size(112, 14);
      this.FIELD_OF_VIEW_R.TabIndex = 11;
      this.FIELD_OF_VIEW_R.Text = "0.000, 0.000";
      this.FIELD_OF_VIEW_R.TextAlign = ContentAlignment.MiddleLeft;
      this.FIELD_OF_VIEW_L.AutoSize = true;
      this.FIELD_OF_VIEW_L.Dock = DockStyle.Fill;
      this.FIELD_OF_VIEW_L.Location = new Point(68, 151);
      this.FIELD_OF_VIEW_L.Name = "FIELD_OF_VIEW_L";
      this.FIELD_OF_VIEW_L.Size = new Size(112, 14);
      this.FIELD_OF_VIEW_L.TabIndex = 10;
      this.FIELD_OF_VIEW_L.Text = "0.000, 0.000";
      this.FIELD_OF_VIEW_L.TextAlign = ContentAlignment.MiddleLeft;
      this.LBL_CAM_DIR_L.AutoSize = true;
      this.LBL_CAM_DIR_L.Dock = DockStyle.Fill;
      this.LBL_CAM_DIR_L.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
      this.LBL_CAM_DIR_L.ForeColor = SystemColors.ControlDarkDark;
      this.LBL_CAM_DIR_L.Location = new Point(4, 61);
      this.LBL_CAM_DIR_L.Name = "LBL_CAM_DIR_L";
      this.LBL_CAM_DIR_L.Size = new Size(57, 14);
      this.LBL_CAM_DIR_L.TabIndex = 12;
      this.LBL_CAM_DIR_L.Text = "Left Eye";
      this.LBL_CAM_DIR_L.TextAlign = ContentAlignment.MiddleLeft;
      this.LBL_CAM_DIR_R.AutoSize = true;
      this.LBL_CAM_DIR_R.Dock = DockStyle.Fill;
      this.LBL_CAM_DIR_R.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
      this.LBL_CAM_DIR_R.ForeColor = SystemColors.ControlDarkDark;
      this.LBL_CAM_DIR_R.Location = new Point(4, 76);
      this.LBL_CAM_DIR_R.Name = "LBL_CAM_DIR_R";
      this.LBL_CAM_DIR_R.Size = new Size(57, 14);
      this.LBL_CAM_DIR_R.TabIndex = 13;
      this.LBL_CAM_DIR_R.Text = "Right Eye";
      this.LBL_CAM_DIR_R.TextAlign = ContentAlignment.MiddleLeft;
      this.LBL_CAM_UP_L.AutoSize = true;
      this.LBL_CAM_UP_L.Dock = DockStyle.Fill;
      this.LBL_CAM_UP_L.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
      this.LBL_CAM_UP_L.ForeColor = SystemColors.ControlDarkDark;
      this.LBL_CAM_UP_L.Location = new Point(4, 106);
      this.LBL_CAM_UP_L.Name = "LBL_CAM_UP_L";
      this.LBL_CAM_UP_L.Size = new Size(57, 14);
      this.LBL_CAM_UP_L.TabIndex = 12;
      this.LBL_CAM_UP_L.Text = "Left Eye";
      this.LBL_CAM_UP_L.TextAlign = ContentAlignment.MiddleLeft;
      this.LBL_CAM_UP_R.AutoSize = true;
      this.LBL_CAM_UP_R.Dock = DockStyle.Fill;
      this.LBL_CAM_UP_R.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
      this.LBL_CAM_UP_R.ForeColor = SystemColors.ControlDarkDark;
      this.LBL_CAM_UP_R.Location = new Point(4, 121);
      this.LBL_CAM_UP_R.Name = "LBL_CAM_UP_R";
      this.LBL_CAM_UP_R.Size = new Size(57, 14);
      this.LBL_CAM_UP_R.TabIndex = 13;
      this.LBL_CAM_UP_R.Text = "Right Eye";
      this.LBL_CAM_UP_R.TextAlign = ContentAlignment.MiddleLeft;
      this.LBL_FIELD_OF_VIEW_L.AutoSize = true;
      this.LBL_FIELD_OF_VIEW_L.Dock = DockStyle.Fill;
      this.LBL_FIELD_OF_VIEW_L.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
      this.LBL_FIELD_OF_VIEW_L.ForeColor = SystemColors.ControlDarkDark;
      this.LBL_FIELD_OF_VIEW_L.Location = new Point(4, 151);
      this.LBL_FIELD_OF_VIEW_L.Name = "LBL_FIELD_OF_VIEW_L";
      this.LBL_FIELD_OF_VIEW_L.Size = new Size(57, 14);
      this.LBL_FIELD_OF_VIEW_L.TabIndex = 12;
      this.LBL_FIELD_OF_VIEW_L.Text = "Left Eye";
      this.LBL_FIELD_OF_VIEW_L.TextAlign = ContentAlignment.MiddleLeft;
      this.LBL_FIELD_OF_VIEW_R.AutoSize = true;
      this.LBL_FIELD_OF_VIEW_R.Dock = DockStyle.Fill;
      this.LBL_FIELD_OF_VIEW_R.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
      this.LBL_FIELD_OF_VIEW_R.ForeColor = SystemColors.ControlDarkDark;
      this.LBL_FIELD_OF_VIEW_R.Location = new Point(4, 166);
      this.LBL_FIELD_OF_VIEW_R.Name = "LBL_FIELD_OF_VIEW_R";
      this.LBL_FIELD_OF_VIEW_R.Size = new Size(57, 14);
      this.LBL_FIELD_OF_VIEW_R.TabIndex = 13;
      this.LBL_FIELD_OF_VIEW_R.Text = "Right Eye";
      this.LBL_FIELD_OF_VIEW_R.TextAlign = ContentAlignment.MiddleLeft;
      this.GROP_TRAC_INFO_CNSL.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.GROP_TRAC_INFO_CNSL.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.GROP_TRAC_INFO_CNSL.Controls.Add((Control)this.Tb_TRAC_INFO_CNSL);
      this.GROP_TRAC_INFO_CNSL.Location = new Point(5, 75);
      this.GROP_TRAC_INFO_CNSL.Name = "GROP_TRAC_INFO_CNSL";
      this.GROP_TRAC_INFO_CNSL.Size = new Size(190, 200);
      this.GROP_TRAC_INFO_CNSL.TabIndex = 36;
      this.GROP_TRAC_INFO_CNSL.TabStop = false;
      this.GROP_TRAC_INFO_CNSL.Text = "Camera Tracking Info Console";
      this.Tb_TRAC_INFO_CNSL.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.Tb_TRAC_INFO_CNSL.BackColor = SystemColors.Control;
      this.Tb_TRAC_INFO_CNSL.BackgroundImageLayout = ImageLayout.None;
      this.Tb_TRAC_INFO_CNSL.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
      this.Tb_TRAC_INFO_CNSL.ColumnCount = 2;
      this.Tb_TRAC_INFO_CNSL.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35f));
      this.Tb_TRAC_INFO_CNSL.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65f));
      this.Tb_TRAC_INFO_CNSL.Controls.Add((Control)this.FIELD_OF_VIEW_R, 1, 11);
      this.Tb_TRAC_INFO_CNSL.Controls.Add((Control)this.LBL_FIELD_OF_VIEW_R, 0, 11);
      this.Tb_TRAC_INFO_CNSL.Controls.Add((Control)this.FIELD_OF_VIEW_L, 1, 10);
      this.Tb_TRAC_INFO_CNSL.Controls.Add((Control)this.LBL_FIELD_OF_VIEW_L, 0, 10);
      this.Tb_TRAC_INFO_CNSL.Controls.Add((Control)this.CAM_UP_R, 1, 8);
      this.Tb_TRAC_INFO_CNSL.Controls.Add((Control)this.LBL_CAM_UP_R, 0, 8);
      this.Tb_TRAC_INFO_CNSL.Controls.Add((Control)this.CAM_UP_L, 1, 7);
      this.Tb_TRAC_INFO_CNSL.Controls.Add((Control)this.LBL_CAM_UP_L, 0, 7);
      this.Tb_TRAC_INFO_CNSL.Controls.Add((Control)this.CAM_DIR_R, 1, 5);
      this.Tb_TRAC_INFO_CNSL.Controls.Add((Control)this.LBL_CAM_DIR_R, 0, 5);
      this.Tb_TRAC_INFO_CNSL.Controls.Add((Control)this.CAM_DIR_L, 1, 4);
      this.Tb_TRAC_INFO_CNSL.Controls.Add((Control)this.LBL_CAM_LOC_L, 0, 1);
      this.Tb_TRAC_INFO_CNSL.Controls.Add((Control)this.CAM_LOC_L, 1, 1);
      this.Tb_TRAC_INFO_CNSL.Controls.Add((Control)this.CAM_LOC_R, 1, 2);
      this.Tb_TRAC_INFO_CNSL.Controls.Add((Control)this.LBL_CAM_LOC_R, 0, 2);
      this.Tb_TRAC_INFO_CNSL.Controls.Add((Control)this.LBL_CAM_DIR_L, 0, 4);
      this.Tb_TRAC_INFO_CNSL.Controls.Add((Control)this.LB_CAM_LOC, 0, 0);
      this.Tb_TRAC_INFO_CNSL.Controls.Add((Control)this.LB_CAM_UP, 0, 6);
      this.Tb_TRAC_INFO_CNSL.Controls.Add((Control)this.LB_CAM_DIR, 0, 3);
      this.Tb_TRAC_INFO_CNSL.Controls.Add((Control)this.LB_CAM_FOV, 0, 9);
      this.Tb_TRAC_INFO_CNSL.Dock = DockStyle.Fill;
      this.Tb_TRAC_INFO_CNSL.ForeColor = SystemColors.WindowText;
      this.Tb_TRAC_INFO_CNSL.Location = new Point(3, 16);
      this.Tb_TRAC_INFO_CNSL.Name = "Tb_TRAC_INFO_CNSL";
      this.Tb_TRAC_INFO_CNSL.RowCount = 12;
      this.Tb_TRAC_INFO_CNSL.RowStyles.Add(new RowStyle(SizeType.Percent, 8.333332f));
      this.Tb_TRAC_INFO_CNSL.RowStyles.Add(new RowStyle(SizeType.Percent, 8.333332f));
      this.Tb_TRAC_INFO_CNSL.RowStyles.Add(new RowStyle(SizeType.Percent, 8.333332f));
      this.Tb_TRAC_INFO_CNSL.RowStyles.Add(new RowStyle(SizeType.Percent, 8.333332f));
      this.Tb_TRAC_INFO_CNSL.RowStyles.Add(new RowStyle(SizeType.Percent, 8.333332f));
      this.Tb_TRAC_INFO_CNSL.RowStyles.Add(new RowStyle(SizeType.Percent, 8.333332f));
      this.Tb_TRAC_INFO_CNSL.RowStyles.Add(new RowStyle(SizeType.Percent, 8.333332f));
      this.Tb_TRAC_INFO_CNSL.RowStyles.Add(new RowStyle(SizeType.Percent, 8.333332f));
      this.Tb_TRAC_INFO_CNSL.RowStyles.Add(new RowStyle(SizeType.Percent, 8.333332f));
      this.Tb_TRAC_INFO_CNSL.RowStyles.Add(new RowStyle(SizeType.Percent, 8.333332f));
      this.Tb_TRAC_INFO_CNSL.RowStyles.Add(new RowStyle(SizeType.Percent, 8.333332f));
      this.Tb_TRAC_INFO_CNSL.RowStyles.Add(new RowStyle(SizeType.Percent, 8.333332f));
      this.Tb_TRAC_INFO_CNSL.Size = new Size(184, 181);
      this.Tb_TRAC_INFO_CNSL.TabIndex = 32;
      this.LB_CAM_LOC.AutoSize = true;
      this.Tb_TRAC_INFO_CNSL.SetColumnSpan((Control)this.LB_CAM_LOC, 2);
      this.LB_CAM_LOC.Dock = DockStyle.Fill;
      this.LB_CAM_LOC.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
      this.LB_CAM_LOC.ForeColor = SystemColors.ControlText;
      this.LB_CAM_LOC.Location = new Point(4, 1);
      this.LB_CAM_LOC.Name = "LB_CAM_LOC";
      this.LB_CAM_LOC.Size = new Size(176, 14);
      this.LB_CAM_LOC.TabIndex = 14;
      this.LB_CAM_LOC.Text = "Location";
      this.LB_CAM_LOC.TextAlign = ContentAlignment.MiddleLeft;
      this.LB_CAM_UP.AutoSize = true;
      this.Tb_TRAC_INFO_CNSL.SetColumnSpan((Control)this.LB_CAM_UP, 2);
      this.LB_CAM_UP.Dock = DockStyle.Fill;
      this.LB_CAM_UP.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
      this.LB_CAM_UP.ForeColor = SystemColors.ControlText;
      this.LB_CAM_UP.Location = new Point(4, 91);
      this.LB_CAM_UP.Name = "LB_CAM_UP";
      this.LB_CAM_UP.Size = new Size(176, 14);
      this.LB_CAM_UP.TabIndex = 16;
      this.LB_CAM_UP.Text = "Up";
      this.LB_CAM_UP.TextAlign = ContentAlignment.MiddleLeft;
      this.LB_CAM_DIR.AutoSize = true;
      this.Tb_TRAC_INFO_CNSL.SetColumnSpan((Control)this.LB_CAM_DIR, 2);
      this.LB_CAM_DIR.Dock = DockStyle.Fill;
      this.LB_CAM_DIR.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
      this.LB_CAM_DIR.ForeColor = SystemColors.ControlText;
      this.LB_CAM_DIR.Location = new Point(4, 46);
      this.LB_CAM_DIR.Name = "LB_CAM_DIR";
      this.LB_CAM_DIR.Size = new Size(176, 14);
      this.LB_CAM_DIR.TabIndex = 15;
      this.LB_CAM_DIR.Text = "Direction";
      this.LB_CAM_DIR.TextAlign = ContentAlignment.MiddleLeft;
      this.LB_CAM_FOV.AutoSize = true;
      this.Tb_TRAC_INFO_CNSL.SetColumnSpan((Control)this.LB_CAM_FOV, 2);
      this.LB_CAM_FOV.Dock = DockStyle.Fill;
      this.LB_CAM_FOV.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
      this.LB_CAM_FOV.ForeColor = SystemColors.ControlText;
      this.LB_CAM_FOV.Location = new Point(4, 136);
      this.LB_CAM_FOV.Name = "LB_CAM_FOV";
      this.LB_CAM_FOV.Size = new Size(176, 14);
      this.LB_CAM_FOV.TabIndex = 17;
      this.LB_CAM_FOV.Text = "FOV";
      this.LB_CAM_FOV.TextAlign = ContentAlignment.MiddleLeft;
      this.GROP_VR_OPTN.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.GROP_VR_OPTN.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.GROP_VR_OPTN.Controls.Add((Control)this.TB_VR_OPTN);
      this.GROP_VR_OPTN.Location = new Point(5, 280);
      this.GROP_VR_OPTN.Name = "GROP_VR_OPTN";
      this.GROP_VR_OPTN.Size = new Size(190, 329);
      this.GROP_VR_OPTN.TabIndex = 37;
      this.GROP_VR_OPTN.TabStop = false;
      this.GROP_VR_OPTN.Text = "VR Option";
      this.TB_VR_OPTN.AllowDrop = true;
      this.TB_VR_OPTN.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.TB_VR_OPTN.BackColor = SystemColors.ControlLightLight;
      this.TB_VR_OPTN.BackgroundImageLayout = ImageLayout.None;
      this.TB_VR_OPTN.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
      this.TB_VR_OPTN.ColumnCount = 2;
      this.TB_VR_OPTN.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
      this.TB_VR_OPTN.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
      this.TB_VR_OPTN.Controls.Add((Control)this.LB_LONGITUDE, 0, 13);
      this.TB_VR_OPTN.Controls.Add((Control)this.LB_USER_OPTN, 0, 0);
      this.TB_VR_OPTN.Controls.Add((Control)this.LB_VOICE, 0, 10);
      this.TB_VR_OPTN.Controls.Add((Control)this.CB_VOICE, 1, 10);
      this.TB_VR_OPTN.Controls.Add((Control)this.LB_MYO, 0, 9);
      this.TB_VR_OPTN.Controls.Add((Control)this.EYE_HEIGHT, 1, 3);
      this.TB_VR_OPTN.Controls.Add((Control)this.CB_MYO, 1, 9);
      this.TB_VR_OPTN.Controls.Add((Control)this.LB_XBOX, 0, 8);
      this.TB_VR_OPTN.Controls.Add((Control)this.CB_XBOX, 1, 8);
      this.TB_VR_OPTN.Controls.Add((Control)this.LB_INPT_OPTN, 0, 7);
      this.TB_VR_OPTN.Controls.Add((Control)this.CB_MIRROR, 1, 6);
      this.TB_VR_OPTN.Controls.Add((Control)this.LB_MIRROR, 0, 6);
      this.TB_VR_OPTN.Controls.Add((Control)this.LB_DISPLAY_MODE, 0, 5);
      this.TB_VR_OPTN.Controls.Add((Control)this.DISPLAY_MODE, 1, 5);
      this.TB_VR_OPTN.Controls.Add((Control)this.LB_EYE_HEIGHT, 0, 3);
      this.TB_VR_OPTN.Controls.Add((Control)this.LB_FLOORS, 0, 2);
      this.TB_VR_OPTN.Controls.Add((Control)this.START_FLOORS, 1, 2);
      this.TB_VR_OPTN.Controls.Add((Control)this.START_POSITION, 1, 1);
      this.TB_VR_OPTN.Controls.Add((Control)this.LB_START_POSITION, 0, 1);
      this.TB_VR_OPTN.Controls.Add((Control)this.LB_DSPL_OPTN, 0, 4);
      this.TB_VR_OPTN.Controls.Add((Control)this.LB_GEO_LOCATION, 0, 11);
      this.TB_VR_OPTN.Controls.Add((Control)this.LB_LATITUDE, 0, 12);
      this.TB_VR_OPTN.Controls.Add((Control)this.LATITUDE, 1, 12);
      this.TB_VR_OPTN.Controls.Add((Control)this.LONGITUDE, 1, 13);
      this.TB_VR_OPTN.Dock = DockStyle.Fill;
      this.TB_VR_OPTN.Location = new Point(3, 16);
      this.TB_VR_OPTN.Name = "TB_VR_OPTN";
      this.TB_VR_OPTN.RightToLeft = RightToLeft.No;
      this.TB_VR_OPTN.RowCount = 14;
      this.TB_VR_OPTN.RowStyles.Add(new RowStyle(SizeType.Percent, 7.142857f));
      this.TB_VR_OPTN.RowStyles.Add(new RowStyle(SizeType.Percent, 7.142857f));
      this.TB_VR_OPTN.RowStyles.Add(new RowStyle(SizeType.Percent, 7.142857f));
      this.TB_VR_OPTN.RowStyles.Add(new RowStyle(SizeType.Percent, 7.142857f));
      this.TB_VR_OPTN.RowStyles.Add(new RowStyle(SizeType.Percent, 7.142857f));
      this.TB_VR_OPTN.RowStyles.Add(new RowStyle(SizeType.Percent, 7.142857f));
      this.TB_VR_OPTN.RowStyles.Add(new RowStyle(SizeType.Percent, 7.142857f));
      this.TB_VR_OPTN.RowStyles.Add(new RowStyle(SizeType.Percent, 7.142857f));
      this.TB_VR_OPTN.RowStyles.Add(new RowStyle(SizeType.Percent, 7.142857f));
      this.TB_VR_OPTN.RowStyles.Add(new RowStyle(SizeType.Percent, 7.142857f));
      this.TB_VR_OPTN.RowStyles.Add(new RowStyle(SizeType.Percent, 7.142857f));
      this.TB_VR_OPTN.RowStyles.Add(new RowStyle(SizeType.Percent, 7.142857f));
      this.TB_VR_OPTN.RowStyles.Add(new RowStyle(SizeType.Percent, 7.142857f));
      this.TB_VR_OPTN.RowStyles.Add(new RowStyle(SizeType.Percent, 7.142857f));
      this.TB_VR_OPTN.Size = new Size(184, 310);
      this.TB_VR_OPTN.TabIndex = 32;
      this.LB_LONGITUDE.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.LB_LONGITUDE.AutoSize = true;
      this.LB_LONGITUDE.Font = new Font("Arial", 8.5f);
      this.LB_LONGITUDE.ForeColor = SystemColors.ControlText;
      this.LB_LONGITUDE.Location = new Point(4, 287);
      this.LB_LONGITUDE.Name = "LB_LONGITUDE";
      this.LB_LONGITUDE.Size = new Size(84, 22);
      this.LB_LONGITUDE.TabIndex = 38;
      this.LB_LONGITUDE.Text = "Longitude";
      this.LB_LONGITUDE.TextAlign = ContentAlignment.MiddleLeft;
      this.LB_USER_OPTN.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.LB_USER_OPTN.AutoSize = true;
      this.TB_VR_OPTN.SetColumnSpan((Control)this.LB_USER_OPTN, 2);
      this.LB_USER_OPTN.Font = new Font("Arial", 8.5f);
      this.LB_USER_OPTN.ForeColor = SystemColors.ControlText;
      this.LB_USER_OPTN.Location = new Point(4, 1);
      this.LB_USER_OPTN.Name = "LB_USER_OPTN";
      this.LB_USER_OPTN.Size = new Size(176, 21);
      this.LB_USER_OPTN.TabIndex = 34;
      this.LB_USER_OPTN.Text = "User Option";
      this.LB_USER_OPTN.TextAlign = ContentAlignment.MiddleCenter;
      this.LB_VOICE.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.LB_VOICE.AutoSize = true;
      this.LB_VOICE.Font = new Font("Arial", 8.5f);
      this.LB_VOICE.ForeColor = SystemColors.ControlText;
      this.LB_VOICE.Location = new Point(4, 221);
      this.LB_VOICE.Name = "LB_VOICE";
      this.LB_VOICE.Size = new Size(84, 21);
      this.LB_VOICE.TabIndex = 25;
      this.LB_VOICE.Text = "Voice";
      this.LB_VOICE.TextAlign = ContentAlignment.MiddleLeft;
      this.CB_VOICE.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.CB_VOICE.AutoSize = true;
      this.CB_VOICE.Checked = true;
      this.CB_VOICE.CheckState = CheckState.Checked;
      this.CB_VOICE.Location = new Point(95, 224);
      this.CB_VOICE.Name = "CB_VOICE";
      this.CB_VOICE.Size = new Size(85, 15);
      this.CB_VOICE.TabIndex = 28;
      this.CB_VOICE.UseVisualStyleBackColor = true;
      this.CB_VOICE.CheckedChanged += new EventHandler(this.CB_VOICE_CheckedChanged);
      this.LB_MYO.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.LB_MYO.AutoSize = true;
      this.LB_MYO.Font = new Font("Arial", 8.5f);
      this.LB_MYO.ForeColor = SystemColors.ControlText;
      this.LB_MYO.Location = new Point(4, 199);
      this.LB_MYO.Name = "LB_MYO";
      this.LB_MYO.Size = new Size(84, 21);
      this.LB_MYO.TabIndex = 23;
      this.LB_MYO.Text = "Myo";
      this.LB_MYO.TextAlign = ContentAlignment.MiddleLeft;
      this.EYE_HEIGHT.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      this.EYE_HEIGHT.BackColor = SystemColors.Window;
      this.EYE_HEIGHT.BorderStyle = BorderStyle.None;
      this.EYE_HEIGHT.Font = new Font("Arial", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
      this.EYE_HEIGHT.Location = new Point(93, 70);
      this.EYE_HEIGHT.Margin = new Padding(1);
      this.EYE_HEIGHT.MinimumSize = new Size(0, 17);
      this.EYE_HEIGHT.Name = "EYE_HEIGHT";
      this.EYE_HEIGHT.Size = new Size(89, 17);
      this.EYE_HEIGHT.TabIndex = 22;
      this.EYE_HEIGHT.Text = "1.6";
      this.EYE_HEIGHT.TextChanged += new EventHandler(this.EYE_HEIGHT_TextChanged);
      this.CB_MYO.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.CB_MYO.AutoSize = true;
      this.CB_MYO.Location = new Point(95, 202);
      this.CB_MYO.Name = "CB_MYO";
      this.CB_MYO.Size = new Size(85, 15);
      this.CB_MYO.TabIndex = 26;
      this.CB_MYO.UseVisualStyleBackColor = true;
      this.CB_MYO.CheckedChanged += new EventHandler(this.CB_MYO_CheckedChanged);
      this.LB_XBOX.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.LB_XBOX.AutoSize = true;
      this.LB_XBOX.Font = new Font("Arial", 8.5f);
      this.LB_XBOX.ForeColor = SystemColors.ControlText;
      this.LB_XBOX.Location = new Point(4, 177);
      this.LB_XBOX.Name = "LB_XBOX";
      this.LB_XBOX.Size = new Size(84, 21);
      this.LB_XBOX.TabIndex = 24;
      this.LB_XBOX.Text = "XBox";
      this.LB_XBOX.TextAlign = ContentAlignment.MiddleLeft;
      this.CB_XBOX.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.CB_XBOX.AutoSize = true;
      this.CB_XBOX.Location = new Point(95, 180);
      this.CB_XBOX.Name = "CB_XBOX";
      this.CB_XBOX.Size = new Size(85, 15);
      this.CB_XBOX.TabIndex = 27;
      this.CB_XBOX.UseVisualStyleBackColor = true;
      this.LB_INPT_OPTN.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.LB_INPT_OPTN.AutoSize = true;
      this.TB_VR_OPTN.SetColumnSpan((Control)this.LB_INPT_OPTN, 2);
      this.LB_INPT_OPTN.Font = new Font("Arial", 8.5f);
      this.LB_INPT_OPTN.ForeColor = SystemColors.ControlText;
      this.LB_INPT_OPTN.Location = new Point(4, 155);
      this.LB_INPT_OPTN.Name = "LB_INPT_OPTN";
      this.LB_INPT_OPTN.Size = new Size(176, 21);
      this.LB_INPT_OPTN.TabIndex = 33;
      this.LB_INPT_OPTN.Text = "Input Option";
      this.LB_INPT_OPTN.TextAlign = ContentAlignment.MiddleCenter;
      this.CB_MIRROR.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.CB_MIRROR.AutoSize = true;
      this.CB_MIRROR.Checked = true;
      this.CB_MIRROR.CheckState = CheckState.Checked;
      this.CB_MIRROR.Location = new Point(95, 136);
      this.CB_MIRROR.Name = "CB_MIRROR";
      this.CB_MIRROR.Size = new Size(85, 15);
      this.CB_MIRROR.TabIndex = 32;
      this.CB_MIRROR.UseVisualStyleBackColor = true;
      this.CB_MIRROR.CheckedChanged += new EventHandler(this.CB_MIRROR_CheckedChanged);
      this.LB_MIRROR.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.LB_MIRROR.AutoSize = true;
      this.LB_MIRROR.Font = new Font("Arial", 8.5f);
      this.LB_MIRROR.ForeColor = SystemColors.ControlText;
      this.LB_MIRROR.Location = new Point(4, 133);
      this.LB_MIRROR.Name = "LB_MIRROR";
      this.LB_MIRROR.Size = new Size(84, 21);
      this.LB_MIRROR.TabIndex = 31;
      this.LB_MIRROR.Text = "Mirror";
      this.LB_MIRROR.TextAlign = ContentAlignment.MiddleLeft;
      this.LB_DISPLAY_MODE.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.LB_DISPLAY_MODE.AutoSize = true;
      this.LB_DISPLAY_MODE.Font = new Font("Arial", 8.5f);
      this.LB_DISPLAY_MODE.ForeColor = SystemColors.ControlText;
      this.LB_DISPLAY_MODE.Location = new Point(4, 111);
      this.LB_DISPLAY_MODE.Name = "LB_DISPLAY_MODE";
      this.LB_DISPLAY_MODE.Size = new Size(84, 21);
      this.LB_DISPLAY_MODE.TabIndex = 29;
      this.LB_DISPLAY_MODE.Text = "Display Mode";
      this.LB_DISPLAY_MODE.TextAlign = ContentAlignment.MiddleLeft;
      this.DISPLAY_MODE.Dock = DockStyle.Fill;
      this.DISPLAY_MODE.DropDownStyle = ComboBoxStyle.DropDownList;
      this.DISPLAY_MODE.FlatStyle = FlatStyle.Flat;
      this.DISPLAY_MODE.FormattingEnabled = true;
      this.DISPLAY_MODE.Location = new Point(92, 111);
      this.DISPLAY_MODE.Margin = new Padding(0);
      this.DISPLAY_MODE.Name = "DISPLAY_MODE";
      this.DISPLAY_MODE.Size = new Size(91, 21);
      this.DISPLAY_MODE.TabIndex = 30;
      this.DISPLAY_MODE.SelectedIndexChanged += new EventHandler(this.DISPLAY_MODE_SelectedIndexChanged);
      this.LB_EYE_HEIGHT.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.LB_EYE_HEIGHT.AutoSize = true;
      this.LB_EYE_HEIGHT.Font = new Font("Arial", 8.5f);
      this.LB_EYE_HEIGHT.ForeColor = SystemColors.ControlText;
      this.LB_EYE_HEIGHT.Location = new Point(4, 67);
      this.LB_EYE_HEIGHT.Name = "LB_EYE_HEIGHT";
      this.LB_EYE_HEIGHT.Size = new Size(84, 21);
      this.LB_EYE_HEIGHT.TabIndex = 21;
      this.LB_EYE_HEIGHT.Text = "Eye Height";
      this.LB_EYE_HEIGHT.TextAlign = ContentAlignment.MiddleLeft;
      this.LB_FLOORS.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.LB_FLOORS.AutoSize = true;
      this.LB_FLOORS.Font = new Font("Arial", 8.5f);
      this.LB_FLOORS.ForeColor = SystemColors.ControlText;
      this.LB_FLOORS.Location = new Point(4, 45);
      this.LB_FLOORS.Name = "LB_FLOORS";
      this.LB_FLOORS.Size = new Size(84, 21);
      this.LB_FLOORS.TabIndex = 18;
      this.LB_FLOORS.Text = "Floors";
      this.LB_FLOORS.TextAlign = ContentAlignment.MiddleLeft;
      this.START_FLOORS.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.START_FLOORS.AutoSize = true;
      this.START_FLOORS.Location = new Point(92, 45);
      this.START_FLOORS.Margin = new Padding(0);
      this.START_FLOORS.Name = "START_FLOORS";
      this.START_FLOORS.Size = new Size(91, 21);
      this.START_FLOORS.TabIndex = 20;
      this.START_FLOORS.Text = "Select";
      this.START_FLOORS.UseVisualStyleBackColor = true;
      this.START_FLOORS.Click += new EventHandler(this.START_FLOORS_Click);
      this.START_POSITION.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.START_POSITION.AutoSize = true;
      this.START_POSITION.Location = new Point(92, 23);
      this.START_POSITION.Margin = new Padding(0);
      this.START_POSITION.Name = "START_POSITION";
      this.START_POSITION.Size = new Size(91, 21);
      this.START_POSITION.TabIndex = 19;
      this.START_POSITION.Text = "Place...";
      this.START_POSITION.UseVisualStyleBackColor = true;
      this.START_POSITION.Click += new EventHandler(this.START_POSITION_Click);
      this.LB_START_POSITION.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.LB_START_POSITION.AutoSize = true;
      this.LB_START_POSITION.Font = new Font("Arial", 8.5f);
      this.LB_START_POSITION.ForeColor = SystemColors.ControlText;
      this.LB_START_POSITION.Location = new Point(4, 23);
      this.LB_START_POSITION.Name = "LB_START_POSITION";
      this.LB_START_POSITION.Size = new Size(84, 21);
      this.LB_START_POSITION.TabIndex = 14;
      this.LB_START_POSITION.Text = "Start Position";
      this.LB_START_POSITION.TextAlign = ContentAlignment.MiddleLeft;
      this.LB_DSPL_OPTN.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.LB_DSPL_OPTN.AutoSize = true;
      this.TB_VR_OPTN.SetColumnSpan((Control)this.LB_DSPL_OPTN, 2);
      this.LB_DSPL_OPTN.Font = new Font("Arial", 8.5f);
      this.LB_DSPL_OPTN.ForeColor = SystemColors.ControlText;
      this.LB_DSPL_OPTN.Location = new Point(4, 89);
      this.LB_DSPL_OPTN.Name = "LB_DSPL_OPTN";
      this.LB_DSPL_OPTN.Size = new Size(176, 21);
      this.LB_DSPL_OPTN.TabIndex = 35;
      this.LB_DSPL_OPTN.Text = "Display Option";
      this.LB_DSPL_OPTN.TextAlign = ContentAlignment.MiddleCenter;
      this.LB_GEO_LOCATION.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.LB_GEO_LOCATION.AutoSize = true;
      this.TB_VR_OPTN.SetColumnSpan((Control)this.LB_GEO_LOCATION, 2);
      this.LB_GEO_LOCATION.Font = new Font("Arial", 8.5f);
      this.LB_GEO_LOCATION.ForeColor = SystemColors.ControlText;
      this.LB_GEO_LOCATION.Location = new Point(4, 243);
      this.LB_GEO_LOCATION.Name = "LB_GEO_LOCATION";
      this.LB_GEO_LOCATION.Size = new Size(176, 21);
      this.LB_GEO_LOCATION.TabIndex = 36;
      this.LB_GEO_LOCATION.Text = "Geo Location";
      this.LB_GEO_LOCATION.TextAlign = ContentAlignment.MiddleCenter;
      this.LB_LATITUDE.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.LB_LATITUDE.AutoSize = true;
      this.LB_LATITUDE.Font = new Font("Arial", 8.5f);
      this.LB_LATITUDE.ForeColor = SystemColors.ControlText;
      this.LB_LATITUDE.Location = new Point(4, 265);
      this.LB_LATITUDE.Name = "LB_LATITUDE";
      this.LB_LATITUDE.Size = new Size(84, 21);
      this.LB_LATITUDE.TabIndex = 37;
      this.LB_LATITUDE.Text = "Latitude";
      this.LB_LATITUDE.TextAlign = ContentAlignment.MiddleLeft;
      this.LATITUDE.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      this.LATITUDE.BackColor = SystemColors.Window;
      this.LATITUDE.BorderStyle = BorderStyle.None;
      this.LATITUDE.Font = new Font("Arial", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
      this.LATITUDE.Location = new Point(93, 268);
      this.LATITUDE.Margin = new Padding(1);
      this.LATITUDE.MinimumSize = new Size(0, 17);
      this.LATITUDE.Name = "LATITUDE";
      this.LATITUDE.Size = new Size(89, 17);
      this.LATITUDE.TabIndex = 39;
      this.LATITUDE.Text = "28.546474";
      this.LATITUDE.TextChanged += new EventHandler(this.LATITUDE_TextChanged);
      this.LONGITUDE.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      this.LONGITUDE.BackColor = SystemColors.Window;
      this.LONGITUDE.BorderStyle = BorderStyle.None;
      this.LONGITUDE.Font = new Font("Arial", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
      this.LONGITUDE.Location = new Point(93, 291);
      this.LONGITUDE.Margin = new Padding(1);
      this.LONGITUDE.MinimumSize = new Size(0, 17);
      this.LONGITUDE.Name = "LONGITUDE";
      this.LONGITUDE.Size = new Size(89, 17);
      this.LONGITUDE.TabIndex = 40;
      this.LONGITUDE.Text = "-81.385560";
      this.LONGITUDE.TextChanged += new EventHandler(this.LONGITUDE_TextChanged);
      this.LB_LING_DOT_WORLD.AutoSize = true;
      this.LB_LING_DOT_WORLD.Location = new Point(5, 612);
      this.LB_LING_DOT_WORLD.Name = "RhinoVR";
      this.LB_LING_DOT_WORLD.Size = new Size(78, 13);
      this.LB_LING_DOT_WORLD.TabIndex = 38;
      this.LB_LING_DOT_WORLD.TabStop = true;
      this.LB_LING_DOT_WORLD.Text = "RhinoVR";
      this.LB_LING_DOT_WORLD.TextAlign = ContentAlignment.MiddleRight;
      this.label1.AutoSize = true;
      this.label1.ForeColor = SystemColors.ControlDarkDark;
      this.label1.Location = new Point(132, 612);
      this.label1.Name = "label1";
      this.label1.Size = new Size(63, 13);
      this.label1.TabIndex = 39;
      this.label1.Text = "RhinoVR";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.Controls.Add((Control)this.label1);
      this.Controls.Add((Control)this.LB_LING_DOT_WORLD);
      this.Controls.Add((Control)this.GROP_VR_OPTN);
      this.Controls.Add((Control)this.GROP_TRAC_INFO_CNSL);
      this.Controls.Add((Control)this.STOP_VR);
      this.Controls.Add((Control)this.START_VR);
      this.Name = "VR_PANEL";
      this.Size = new Size(200, 640);
      this.GROP_TRAC_INFO_CNSL.ResumeLayout(false);
      this.Tb_TRAC_INFO_CNSL.ResumeLayout(false);
      this.Tb_TRAC_INFO_CNSL.PerformLayout();
      this.GROP_VR_OPTN.ResumeLayout(false);
      this.TB_VR_OPTN.ResumeLayout(false);
      this.TB_VR_OPTN.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
