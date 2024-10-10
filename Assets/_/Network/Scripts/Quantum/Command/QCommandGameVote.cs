using Photon.Deterministic;

namespace Quantum
{
	public class QCommandGameVote : DeterministicCommand
	{
		public int VoteType;
		public PlayerRef VotePlayer;
		
		public override void Serialize(BitStream stream)
		{
			stream.Serialize(ref VoteType);
			stream.Serialize(ref VotePlayer);
		}
	}
}