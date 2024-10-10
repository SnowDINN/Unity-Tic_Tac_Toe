using Photon.Deterministic;

namespace Quantum
{
	public class QCommandGameTurn : DeterministicCommand
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