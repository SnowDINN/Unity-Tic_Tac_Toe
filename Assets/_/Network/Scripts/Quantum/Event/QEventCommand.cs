using Photon.Deterministic;

namespace Quantum
{
	/// <summary>
	/// 네트워크 이벤트를 수신하는 로컬 이벤트
	/// </summary>
	public class QEventCommand
	{
		public PlayerRef Player;
		public DeterministicCommand Command;
	}
}