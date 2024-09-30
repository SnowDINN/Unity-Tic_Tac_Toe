using Quantum;
using Redbean.Content;

namespace Redbean.Network
{
	public class StoneView : QuantumEntityViewComponent
	{
		private int position;
		private int owner;

		public override void OnActivate(Frame frame)
		{
			position = frame.Get<Stone>(EntityRef).Position;
			owner = frame.Get<Stone>(EntityRef).Owner;
		}

		private void Start()
		{
			GameSubscriber.Spawn(position, owner);
		}
	}
}	