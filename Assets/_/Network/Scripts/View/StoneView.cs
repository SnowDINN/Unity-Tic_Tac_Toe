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
			
			GameSubscriber.SetStoneCreate(new EVT_PositionAndOwner
			{
				OwnerId = ownerId,
				Position = new EVT_Position
				{
					X = x,
					Y = y
				}
			});

			frame.Signals.OnMatchValidation(x, y);
		}

		public override void OnDeactivate()
		{
			GameSubscriber.SetStoneDestroy(new EVT_Position
			{
				X = x,
				Y = y
			});
		}
	}
}	