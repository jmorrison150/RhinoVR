// Decompiled with JetBrains decompiler
// Type: RhinoVR.Properties.MainIcon
// Assembly: RhinoVR, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0F066D18-B920-40E4-BC83-5E6F0AA166E5
// Assembly location: C:\Program Files\Rhinoceros 5 (64-bit)\Plug-ins\RhinoVR.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace RhinoVR.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class MainIcon
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) MainIcon.resourceMan, (object) null))
          MainIcon.resourceMan = new ResourceManager("RhinoVR.Properties.MainIcon", typeof (MainIcon).Assembly);
        return MainIcon.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get
      {
        return MainIcon.resourceCulture;
      }
      set
      {
        MainIcon.resourceCulture = value;
      }
    }

    internal static Icon ling
    {
      get
      {
        return (Icon) MainIcon.ResourceManager.GetObject("ling", MainIcon.resourceCulture);
      }
    }

    internal MainIcon()
    {
    }
  }
}
