using Photon.Deterministic;

namespace Redbean.Network
{
	public class QCommandTurnEnd : DeterministicCommand
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