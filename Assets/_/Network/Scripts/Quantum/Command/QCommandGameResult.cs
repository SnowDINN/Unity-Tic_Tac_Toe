using Photon.Deterministic;

namespace Quantum
{
	public class QCommandGameResult : DeterministicCommand
	{
		public PlayerRef WinnerPlayer;
		
		public override void Serialize(BitStream stream)
		{
			stream.Serialize(ref WinnerPlayer);
		}
	}
}