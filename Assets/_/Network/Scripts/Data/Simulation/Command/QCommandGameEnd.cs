using Photon.Deterministic;

namespace Redbean.Network
{
	public class QCommandGameEnd : DeterministicCommand
	{
		public int WinnerId;
		
		public override void Serialize(BitStream stream)
		{
			stream.Serialize(ref WinnerId);
		}
	}
}