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
        eventCount = 3;
      }
      static partial void GetParentEventIDCodeGen(Int32 eventID, ref Int32 parentEventID) {
        switch (eventID) {
          default: break;
        }
      }
      static partial void GetEventTypeCodeGen(Int32 eventID, ref System.Type result) {
        switch (eventID) {
          case EventBoardMatch.ID: result = typeof(EventBoardMatch); return;
          case EventStoneHighlight.ID: result = typeof(EventStoneHighlight); return;
          default: break;
        }
      }
      public EventBoardMatch BoardMatch(Int32 ActorId) {
        var ev = _f.Context.AcquireEvent<EventBoardMatch>(EventBoardMatch.ID);
        ev.ActorId = ActorId;
        _f.AddEvent(ev);
        return ev;
      }
      public EventStoneHighlight StoneHighlight(Stone Stone) {
        var ev = _f.Context.AcquireEvent<EventStoneHighlight>(EventStoneHighlight.ID);
        ev.Stone = Stone;
        _f.AddEvent(ev);
        return ev;
      }
    }
  }
  public unsafe partial class EventBoardMatch : EventBase {
    public new const Int32 ID = 1;
    public Int32 ActorId;
    protected EventBoardMatch(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventBoardMatch() : 
        base(1, EventFlags.Server|EventFlags.Client) {
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
        hash = hash * 31 + ActorId.GetHashCode();
        return hash;
      }
    }
  }
  public unsafe partial class EventStoneHighlight : EventBase {
    public new const Int32 ID = 2;
    public Stone Stone;
    protected EventStoneHighlight(Int32 id, EventFlags flags) : 
        base(id, flags) {
    }
    public EventStoneHighlight() : 
        base(2, EventFlags.Server|EventFlags.Client) {
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
}
#pragma warning restore 0109
#pragma warning restore 1591
