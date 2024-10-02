using Photon.Deterministic;
using Quantum;

namespace Redbean.Network
{
	public class QCommandTurnEnd : DeterministicCommand
	{
		public AssetRef<EntityPrototype> Entity;
		public int X;
		public int Y;
		
		public override void Serialize(BitStream stream)
		{
			stream.Serialize(ref Entity);
			stream.Serialize(ref X);
			stream.Serialize(ref Y);
		}
	}
}