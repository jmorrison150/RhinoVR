using Rhino;
using Rhino.Display;
using Rhino.Geometry;
using System;
using System.Drawing;
using System.Globalization;

namespace RhinoVR {
  internal class SunControllerConduit : DisplayConduit {
    private RhinoDoc _rhinoDoc;
    private DateTime _datetime;
    private double _dateParam;
    private double _timeParam;
    private int _dateSlideRadius;
    private int _timeSlideRadious;
    private int _daysInYear;
    private int[] _daysInEachMonth;
    private int _year;
    private int _month;
    private int _day;
    private int _hour;
    private int _minute;
    private double _latitude;
    private double _longitude;
    private double _altitude;
    private Color _dateArcColor;
    private Color _timeArcColor;

    public SunControllerConduit() {
     
    }

    protected virtual void DrawForeground(DrawEventArgs e) {
      for (int index = 0; index < Viewports.RiftViews.Length; ++index) {
        if (Viewports.RiftViews[index] != null && Viewports.RiftViews[index].Document != null && e.Viewport.Name == Viewports.RiftViews[index].MainViewport.Name)
          this.SunSliderController(e);
      }
    }

    private void SunSliderController(DrawEventArgs e) {
      Rectangle bounds = e.Viewport.Bounds;
      if (this._dateParam != 0.0) {
        Point3d[] point3dArray = new Point3d[3];
        for (int index = 0; index < 3; ++index) {
          double num1 = (double)(bounds.Width / 2) + Math.Sin(Math.PI - Math.PI / 2.0 * (double)index) * (double)this._dateSlideRadius;
          double num2 = (double)(bounds.Height / 2) + Math.Cos(Math.PI - Math.PI / 2.0 * (double)index) * (double)this._dateSlideRadius;
          Point3d local  = point3dArray[index];
          Rhino.Geometry.Line world = e.Viewport.ClientToWorld(new System.Drawing.Point((int)num1, (int)num2));
          Point3d from = world.From;
          local = from;
        }
        e.Display.DrawArc(new Arc(new Circle(point3dArray[0], point3dArray[1], point3dArray[2]), 2.0 * Math.PI * this._dateParam), this._dateArcColor, 10);
      }
      if (this._timeParam != 0.0) {
        Point3d[] point3dArray = new Point3d[3];
        for (int index = 0; index < 3; ++index) {
          double num1 = (double)(bounds.Width / 2) + Math.Sin(Math.PI - Math.PI / 2.0 * (double)index) * (double)this._timeSlideRadious;
          double num2 = (double)(bounds.Height / 2) + Math.Cos(Math.PI - Math.PI / 2.0 * (double)index) * (double)this._timeSlideRadious;
          Point3d local = point3dArray[index];
          Line world = e.Viewport.ClientToWorld(new System.Drawing.Point((int)num1, (int)num2));
          Point3d from = world.From;
          local = from;
        }
        e.Display.DrawArc(new Arc(new Circle(point3dArray[0], point3dArray[1], point3dArray[2]), 2.0 * Math.PI * this._timeParam), this._timeArcColor, 10);
      }
      e.Display.Draw2dText(this._datetime.ToString("m"), Color.White, new Point2d((double)(bounds.Width / 2), (double)(bounds.Height / 2 - 13)), true, 20);
      e.Display.Draw2dText(this._datetime.ToString("t"), Color.White, new Point2d((double)(bounds.Width / 2), (double)(bounds.Height / 2 + 13)), true, 20);
      e.Display.Draw2dText("Date", this._dateArcColor, new Point2d((double)(bounds.Width / 2), (double)(bounds.Height / 2 - this._dateSlideRadius + 20)), true, 10);
      e.Display.Draw2dText("Time", this._timeArcColor, new Point2d((double)(bounds.Width / 2), (double)(bounds.Height / 2 - this._timeSlideRadious - 20)), true, 10);
    }

    public void SunlightSettings(RhinoDoc doc, double latitude, double longitude) {
      this._rhinoDoc = doc;
      Calendar calendar = (Calendar)new GregorianCalendar();
      DateTime now = DateTime.Now;
      this._year = now.Year;
      this._month = now.Month;
      this._day = now.Day;
      this._hour = now.Hour;
      this._minute = now.Minute;
      this._latitude = latitude;
      this._longitude = longitude;
      this._daysInYear = calendar.GetDaysInYear(this._year);
      for (int index = 0; index < 12; ++index)
        this._daysInEachMonth[index] = index != 0 ? this._daysInEachMonth[index - 1] + calendar.GetDaysInMonth(this._year, index + 1) : calendar.GetDaysInMonth(this._year, index + 1);
    }

    public void SunSetToNow(ref double dateParam, ref double timeParam) {
      if (dateParam != -1.0 || timeParam != -1.0)
        return;
      this._dateParam = this._month != 1 ? (double)(this._daysInEachMonth[this._month - 2] + this._day) / (double)this._daysInYear : (double)this._day / (double)this._daysInYear;
      this._timeParam = (double)(this._hour * 60 + this._minute) / 1439.0;
      dateParam = this._dateParam;
      timeParam = this._timeParam;
      this._datetime = new DateTime(this._year, this._month, this._day, this._hour, this._minute, 0);
      this._rhinoDoc.Lights.Sun.SetPosition(this._datetime, this._latitude, this._longitude);
    }

    public void SunColor(double altitude) {
      int red;
      int green1;
      int blue1;
      int maxValue;
      int green2;
      int blue2;
      if (altitude > 0.0) {
        red = (int)((double)byte.MaxValue * (altitude / 90.0));
        green1 = (int)((double)byte.MaxValue * (altitude / 90.0));
        blue1 = (int)((double)byte.MaxValue - (double)byte.MaxValue * (altitude / 90.0));
        maxValue = (int)byte.MaxValue;
        green2 = (int)((double)byte.MaxValue - (double)byte.MaxValue * (altitude / 90.0));
        blue2 = 0;
      } else {
        red = 0;
        green1 = 0;
        blue1 = (int)byte.MaxValue;
        maxValue = (int)byte.MaxValue;
        green2 = (int)byte.MaxValue;
        blue2 = 0;
      }
      this._dateArcColor = Color.FromArgb(0, maxValue, green2, blue2);
      this._timeArcColor = Color.FromArgb(0, red, green1, blue1);
    }

    public void SunLightOn() {
      this._rhinoDoc.Lights.Sun.Enabled = (true);
    }

    public void SunLightOff() {
      this._rhinoDoc.Lights.Sun.Enabled = (false);
    }

    public void SunPosition(double dateParam, double timeParam) {
      this._dateParam = dateParam;
      this._timeParam = timeParam;
      int num1 = (int)((double)(this._daysInYear - 1) * this._dateParam + 1.0);
      int num2 = (int)(1439.0 * this._timeParam);
      for (int index = 0; index < 12; ++index) {
        if (index == 0) {
          if (num1 >= 1 && num1 <= this._daysInEachMonth[index]) {
            this._month = index + 1;
            this._day = num1;
          }
        } else if (num1 > this._daysInEachMonth[index - 1] && num1 <= this._daysInEachMonth[index]) {
          this._month = index + 1;
          this._day = num1 - this._daysInEachMonth[index - 1];
        }
      }
      this._hour = (int)Math.Floor((double)(num2 / 60));
      this._minute = num2 % 60;
      this._datetime = new DateTime(this._year, this._month, this._day, this._hour, this._minute, 0);
      this._rhinoDoc.Lights.Sun.SetPosition(this._datetime, this._latitude, this._longitude);
      this._altitude = this._rhinoDoc.Lights.Sun.Altitude;
      this.SunColor(this._altitude);
    }
  }
}
