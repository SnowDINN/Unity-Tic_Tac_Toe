using Quantum;
using Redbean.Content;

namespace Redbean.Network
{
	public class StoneView : QuantumEntityViewComponent
	{
		private int x;
		private int y;
		private int owner;

		public override void OnActivate(Frame frame)
		{
			x = frame.Get<Stone>(EntityRef).X;
			y = frame.Get<Stone>(EntityRef).Y;
			owner = frame.Get<Stone>(EntityRef).Owner;
			
			GameSubscriber.Spawn(new SpawnStream
			{
				Owner = owner,
				X = x,
				Y = y
			});

			frame.Signals.OnStoneMatch(x, y);
		}
	}
}	