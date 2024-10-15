using Photon.Deterministic;

namespace Quantum
{
	/// <summary>
	/// 게임 투표 동기화 네트워크 이벤트
	/// </summary>
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