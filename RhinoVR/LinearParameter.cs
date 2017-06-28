// Decompiled with JetBrains decompiler
// Type: RhinoVR.LinearParameter
// Assembly: RhinoVR, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0F066D18-B920-40E4-BC83-5E6F0AA166E5
// Assembly location: C:\Program Files\Rhinoceros 5 (64-bit)\Plug-ins\RhinoVR.dll

namespace RhinoVR {
  internal class LinearParameter {
    private float _valueA;
    private float _valueB;
    private float _input;

    public float input { get { return this._input; } }
    public float valueA { get { return this._valueA; } }
    public float valueB { get { return this._valueB; } }

    public LinearParameter(float valueA, float valueB, float initialInput) {
      this._valueA = valueA;
      this._valueB = valueB;
      this._input = initialInput;
    }

    public void setInput(float input) {
      this._input = utils.clamp(input, 0.0f, 1f);
    }

    public float output() {
      return (float)((1.0 - (double)this._input) * (double)this._valueA + (double)this._input * (double)this._valueB);
    }
  }
}
