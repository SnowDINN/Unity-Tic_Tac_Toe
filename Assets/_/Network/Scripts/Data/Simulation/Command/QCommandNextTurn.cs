using Photon.Deterministic;

namespace Redbean.Network
{
	public class QCommandNextTurn : DeterministicCommand
	{
		public int NextPlayerTurn;
		
		public override void Serialize(BitStream stream)
		{
			stream.Serialize(ref NextPlayerTurn);
		}
	}
}