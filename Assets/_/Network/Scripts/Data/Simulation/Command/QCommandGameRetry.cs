using Photon.Deterministic;

namespace Redbean.Network
{
	public class QCommandGameRetry : DeterministicCommand
	{
		public int ActorId;
		
		public override void Serialize(BitStream stream)
		{
			stream.Serialize(ref ActorId);
		}
	}
}