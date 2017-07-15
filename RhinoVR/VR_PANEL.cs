using Rhino;
using Rhino.Display;
//using Rhino.Geometry;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RhinoVR {
  [Guid("D1508142-E289-4CF4-AB6F-AB5A1F1E4D9C")]
  public class VR_PANEL : UserControl {

    private static double _eyeHeight = 1.6;
    private readonly RiftViewportConduit _riftViewportConduit = new RiftViewportConduit();
    //private MyoController _myoController = new MyoController();
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

    public static double EyeHeight { get { return VR_PANEL._eyeHeight; } }
    public static DisplayModeDescription[] DisplayModes { get { return VR_PANEL._displayModes; } }
    public static double Latitude { get { return VR_PANEL._latitude; } }
    public static double Longitude { get { return VR_PANEL._longitude; } }

    public VR_PANEL() {
      this.InitializeComponent();
      VR_PANEL._displayModes = DisplayModeDescription.GetDisplayModes();
      foreach (DisplayModeDescription displayMode in VR_PANEL._displayModes)
        this.DISPLAY_MODE.Items.Add((object)displayMode.EnglishName);

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

      this.CAM_LOC_L.Text = string.Format("{0:F3}, {1:F3}, {2:F3}", camLoc[0].X, camLoc[0].Y, camLoc[0].Z);
      this.CAM_LOC_R.Text = string.Format("{0:F3}, {1:F3}, {2:F3}", camLoc[1].X, camLoc[1].Y, camLoc[1].Z);
      this.CAM_DIR_L.Text = string.Format("{0:F3}, {1:F3}, {2:F3}", camDir[0].X, camDir[0].Y, camDir[0].Z);
      this.CAM_DIR_R.Text = string.Format("{0:F3}, {1:F3}, {2:F3}", camDir[1].X, camDir[1].Y, camDir[1].Z);
      this.CAM_UP_L.Text = string.Format("{0:F3}, {1:F3}, {2:F3}", camUp[0].X, camUp[0].Y, camUp[0].Z);
      this.CAM_UP_R.Text = string.Format("{0:F3}, {1:F3}, {2:F3}", camUp[1].X, camUp[1].Y, camUp[1].Z);
      this.FIELD_OF_VIEW_L.Text = string.Format("{0:F3}, {1:F3}", fovL[0], fovR[0]);
      this.FIELD_OF_VIEW_R.Text = string.Format("{0:F3}, {1:F3}", fovL[1], fovR[1]);
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
      this.START_VR = new System.Windows.Forms.Button();
      this.STOP_VR = new System.Windows.Forms.Button();
      this.LBL_CAM_LOC_R = new System.Windows.Forms.Label();
      this.LBL_CAM_LOC_L = new System.Windows.Forms.Label();
      this.CAM_LOC_R = new System.Windows.Forms.Label();
      this.CAM_LOC_L = new System.Windows.Forms.Label();
      this.CAM_DIR_R = new System.Windows.Forms.Label();
      this.CAM_DIR_L = new System.Windows.Forms.Label();
      this.CAM_UP_R = new System.Windows.Forms.Label();
      this.CAM_UP_L = new System.Windows.Forms.Label();
      this.FIELD_OF_VIEW_R = new System.Windows.Forms.Label();
      this.FIELD_OF_VIEW_L = new System.Windows.Forms.Label();
      this.LBL_CAM_DIR_L = new System.Windows.Forms.Label();
      this.LBL_CAM_DIR_R = new System.Windows.Forms.Label();
      this.LBL_CAM_UP_L = new System.Windows.Forms.Label();
      this.LBL_CAM_UP_R = new System.Windows.Forms.Label();
      this.LBL_FIELD_OF_VIEW_L = new System.Windows.Forms.Label();
      this.LBL_FIELD_OF_VIEW_R = new System.Windows.Forms.Label();
      this.GROP_TRAC_INFO_CNSL = new System.Windows.Forms.GroupBox();
      this.Tb_TRAC_INFO_CNSL = new System.Windows.Forms.TableLayoutPanel();
      this.LB_CAM_LOC = new System.Windows.Forms.Label();
      this.LB_CAM_UP = new System.Windows.Forms.Label();
      this.LB_CAM_DIR = new System.Windows.Forms.Label();
      this.LB_CAM_FOV = new System.Windows.Forms.Label();
      this.GROP_VR_OPTN = new System.Windows.Forms.GroupBox();
      this.TB_VR_OPTN = new System.Windows.Forms.TableLayoutPanel();
      this.LB_USER_OPTN = new System.Windows.Forms.Label();
      this.EYE_HEIGHT = new System.Windows.Forms.TextBox();
      this.LB_INPT_OPTN = new System.Windows.Forms.Label();
      this.CB_MIRROR = new System.Windows.Forms.CheckBox();
      this.LB_MIRROR = new System.Windows.Forms.Label();
      this.LB_DISPLAY_MODE = new System.Windows.Forms.Label();
      this.DISPLAY_MODE = new System.Windows.Forms.ComboBox();
      this.LB_EYE_HEIGHT = new System.Windows.Forms.Label();
      this.LB_FLOORS = new System.Windows.Forms.Label();
      this.START_FLOORS = new System.Windows.Forms.Button();
      this.START_POSITION = new System.Windows.Forms.Button();
      this.LB_START_POSITION = new System.Windows.Forms.Label();
      this.LB_DSPL_OPTN = new System.Windows.Forms.Label();
      this.LB_VOICE = new System.Windows.Forms.Label();
      this.CB_VOICE = new System.Windows.Forms.CheckBox();
      this.LB_GEO_LOCATION = new System.Windows.Forms.Label();
      this.LB_LATITUDE = new System.Windows.Forms.Label();
      this.LATITUDE = new System.Windows.Forms.TextBox();
      this.LB_LONGITUDE = new System.Windows.Forms.Label();
      this.LONGITUDE = new System.Windows.Forms.TextBox();
      this.LB_LING_DOT_WORLD = new System.Windows.Forms.LinkLabel();
      this.label1 = new System.Windows.Forms.Label();
      this.GROP_TRAC_INFO_CNSL.SuspendLayout();
      this.Tb_TRAC_INFO_CNSL.SuspendLayout();
      this.GROP_VR_OPTN.SuspendLayout();
      this.TB_VR_OPTN.SuspendLayout();
      this.SuspendLayout();
      // 
      // START_VR
      // 
      this.START_VR.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.START_VR.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.START_VR.Location = new System.Drawing.Point(5, 5);
      this.START_VR.Name = "START_VR";
      this.START_VR.Size = new System.Drawing.Size(190, 30);
      this.START_VR.TabIndex = 0;
      this.START_VR.Text = "Start VR";
      this.START_VR.UseVisualStyleBackColor = true;
      this.START_VR.Click += new System.EventHandler(this.START_VR_Click);
      // 
      // STOP_VR
      // 
      this.STOP_VR.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.STOP_VR.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.STOP_VR.Location = new System.Drawing.Point(5, 40);
      this.STOP_VR.Name = "STOP_VR";
      this.STOP_VR.Size = new System.Drawing.Size(190, 30);
      this.STOP_VR.TabIndex = 1;
      this.STOP_VR.Text = "Stop VR";
      this.STOP_VR.UseVisualStyleBackColor = true;
      this.STOP_VR.Click += new System.EventHandler(this.STOP_VR_Click);
      // 
      // LBL_CAM_LOC_R
      // 
      this.LBL_CAM_LOC_R.AutoSize = true;
      this.LBL_CAM_LOC_R.Dock = System.Windows.Forms.DockStyle.Fill;
      this.LBL_CAM_LOC_R.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.LBL_CAM_LOC_R.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
      this.LBL_CAM_LOC_R.Location = new System.Drawing.Point(4, 31);
      this.LBL_CAM_LOC_R.Name = "LBL_CAM_LOC_R";
      this.LBL_CAM_LOC_R.Size = new System.Drawing.Size(57, 14);
      this.LBL_CAM_LOC_R.TabIndex = 13;
      this.LBL_CAM_LOC_R.Text = "Right Eye";
      this.LBL_CAM_LOC_R.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // LBL_CAM_LOC_L
      // 
      this.LBL_CAM_LOC_L.AutoSize = true;
      this.LBL_CAM_LOC_L.Dock = System.Windows.Forms.DockStyle.Fill;
      this.LBL_CAM_LOC_L.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.LBL_CAM_LOC_L.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
      this.LBL_CAM_LOC_L.Location = new System.Drawing.Point(4, 16);
      this.LBL_CAM_LOC_L.Name = "LBL_CAM_LOC_L";
      this.LBL_CAM_LOC_L.Size = new System.Drawing.Size(57, 14);
      this.LBL_CAM_LOC_L.TabIndex = 12;
      this.LBL_CAM_LOC_L.Text = "Left Eye";
      this.LBL_CAM_LOC_L.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // CAM_LOC_R
      // 
      this.CAM_LOC_R.AutoSize = true;
      this.CAM_LOC_R.Dock = System.Windows.Forms.DockStyle.Fill;
      this.CAM_LOC_R.Location = new System.Drawing.Point(68, 31);
      this.CAM_LOC_R.Name = "CAM_LOC_R";
      this.CAM_LOC_R.Size = new System.Drawing.Size(112, 14);
      this.CAM_LOC_R.TabIndex = 11;
      this.CAM_LOC_R.Text = "0.000, 0.000, 0.000";
      this.CAM_LOC_R.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // CAM_LOC_L
      // 
      this.CAM_LOC_L.AutoSize = true;
      this.CAM_LOC_L.Dock = System.Windows.Forms.DockStyle.Fill;
      this.CAM_LOC_L.Location = new System.Drawing.Point(68, 16);
      this.CAM_LOC_L.Name = "CAM_LOC_L";
      this.CAM_LOC_L.Size = new System.Drawing.Size(112, 14);
      this.CAM_LOC_L.TabIndex = 10;
      this.CAM_LOC_L.Text = "0.000, 0.000, 0.000";
      this.CAM_LOC_L.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // CAM_DIR_R
      // 
      this.CAM_DIR_R.AutoSize = true;
      this.CAM_DIR_R.Dock = System.Windows.Forms.DockStyle.Fill;
      this.CAM_DIR_R.Location = new System.Drawing.Point(68, 76);
      this.CAM_DIR_R.Name = "CAM_DIR_R";
      this.CAM_DIR_R.Size = new System.Drawing.Size(112, 14);
      this.CAM_DIR_R.TabIndex = 11;
      this.CAM_DIR_R.Text = "0.000, 0.000, 0.000";
      this.CAM_DIR_R.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // CAM_DIR_L
      // 
      this.CAM_DIR_L.AutoSize = true;
      this.CAM_DIR_L.Dock = System.Windows.Forms.DockStyle.Fill;
      this.CAM_DIR_L.Location = new System.Drawing.Point(68, 61);
      this.CAM_DIR_L.Name = "CAM_DIR_L";
      this.CAM_DIR_L.Size = new System.Drawing.Size(112, 14);
      this.CAM_DIR_L.TabIndex = 10;
      this.CAM_DIR_L.Text = "0.000, 0.000, 0.000";
      this.CAM_DIR_L.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // CAM_UP_R
      // 
      this.CAM_UP_R.AutoSize = true;
      this.CAM_UP_R.Dock = System.Windows.Forms.DockStyle.Fill;
      this.CAM_UP_R.Location = new System.Drawing.Point(68, 121);
      this.CAM_UP_R.Name = "CAM_UP_R";
      this.CAM_UP_R.Size = new System.Drawing.Size(112, 14);
      this.CAM_UP_R.TabIndex = 11;
      this.CAM_UP_R.Text = "0.000, 0.000, 0.000";
      this.CAM_UP_R.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // CAM_UP_L
      // 
      this.CAM_UP_L.AutoSize = true;
      this.CAM_UP_L.Dock = System.Windows.Forms.DockStyle.Fill;
      this.CAM_UP_L.Location = new System.Drawing.Point(68, 106);
      this.CAM_UP_L.Name = "CAM_UP_L";
      this.CAM_UP_L.Size = new System.Drawing.Size(112, 14);
      this.CAM_UP_L.TabIndex = 10;
      this.CAM_UP_L.Text = "0.000, 0.000, 0.000";
      this.CAM_UP_L.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // FIELD_OF_VIEW_R
      // 
      this.FIELD_OF_VIEW_R.AutoSize = true;
      this.FIELD_OF_VIEW_R.Dock = System.Windows.Forms.DockStyle.Fill;
      this.FIELD_OF_VIEW_R.Location = new System.Drawing.Point(68, 166);
      this.FIELD_OF_VIEW_R.Name = "FIELD_OF_VIEW_R";
      this.FIELD_OF_VIEW_R.Size = new System.Drawing.Size(112, 14);
      this.FIELD_OF_VIEW_R.TabIndex = 11;
      this.FIELD_OF_VIEW_R.Text = "0.000, 0.000";
      this.FIELD_OF_VIEW_R.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // FIELD_OF_VIEW_L
      // 
      this.FIELD_OF_VIEW_L.AutoSize = true;
      this.FIELD_OF_VIEW_L.Dock = System.Windows.Forms.DockStyle.Fill;
      this.FIELD_OF_VIEW_L.Location = new System.Drawing.Point(68, 151);
      this.FIELD_OF_VIEW_L.Name = "FIELD_OF_VIEW_L";
      this.FIELD_OF_VIEW_L.Size = new System.Drawing.Size(112, 14);
      this.FIELD_OF_VIEW_L.TabIndex = 10;
      this.FIELD_OF_VIEW_L.Text = "0.000, 0.000";
      this.FIELD_OF_VIEW_L.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // LBL_CAM_DIR_L
      // 
      this.LBL_CAM_DIR_L.AutoSize = true;
      this.LBL_CAM_DIR_L.Dock = System.Windows.Forms.DockStyle.Fill;
      this.LBL_CAM_DIR_L.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.LBL_CAM_DIR_L.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
      this.LBL_CAM_DIR_L.Location = new System.Drawing.Point(4, 61);
      this.LBL_CAM_DIR_L.Name = "LBL_CAM_DIR_L";
      this.LBL_CAM_DIR_L.Size = new System.Drawing.Size(57, 14);
      this.LBL_CAM_DIR_L.TabIndex = 12;
      this.LBL_CAM_DIR_L.Text = "Left Eye";
      this.LBL_CAM_DIR_L.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // LBL_CAM_DIR_R
      // 
      this.LBL_CAM_DIR_R.AutoSize = true;
      this.LBL_CAM_DIR_R.Dock = System.Windows.Forms.DockStyle.Fill;
      this.LBL_CAM_DIR_R.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.LBL_CAM_DIR_R.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
      this.LBL_CAM_DIR_R.Location = new System.Drawing.Point(4, 76);
      this.LBL_CAM_DIR_R.Name = "LBL_CAM_DIR_R";
      this.LBL_CAM_DIR_R.Size = new System.Drawing.Size(57, 14);
      this.LBL_CAM_DIR_R.TabIndex = 13;
      this.LBL_CAM_DIR_R.Text = "Right Eye";
      this.LBL_CAM_DIR_R.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // LBL_CAM_UP_L
      // 
      this.LBL_CAM_UP_L.AutoSize = true;
      this.LBL_CAM_UP_L.Dock = System.Windows.Forms.DockStyle.Fill;
      this.LBL_CAM_UP_L.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.LBL_CAM_UP_L.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
      this.LBL_CAM_UP_L.Location = new System.Drawing.Point(4, 106);
      this.LBL_CAM_UP_L.Name = "LBL_CAM_UP_L";
      this.LBL_CAM_UP_L.Size = new System.Drawing.Size(57, 14);
      this.LBL_CAM_UP_L.TabIndex = 12;
      this.LBL_CAM_UP_L.Text = "Left Eye";
      this.LBL_CAM_UP_L.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // LBL_CAM_UP_R
      // 
      this.LBL_CAM_UP_R.AutoSize = true;
      this.LBL_CAM_UP_R.Dock = System.Windows.Forms.DockStyle.Fill;
      this.LBL_CAM_UP_R.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.LBL_CAM_UP_R.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
      this.LBL_CAM_UP_R.Location = new System.Drawing.Point(4, 121);
      this.LBL_CAM_UP_R.Name = "LBL_CAM_UP_R";
      this.LBL_CAM_UP_R.Size = new System.Drawing.Size(57, 14);
      this.LBL_CAM_UP_R.TabIndex = 13;
      this.LBL_CAM_UP_R.Text = "Right Eye";
      this.LBL_CAM_UP_R.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // LBL_FIELD_OF_VIEW_L
      // 
      this.LBL_FIELD_OF_VIEW_L.AutoSize = true;
      this.LBL_FIELD_OF_VIEW_L.Dock = System.Windows.Forms.DockStyle.Fill;
      this.LBL_FIELD_OF_VIEW_L.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.LBL_FIELD_OF_VIEW_L.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
      this.LBL_FIELD_OF_VIEW_L.Location = new System.Drawing.Point(4, 151);
      this.LBL_FIELD_OF_VIEW_L.Name = "LBL_FIELD_OF_VIEW_L";
      this.LBL_FIELD_OF_VIEW_L.Size = new System.Drawing.Size(57, 14);
      this.LBL_FIELD_OF_VIEW_L.TabIndex = 12;
      this.LBL_FIELD_OF_VIEW_L.Text = "Left Eye";
      this.LBL_FIELD_OF_VIEW_L.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // LBL_FIELD_OF_VIEW_R
      // 
      this.LBL_FIELD_OF_VIEW_R.AutoSize = true;
      this.LBL_FIELD_OF_VIEW_R.Dock = System.Windows.Forms.DockStyle.Fill;
      this.LBL_FIELD_OF_VIEW_R.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.LBL_FIELD_OF_VIEW_R.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
      this.LBL_FIELD_OF_VIEW_R.Location = new System.Drawing.Point(4, 166);
      this.LBL_FIELD_OF_VIEW_R.Name = "LBL_FIELD_OF_VIEW_R";
      this.LBL_FIELD_OF_VIEW_R.Size = new System.Drawing.Size(57, 14);
      this.LBL_FIELD_OF_VIEW_R.TabIndex = 13;
      this.LBL_FIELD_OF_VIEW_R.Text = "Right Eye";
      this.LBL_FIELD_OF_VIEW_R.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // GROP_TRAC_INFO_CNSL
      // 
      this.GROP_TRAC_INFO_CNSL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.GROP_TRAC_INFO_CNSL.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.GROP_TRAC_INFO_CNSL.Controls.Add(this.Tb_TRAC_INFO_CNSL);
      this.GROP_TRAC_INFO_CNSL.Location = new System.Drawing.Point(5, 75);
      this.GROP_TRAC_INFO_CNSL.Name = "GROP_TRAC_INFO_CNSL";
      this.GROP_TRAC_INFO_CNSL.Size = new System.Drawing.Size(190, 200);
      this.GROP_TRAC_INFO_CNSL.TabIndex = 36;
      this.GROP_TRAC_INFO_CNSL.TabStop = false;
      this.GROP_TRAC_INFO_CNSL.Text = "Camera Tracking Info Console";
      // 
      // Tb_TRAC_INFO_CNSL
      // 
      this.Tb_TRAC_INFO_CNSL.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.Tb_TRAC_INFO_CNSL.BackColor = System.Drawing.SystemColors.Control;
      this.Tb_TRAC_INFO_CNSL.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
      this.Tb_TRAC_INFO_CNSL.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
      this.Tb_TRAC_INFO_CNSL.ColumnCount = 2;
      this.Tb_TRAC_INFO_CNSL.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
      this.Tb_TRAC_INFO_CNSL.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
      this.Tb_TRAC_INFO_CNSL.Controls.Add(this.FIELD_OF_VIEW_R, 1, 11);
      this.Tb_TRAC_INFO_CNSL.Controls.Add(this.LBL_FIELD_OF_VIEW_R, 0, 11);
      this.Tb_TRAC_INFO_CNSL.Controls.Add(this.FIELD_OF_VIEW_L, 1, 10);
      this.Tb_TRAC_INFO_CNSL.Controls.Add(this.LBL_FIELD_OF_VIEW_L, 0, 10);
      this.Tb_TRAC_INFO_CNSL.Controls.Add(this.CAM_UP_R, 1, 8);
      this.Tb_TRAC_INFO_CNSL.Controls.Add(this.LBL_CAM_UP_R, 0, 8);
      this.Tb_TRAC_INFO_CNSL.Controls.Add(this.CAM_UP_L, 1, 7);
      this.Tb_TRAC_INFO_CNSL.Controls.Add(this.LBL_CAM_UP_L, 0, 7);
      this.Tb_TRAC_INFO_CNSL.Controls.Add(this.CAM_DIR_R, 1, 5);
      this.Tb_TRAC_INFO_CNSL.Controls.Add(this.LBL_CAM_DIR_R, 0, 5);
      this.Tb_TRAC_INFO_CNSL.Controls.Add(this.CAM_DIR_L, 1, 4);
      this.Tb_TRAC_INFO_CNSL.Controls.Add(this.LBL_CAM_LOC_L, 0, 1);
      this.Tb_TRAC_INFO_CNSL.Controls.Add(this.CAM_LOC_L, 1, 1);
      this.Tb_TRAC_INFO_CNSL.Controls.Add(this.CAM_LOC_R, 1, 2);
      this.Tb_TRAC_INFO_CNSL.Controls.Add(this.LBL_CAM_LOC_R, 0, 2);
      this.Tb_TRAC_INFO_CNSL.Controls.Add(this.LBL_CAM_DIR_L, 0, 4);
      this.Tb_TRAC_INFO_CNSL.Controls.Add(this.LB_CAM_LOC, 0, 0);
      this.Tb_TRAC_INFO_CNSL.Controls.Add(this.LB_CAM_UP, 0, 6);
      this.Tb_TRAC_INFO_CNSL.Controls.Add(this.LB_CAM_DIR, 0, 3);
      this.Tb_TRAC_INFO_CNSL.Controls.Add(this.LB_CAM_FOV, 0, 9);
      this.Tb_TRAC_INFO_CNSL.Dock = System.Windows.Forms.DockStyle.Fill;
      this.Tb_TRAC_INFO_CNSL.ForeColor = System.Drawing.SystemColors.WindowText;
      this.Tb_TRAC_INFO_CNSL.Location = new System.Drawing.Point(3, 16);
      this.Tb_TRAC_INFO_CNSL.Name = "Tb_TRAC_INFO_CNSL";
      this.Tb_TRAC_INFO_CNSL.RowCount = 12;
      this.Tb_TRAC_INFO_CNSL.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
      this.Tb_TRAC_INFO_CNSL.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
      this.Tb_TRAC_INFO_CNSL.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
      this.Tb_TRAC_INFO_CNSL.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
      this.Tb_TRAC_INFO_CNSL.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
      this.Tb_TRAC_INFO_CNSL.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
      this.Tb_TRAC_INFO_CNSL.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
      this.Tb_TRAC_INFO_CNSL.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
      this.Tb_TRAC_INFO_CNSL.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
      this.Tb_TRAC_INFO_CNSL.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
      this.Tb_TRAC_INFO_CNSL.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
      this.Tb_TRAC_INFO_CNSL.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
      this.Tb_TRAC_INFO_CNSL.Size = new System.Drawing.Size(184, 181);
      this.Tb_TRAC_INFO_CNSL.TabIndex = 32;
      // 
      // LB_CAM_LOC
      // 
      this.LB_CAM_LOC.AutoSize = true;
      this.Tb_TRAC_INFO_CNSL.SetColumnSpan(this.LB_CAM_LOC, 2);
      this.LB_CAM_LOC.Dock = System.Windows.Forms.DockStyle.Fill;
      this.LB_CAM_LOC.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.LB_CAM_LOC.ForeColor = System.Drawing.SystemColors.ControlText;
      this.LB_CAM_LOC.Location = new System.Drawing.Point(4, 1);
      this.LB_CAM_LOC.Name = "LB_CAM_LOC";
      this.LB_CAM_LOC.Size = new System.Drawing.Size(176, 14);
      this.LB_CAM_LOC.TabIndex = 14;
      this.LB_CAM_LOC.Text = "Location";
      this.LB_CAM_LOC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // LB_CAM_UP
      // 
      this.LB_CAM_UP.AutoSize = true;
      this.Tb_TRAC_INFO_CNSL.SetColumnSpan(this.LB_CAM_UP, 2);
      this.LB_CAM_UP.Dock = System.Windows.Forms.DockStyle.Fill;
      this.LB_CAM_UP.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.LB_CAM_UP.ForeColor = System.Drawing.SystemColors.ControlText;
      this.LB_CAM_UP.Location = new System.Drawing.Point(4, 91);
      this.LB_CAM_UP.Name = "LB_CAM_UP";
      this.LB_CAM_UP.Size = new System.Drawing.Size(176, 14);
      this.LB_CAM_UP.TabIndex = 16;
      this.LB_CAM_UP.Text = "Up";
      this.LB_CAM_UP.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // LB_CAM_DIR
      // 
      this.LB_CAM_DIR.AutoSize = true;
      this.Tb_TRAC_INFO_CNSL.SetColumnSpan(this.LB_CAM_DIR, 2);
      this.LB_CAM_DIR.Dock = System.Windows.Forms.DockStyle.Fill;
      this.LB_CAM_DIR.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.LB_CAM_DIR.ForeColor = System.Drawing.SystemColors.ControlText;
      this.LB_CAM_DIR.Location = new System.Drawing.Point(4, 46);
      this.LB_CAM_DIR.Name = "LB_CAM_DIR";
      this.LB_CAM_DIR.Size = new System.Drawing.Size(176, 14);
      this.LB_CAM_DIR.TabIndex = 15;
      this.LB_CAM_DIR.Text = "Direction";
      this.LB_CAM_DIR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // LB_CAM_FOV
      // 
      this.LB_CAM_FOV.AutoSize = true;
      this.Tb_TRAC_INFO_CNSL.SetColumnSpan(this.LB_CAM_FOV, 2);
      this.LB_CAM_FOV.Dock = System.Windows.Forms.DockStyle.Fill;
      this.LB_CAM_FOV.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.LB_CAM_FOV.ForeColor = System.Drawing.SystemColors.ControlText;
      this.LB_CAM_FOV.Location = new System.Drawing.Point(4, 136);
      this.LB_CAM_FOV.Name = "LB_CAM_FOV";
      this.LB_CAM_FOV.Size = new System.Drawing.Size(176, 14);
      this.LB_CAM_FOV.TabIndex = 17;
      this.LB_CAM_FOV.Text = "FOV";
      this.LB_CAM_FOV.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // GROP_VR_OPTN
      // 
      this.GROP_VR_OPTN.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.GROP_VR_OPTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.GROP_VR_OPTN.Controls.Add(this.TB_VR_OPTN);
      this.GROP_VR_OPTN.Location = new System.Drawing.Point(5, 280);
      this.GROP_VR_OPTN.Name = "GROP_VR_OPTN";
      this.GROP_VR_OPTN.Size = new System.Drawing.Size(190, 329);
      this.GROP_VR_OPTN.TabIndex = 37;
      this.GROP_VR_OPTN.TabStop = false;
      this.GROP_VR_OPTN.Text = "VR Option";
      // 
      // TB_VR_OPTN
      // 
      this.TB_VR_OPTN.AllowDrop = true;
      this.TB_VR_OPTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.TB_VR_OPTN.BackColor = System.Drawing.SystemColors.ControlLightLight;
      this.TB_VR_OPTN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
      this.TB_VR_OPTN.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
      this.TB_VR_OPTN.ColumnCount = 2;
      this.TB_VR_OPTN.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.TB_VR_OPTN.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.TB_VR_OPTN.Controls.Add(this.LB_USER_OPTN, 0, 0);
      this.TB_VR_OPTN.Controls.Add(this.EYE_HEIGHT, 1, 3);
      this.TB_VR_OPTN.Controls.Add(this.LB_INPT_OPTN, 0, 7);
      this.TB_VR_OPTN.Controls.Add(this.CB_MIRROR, 1, 6);
      this.TB_VR_OPTN.Controls.Add(this.LB_MIRROR, 0, 6);
      this.TB_VR_OPTN.Controls.Add(this.LB_DISPLAY_MODE, 0, 5);
      this.TB_VR_OPTN.Controls.Add(this.DISPLAY_MODE, 1, 5);
      this.TB_VR_OPTN.Controls.Add(this.LB_EYE_HEIGHT, 0, 3);
      this.TB_VR_OPTN.Controls.Add(this.LB_FLOORS, 0, 2);
      this.TB_VR_OPTN.Controls.Add(this.START_FLOORS, 1, 2);
      this.TB_VR_OPTN.Controls.Add(this.START_POSITION, 1, 1);
      this.TB_VR_OPTN.Controls.Add(this.LB_START_POSITION, 0, 1);
      this.TB_VR_OPTN.Controls.Add(this.LB_DSPL_OPTN, 0, 4);
      this.TB_VR_OPTN.Controls.Add(this.LB_VOICE, 0, 8);
      this.TB_VR_OPTN.Controls.Add(this.CB_VOICE, 1, 8);
      this.TB_VR_OPTN.Controls.Add(this.LB_GEO_LOCATION, 0, 9);
      this.TB_VR_OPTN.Controls.Add(this.LB_LATITUDE, 0, 10);
      this.TB_VR_OPTN.Controls.Add(this.LATITUDE, 1, 10);
      this.TB_VR_OPTN.Controls.Add(this.LB_LONGITUDE, 0, 11);
      this.TB_VR_OPTN.Controls.Add(this.LONGITUDE, 1, 11);
      this.TB_VR_OPTN.Dock = System.Windows.Forms.DockStyle.Fill;
      this.TB_VR_OPTN.Location = new System.Drawing.Point(3, 16);
      this.TB_VR_OPTN.Name = "TB_VR_OPTN";
      this.TB_VR_OPTN.RightToLeft = System.Windows.Forms.RightToLeft.No;
      this.TB_VR_OPTN.RowCount = 12;
      this.TB_VR_OPTN.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.142857F));
      this.TB_VR_OPTN.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.142857F));
      this.TB_VR_OPTN.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.142857F));
      this.TB_VR_OPTN.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.142857F));
      this.TB_VR_OPTN.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.142857F));
      this.TB_VR_OPTN.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.142857F));
      this.TB_VR_OPTN.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.142857F));
      this.TB_VR_OPTN.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.142857F));
      this.TB_VR_OPTN.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.142857F));
      this.TB_VR_OPTN.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.142857F));
      this.TB_VR_OPTN.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.825243F));
      this.TB_VR_OPTN.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.796116F));
      this.TB_VR_OPTN.Size = new System.Drawing.Size(184, 310);
      this.TB_VR_OPTN.TabIndex = 32;
      // 
      // LB_USER_OPTN
      // 
      this.LB_USER_OPTN.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.LB_USER_OPTN.AutoSize = true;
      this.TB_VR_OPTN.SetColumnSpan(this.LB_USER_OPTN, 2);
      this.LB_USER_OPTN.Font = new System.Drawing.Font("Arial", 8.5F);
      this.LB_USER_OPTN.ForeColor = System.Drawing.SystemColors.ControlText;
      this.LB_USER_OPTN.Location = new System.Drawing.Point(4, 1);
      this.LB_USER_OPTN.Name = "LB_USER_OPTN";
      this.LB_USER_OPTN.Size = new System.Drawing.Size(176, 25);
      this.LB_USER_OPTN.TabIndex = 34;
      this.LB_USER_OPTN.Text = "User Option";
      this.LB_USER_OPTN.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // EYE_HEIGHT
      // 
      this.EYE_HEIGHT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.EYE_HEIGHT.BackColor = System.Drawing.SystemColors.Window;
      this.EYE_HEIGHT.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.EYE_HEIGHT.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.EYE_HEIGHT.Location = new System.Drawing.Point(93, 84);
      this.EYE_HEIGHT.Margin = new System.Windows.Forms.Padding(1);
      this.EYE_HEIGHT.MinimumSize = new System.Drawing.Size(0, 17);
      this.EYE_HEIGHT.Name = "EYE_HEIGHT";
      this.EYE_HEIGHT.Size = new System.Drawing.Size(89, 17);
      this.EYE_HEIGHT.TabIndex = 22;
      this.EYE_HEIGHT.Text = "1.6";
      this.EYE_HEIGHT.TextChanged += new System.EventHandler(this.EYE_HEIGHT_TextChanged);
      // 
      // LB_INPT_OPTN
      // 
      this.LB_INPT_OPTN.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.LB_INPT_OPTN.AutoSize = true;
      this.TB_VR_OPTN.SetColumnSpan(this.LB_INPT_OPTN, 2);
      this.LB_INPT_OPTN.Font = new System.Drawing.Font("Arial", 8.5F);
      this.LB_INPT_OPTN.ForeColor = System.Drawing.SystemColors.ControlText;
      this.LB_INPT_OPTN.Location = new System.Drawing.Point(4, 183);
      this.LB_INPT_OPTN.Name = "LB_INPT_OPTN";
      this.LB_INPT_OPTN.Size = new System.Drawing.Size(176, 25);
      this.LB_INPT_OPTN.TabIndex = 33;
      this.LB_INPT_OPTN.Text = "Input Option";
      this.LB_INPT_OPTN.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // CB_MIRROR
      // 
      this.CB_MIRROR.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.CB_MIRROR.AutoSize = true;
      this.CB_MIRROR.Checked = true;
      this.CB_MIRROR.CheckState = System.Windows.Forms.CheckState.Checked;
      this.CB_MIRROR.Location = new System.Drawing.Point(95, 160);
      this.CB_MIRROR.Name = "CB_MIRROR";
      this.CB_MIRROR.Size = new System.Drawing.Size(85, 19);
      this.CB_MIRROR.TabIndex = 32;
      this.CB_MIRROR.UseVisualStyleBackColor = true;
      this.CB_MIRROR.CheckedChanged += new System.EventHandler(this.CB_MIRROR_CheckedChanged);
      // 
      // LB_MIRROR
      // 
      this.LB_MIRROR.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.LB_MIRROR.AutoSize = true;
      this.LB_MIRROR.Font = new System.Drawing.Font("Arial", 8.5F);
      this.LB_MIRROR.ForeColor = System.Drawing.SystemColors.ControlText;
      this.LB_MIRROR.Location = new System.Drawing.Point(4, 157);
      this.LB_MIRROR.Name = "LB_MIRROR";
      this.LB_MIRROR.Size = new System.Drawing.Size(84, 25);
      this.LB_MIRROR.TabIndex = 31;
      this.LB_MIRROR.Text = "Mirror";
      this.LB_MIRROR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // LB_DISPLAY_MODE
      // 
      this.LB_DISPLAY_MODE.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.LB_DISPLAY_MODE.AutoSize = true;
      this.LB_DISPLAY_MODE.Font = new System.Drawing.Font("Arial", 8.5F);
      this.LB_DISPLAY_MODE.ForeColor = System.Drawing.SystemColors.ControlText;
      this.LB_DISPLAY_MODE.Location = new System.Drawing.Point(4, 131);
      this.LB_DISPLAY_MODE.Name = "LB_DISPLAY_MODE";
      this.LB_DISPLAY_MODE.Size = new System.Drawing.Size(84, 25);
      this.LB_DISPLAY_MODE.TabIndex = 29;
      this.LB_DISPLAY_MODE.Text = "Display Mode";
      this.LB_DISPLAY_MODE.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // DISPLAY_MODE
      // 
      this.DISPLAY_MODE.Dock = System.Windows.Forms.DockStyle.Fill;
      this.DISPLAY_MODE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.DISPLAY_MODE.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.DISPLAY_MODE.FormattingEnabled = true;
      this.DISPLAY_MODE.Location = new System.Drawing.Point(92, 131);
      this.DISPLAY_MODE.Margin = new System.Windows.Forms.Padding(0);
      this.DISPLAY_MODE.Name = "DISPLAY_MODE";
      this.DISPLAY_MODE.Size = new System.Drawing.Size(91, 21);
      this.DISPLAY_MODE.TabIndex = 30;
      this.DISPLAY_MODE.SelectedIndexChanged += new System.EventHandler(this.DISPLAY_MODE_SelectedIndexChanged);
      // 
      // LB_EYE_HEIGHT
      // 
      this.LB_EYE_HEIGHT.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.LB_EYE_HEIGHT.AutoSize = true;
      this.LB_EYE_HEIGHT.Font = new System.Drawing.Font("Arial", 8.5F);
      this.LB_EYE_HEIGHT.ForeColor = System.Drawing.SystemColors.ControlText;
      this.LB_EYE_HEIGHT.Location = new System.Drawing.Point(4, 79);
      this.LB_EYE_HEIGHT.Name = "LB_EYE_HEIGHT";
      this.LB_EYE_HEIGHT.Size = new System.Drawing.Size(84, 25);
      this.LB_EYE_HEIGHT.TabIndex = 21;
      this.LB_EYE_HEIGHT.Text = "Eye Height";
      this.LB_EYE_HEIGHT.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // LB_FLOORS
      // 
      this.LB_FLOORS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.LB_FLOORS.AutoSize = true;
      this.LB_FLOORS.Font = new System.Drawing.Font("Arial", 8.5F);
      this.LB_FLOORS.ForeColor = System.Drawing.SystemColors.ControlText;
      this.LB_FLOORS.Location = new System.Drawing.Point(4, 53);
      this.LB_FLOORS.Name = "LB_FLOORS";
      this.LB_FLOORS.Size = new System.Drawing.Size(84, 25);
      this.LB_FLOORS.TabIndex = 18;
      this.LB_FLOORS.Text = "Floors";
      this.LB_FLOORS.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // START_FLOORS
      // 
      this.START_FLOORS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.START_FLOORS.AutoSize = true;
      this.START_FLOORS.Location = new System.Drawing.Point(92, 53);
      this.START_FLOORS.Margin = new System.Windows.Forms.Padding(0);
      this.START_FLOORS.Name = "START_FLOORS";
      this.START_FLOORS.Size = new System.Drawing.Size(91, 25);
      this.START_FLOORS.TabIndex = 20;
      this.START_FLOORS.Text = "Select";
      this.START_FLOORS.UseVisualStyleBackColor = true;
      this.START_FLOORS.Click += new System.EventHandler(this.START_FLOORS_Click);
      // 
      // START_POSITION
      // 
      this.START_POSITION.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.START_POSITION.AutoSize = true;
      this.START_POSITION.Location = new System.Drawing.Point(92, 27);
      this.START_POSITION.Margin = new System.Windows.Forms.Padding(0);
      this.START_POSITION.Name = "START_POSITION";
      this.START_POSITION.Size = new System.Drawing.Size(91, 25);
      this.START_POSITION.TabIndex = 19;
      this.START_POSITION.Text = "Place...";
      this.START_POSITION.UseVisualStyleBackColor = true;
      this.START_POSITION.Click += new System.EventHandler(this.START_POSITION_Click);
      // 
      // LB_START_POSITION
      // 
      this.LB_START_POSITION.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.LB_START_POSITION.AutoSize = true;
      this.LB_START_POSITION.Font = new System.Drawing.Font("Arial", 8.5F);
      this.LB_START_POSITION.ForeColor = System.Drawing.SystemColors.ControlText;
      this.LB_START_POSITION.Location = new System.Drawing.Point(4, 27);
      this.LB_START_POSITION.Name = "LB_START_POSITION";
      this.LB_START_POSITION.Size = new System.Drawing.Size(84, 25);
      this.LB_START_POSITION.TabIndex = 14;
      this.LB_START_POSITION.Text = "Start Position";
      this.LB_START_POSITION.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // LB_DSPL_OPTN
      // 
      this.LB_DSPL_OPTN.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.LB_DSPL_OPTN.AutoSize = true;
      this.TB_VR_OPTN.SetColumnSpan(this.LB_DSPL_OPTN, 2);
      this.LB_DSPL_OPTN.Font = new System.Drawing.Font("Arial", 8.5F);
      this.LB_DSPL_OPTN.ForeColor = System.Drawing.SystemColors.ControlText;
      this.LB_DSPL_OPTN.Location = new System.Drawing.Point(4, 105);
      this.LB_DSPL_OPTN.Name = "LB_DSPL_OPTN";
      this.LB_DSPL_OPTN.Size = new System.Drawing.Size(176, 25);
      this.LB_DSPL_OPTN.TabIndex = 35;
      this.LB_DSPL_OPTN.Text = "Display Option";
      this.LB_DSPL_OPTN.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // LB_VOICE
      // 
      this.LB_VOICE.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.LB_VOICE.AutoSize = true;
      this.LB_VOICE.Font = new System.Drawing.Font("Arial", 8.5F);
      this.LB_VOICE.ForeColor = System.Drawing.SystemColors.ControlText;
      this.LB_VOICE.Location = new System.Drawing.Point(4, 209);
      this.LB_VOICE.Name = "LB_VOICE";
      this.LB_VOICE.Size = new System.Drawing.Size(84, 25);
      this.LB_VOICE.TabIndex = 25;
      this.LB_VOICE.Text = "Voice";
      this.LB_VOICE.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // CB_VOICE
      // 
      this.CB_VOICE.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.CB_VOICE.AutoSize = true;
      this.CB_VOICE.Checked = true;
      this.CB_VOICE.CheckState = System.Windows.Forms.CheckState.Checked;
      this.CB_VOICE.Location = new System.Drawing.Point(95, 212);
      this.CB_VOICE.Name = "CB_VOICE";
      this.CB_VOICE.Size = new System.Drawing.Size(85, 19);
      this.CB_VOICE.TabIndex = 28;
      this.CB_VOICE.UseVisualStyleBackColor = true;
      this.CB_VOICE.CheckedChanged += new System.EventHandler(this.CB_VOICE_CheckedChanged);
      // 
      // LB_GEO_LOCATION
      // 
      this.LB_GEO_LOCATION.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.LB_GEO_LOCATION.AutoSize = true;
      this.TB_VR_OPTN.SetColumnSpan(this.LB_GEO_LOCATION, 2);
      this.LB_GEO_LOCATION.Font = new System.Drawing.Font("Arial", 8.5F);
      this.LB_GEO_LOCATION.ForeColor = System.Drawing.SystemColors.ControlText;
      this.LB_GEO_LOCATION.Location = new System.Drawing.Point(4, 235);
      this.LB_GEO_LOCATION.Name = "LB_GEO_LOCATION";
      this.LB_GEO_LOCATION.Size = new System.Drawing.Size(176, 25);
      this.LB_GEO_LOCATION.TabIndex = 36;
      this.LB_GEO_LOCATION.Text = "Geo Location";
      this.LB_GEO_LOCATION.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // LB_LATITUDE
      // 
      this.LB_LATITUDE.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.LB_LATITUDE.AutoSize = true;
      this.LB_LATITUDE.Font = new System.Drawing.Font("Arial", 8.5F);
      this.LB_LATITUDE.ForeColor = System.Drawing.SystemColors.ControlText;
      this.LB_LATITUDE.Location = new System.Drawing.Point(4, 261);
      this.LB_LATITUDE.Name = "LB_LATITUDE";
      this.LB_LATITUDE.Size = new System.Drawing.Size(84, 20);
      this.LB_LATITUDE.TabIndex = 37;
      this.LB_LATITUDE.Text = "Latitude";
      this.LB_LATITUDE.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // LATITUDE
      // 
      this.LATITUDE.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.LATITUDE.BackColor = System.Drawing.SystemColors.Window;
      this.LATITUDE.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.LATITUDE.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.LATITUDE.Location = new System.Drawing.Point(93, 264);
      this.LATITUDE.Margin = new System.Windows.Forms.Padding(1);
      this.LATITUDE.MinimumSize = new System.Drawing.Size(0, 17);
      this.LATITUDE.Name = "LATITUDE";
      this.LATITUDE.Size = new System.Drawing.Size(89, 17);
      this.LATITUDE.TabIndex = 39;
      this.LATITUDE.Text = "28.546474";
      this.LATITUDE.TextChanged += new System.EventHandler(this.LATITUDE_TextChanged);
      // 
      // LB_LONGITUDE
      // 
      this.LB_LONGITUDE.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.LB_LONGITUDE.AutoSize = true;
      this.LB_LONGITUDE.Font = new System.Drawing.Font("Arial", 8.5F);
      this.LB_LONGITUDE.ForeColor = System.Drawing.SystemColors.ControlText;
      this.LB_LONGITUDE.Location = new System.Drawing.Point(4, 282);
      this.LB_LONGITUDE.Name = "LB_LONGITUDE";
      this.LB_LONGITUDE.Size = new System.Drawing.Size(84, 27);
      this.LB_LONGITUDE.TabIndex = 38;
      this.LB_LONGITUDE.Text = "Longitude";
      this.LB_LONGITUDE.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // LONGITUDE
      // 
      this.LONGITUDE.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
      this.LONGITUDE.BackColor = System.Drawing.SystemColors.Window;
      this.LONGITUDE.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.LONGITUDE.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.LONGITUDE.Location = new System.Drawing.Point(93, 288);
      this.LONGITUDE.Margin = new System.Windows.Forms.Padding(1);
      this.LONGITUDE.MinimumSize = new System.Drawing.Size(0, 17);
      this.LONGITUDE.Name = "LONGITUDE";
      this.LONGITUDE.Size = new System.Drawing.Size(89, 17);
      this.LONGITUDE.TabIndex = 40;
      this.LONGITUDE.Text = "-81.385560";
      this.LONGITUDE.TextChanged += new System.EventHandler(this.LONGITUDE_TextChanged);
      // 
      // LB_LING_DOT_WORLD
      // 
      this.LB_LING_DOT_WORLD.AutoSize = true;
      this.LB_LING_DOT_WORLD.Location = new System.Drawing.Point(5, 612);
      this.LB_LING_DOT_WORLD.Name = "LB_LING_DOT_WORLD";
      this.LB_LING_DOT_WORLD.Size = new System.Drawing.Size(50, 13);
      this.LB_LING_DOT_WORLD.TabIndex = 38;
      this.LB_LING_DOT_WORLD.TabStop = true;
      this.LB_LING_DOT_WORLD.Text = "RhinoVR";
      this.LB_LING_DOT_WORLD.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
      this.label1.Location = new System.Drawing.Point(132, 612);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(50, 13);
      this.label1.TabIndex = 39;
      this.label1.Text = "RhinoVR";
      // 
      // VR_PANEL
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoScroll = true;
      this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.Controls.Add(this.label1);
      this.Controls.Add(this.LB_LING_DOT_WORLD);
      this.Controls.Add(this.GROP_VR_OPTN);
      this.Controls.Add(this.GROP_TRAC_INFO_CNSL);
      this.Controls.Add(this.STOP_VR);
      this.Controls.Add(this.START_VR);
      this.Name = "VR_PANEL";
      this.Size = new System.Drawing.Size(200, 640);
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
