using Photon.Deterministic;
using Quantum;

namespace Redbean.Network
{
	public class QCommandStoneDestroy : DeterministicCommand
	{
		public EntityRef Entity;
		
		public override void Serialize(BitStream stream)
		{
			stream.Serialize(ref Entity);
		}
	}
}