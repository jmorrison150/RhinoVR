// Decompiled with JetBrains decompiler
// Type: RhinoVR.utils
// Assembly: RhinoVR, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0F066D18-B920-40E4-BC83-5E6F0AA166E5
// Assembly location: C:\Program Files\Rhinoceros 5 (64-bit)\Plug-ins\RhinoVR.dll

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
