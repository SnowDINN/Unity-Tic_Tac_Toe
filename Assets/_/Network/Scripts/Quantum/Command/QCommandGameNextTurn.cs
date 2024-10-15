using Photon.Deterministic;

namespace Quantum
{
	/// <summary>
	/// 다음 턴 동기화 네트워크 이벤트
	/// </summary>
	public class QCommandGameNextTurn : DeterministicCommand
	{
		public int X;
		public int Y;
		
		public override void Serialize(BitStream stream)
		{
			stream.Serialize(ref X);
			stream.Serialize(ref Y);
		}
	}
}