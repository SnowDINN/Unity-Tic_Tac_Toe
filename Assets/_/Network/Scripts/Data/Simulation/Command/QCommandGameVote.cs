using Photon.Deterministic;

namespace Redbean.Network
{
	public class QCommandGameVote : DeterministicCommand
	{
		public int VoteType;
		public int ActorId;
		
		public override void Serialize(BitStream stream)
		{
			stream.Serialize(ref VoteType);
			stream.Serialize(ref ActorId);
		}
	}
}