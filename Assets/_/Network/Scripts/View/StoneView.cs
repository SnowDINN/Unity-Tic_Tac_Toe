using Quantum;
using Redbean.Content;

namespace Redbean.Network
{
	public class StoneView : QuantumEntityViewComponent
	{
		private int index;
		private int Owner;

		public override void OnActivate(Frame frame)
		{
			index = frame.Get<Stone>(EntityRef).Index;
			Owner = frame.Get<Stone>(EntityRef).Owner;
		}

		private void Start()
		{
			GameSubscriber.Spawn(index, Owner);
		}
	}
}	