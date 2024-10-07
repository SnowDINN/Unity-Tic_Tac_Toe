// <auto-generated>
// This code was auto-generated by a tool, every time
// the tool executes this code will be reset.
//
// If you need to extend the classes generated to add
// fields or methods to them, please create partial
// declarations in another file.
// </auto-generated>
#pragma warning disable 0109
#pragma warning disable 1591


namespace Quantum {
  using Photon.Deterministic;
  using Quantum;
  using Quantum.Core;
  using Quantum.Collections;
  using Quantum.Inspector;
  using Quantum.Physics2D;
  using Quantum.Physics3D;
  using Byte = System.Byte;
  using SByte = System.SByte;
  using Int16 = System.Int16;
  using UInt16 = System.UInt16;
  using Int32 = System.Int32;
  using UInt32 = System.UInt32;
  using Int64 = System.Int64;
  using UInt64 = System.UInt64;
  using Boolean = System.Boolean;
  using String = System.String;
  using Object = System.Object;
  using FlagsAttribute = System.FlagsAttribute;
  using SerializableAttribute = System.SerializableAttribute;
  using MethodImplAttribute = System.Runtime.CompilerServices.MethodImplAttribute;
  using MethodImplOptions = System.Runtime.CompilerServices.MethodImplOptions;
  using FieldOffsetAttribute = System.Runtime.InteropServices.FieldOffsetAttribute;
  using StructLayoutAttribute = System.Runtime.InteropServices.StructLayoutAttribute;
  using LayoutKind = System.Runtime.InteropServices.LayoutKind;
  #if QUANTUM_UNITY //;
  using TooltipAttribute = UnityEngine.TooltipAttribute;
  using HeaderAttribute = UnityEngine.HeaderAttribute;
  using SpaceAttribute = UnityEngine.SpaceAttribute;
  using RangeAttribute = UnityEngine.RangeAttribute;
  using HideInInspectorAttribute = UnityEngine.HideInInspector;
  using PreserveAttribute = UnityEngine.Scripting.PreserveAttribute;
  using FormerlySerializedAsAttribute = UnityEngine.Serialization.FormerlySerializedAsAttribute;
  using MovedFromAttribute = UnityEngine.Scripting.APIUpdating.MovedFromAttribute;
  using CreateAssetMenu = UnityEngine.CreateAssetMenuAttribute;
  using RuntimeInitializeOnLoadMethodAttribute = UnityEngine.RuntimeInitializeOnLoadMethodAttribute;
  #endif //;
  
  public unsafe partial class Frame {
    public unsafe partial struct FrameEvents {
      static partial void GetEventTypeCountCodeGen(ref Int32 eventCount) {
        eventCount = 5;
      }
      static partial void GetParentEventIDCodeGen(Int32 eventID, ref Int32 parentEventID) {
        switch (eventID) {
          default: break;
        }
      }
      static partial void GetEventTypeCodeGen(Int32 eventID, ref System.Type result) {
        switch (eventID) {
          case EventOnStoneCreated.ID: result = typeof(EventOnStoneCreated); return;
          case EventOnStoneDestroyed.ID: result = typeof(EventOnStoneDestroyed); return;
          case EventOnStoneHighlighted.ID: result = typeof(EventOnStoneHighlighted); return;
          case EventOnStoneMatchValidation.ID: result = typeof(EventOnStoneMatchValidation); return;
          default: break;
        }
      }
      public EventOnStoneCreated OnStoneCreated(QComponentStone Stone) {
        if (_f.IsPredicted) return null;
        var ev = _f.Context.AcquireEvent<EventOnStoneCreated>(EventOnStoneCreated.ID);
        ev.Stone = Stone;
        _f.AddEvent(ev);
        return ev;
      }
      public EventOnStoneDestroyed OnStoneDestroyed(QComponentStone Stone) {
        if (_f.IsPredicted) return null;
        var ev = _f.Context.AcquireEvent<EventOnStoneDestroyed>(EventOnStoneDestroyed.ID);
        ev.Stone = Stone;
        _f.AddEvent(ev);
        return ev;
      }
      public EventOnStoneHighlighted OnStoneHighlighted(QComponentStone Stone) {
        if (_f.IsPredicted) return null;
        var ev = _f.Context.AcquireEvent<EventOnStoneHighlighted>(EventOnStoneHighlighted.ID);
        ev.Stone = Stone;
        _f.AddEvent(ev);
        return ev;
      }
      public EventOnStoneMatchValidation OnStoneMatchValidation(Int32 X, Int32 Y) {
        if (_f.IsPredicted) return null;
        var ev = _f.Context.AcquireEvent<EventOnStoneMatchValidation>(EventOnStoneMatchValidation.ID);
        ev.X = X;
        ev.Y = Y;
        _f.AddEvent(ev);
        return ev;
      }
    }
  }
  public unsafe partial class EventOnStoneCreated : EventBase {
    public new const Int32 ID = 1;
    public QComponentStone Stone;
    protected EventOnStoneCreated(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnStoneCreated() : 
        base(1, EventFlags.Server|EventFlags.Client|EventFlags.Synced) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 41;
        hash = hash * 31 + Stone.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventOnStoneDestroyed : EventBase {
    public new const Int32 ID = 2;
    public QComponentStone Stone;
    protected EventOnStoneDestroyed(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnStoneDestroyed() : 
        base(2, EventFlags.Server|EventFlags.Client|EventFlags.Synced) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 43;
        hash = hash * 31 + Stone.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventOnStoneHighlighted : EventBase {
    public new const Int32 ID = 3;
    public QComponentStone Stone;
    protected EventOnStoneHighlighted(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnStoneHighlighted() : 
        base(3, EventFlags.Server|EventFlags.Client|EventFlags.Synced) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 47;
        hash = hash * 31 + Stone.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventOnStoneMatchValidation : EventBase {
    public new const Int32 ID = 4;
    public Int32 X;
    public Int32 Y;
    protected EventOnStoneMatchValidation(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventOnStoneMatchValidation() : 
        base(4, EventFlags.Server|EventFlags.Client|EventFlags.Synced) {
    }
    public new QuantumGame Game {
      get {
        return (QuantumGame)base.Game;
      }
      set {
        base.Game = value;
      }
    }
    public override Int32 GetHashCode() {
      unchecked {
        var hash = 53;
        hash = hash * 31 + X.GetHashCode();
        hash = hash * 31 + Y.GetHashCode();
        return hash;
      }
    }
  }
}
#pragma warning restore 0109
#pragma warning restore 1591
