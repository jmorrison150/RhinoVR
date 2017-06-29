using System;

namespace RhinoVR {
  internal class utils {
    public static float clamp(float value, float min, float max) {
      if ((double)value < (double)min)
        return min;
      if ((double)value <= (double)max)
        return value;
      return max;
    }

    public static float extractFractional(float value) {
      return value - (float)Math.Truncate((double)value);
    }
  }
}
