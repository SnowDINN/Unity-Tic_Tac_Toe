using Quantum;
using Redbean.Content;

namespace Redbean.Network
{
	public class StoneView : QuantumEntityViewComponent
	{
		private int x;
		private int y;
		private int ownerId;

		public override void OnActivate(Frame frame)
		{
			x = frame.Get<QComponentStone>(EntityRef).X;
			y = frame.Get<QComponentStone>(EntityRef).Y;
			ownerId = frame.Get<QComponentStone>(EntityRef).OwnerId;
			
			GameSubscriber.StoneCreate(new StoneCreateStream
			{
				OwnerId = ownerId,
				X = x,
				Y = y
			});

			frame.Signals.OnMatchValidation(x, y);
		}

		public override void OnDeactivate()
		{
			GameSubscriber.StoneDestroy(new StoneDestroyStream
			{
				X = x,
				Y = y
			});
		}
	}
}	