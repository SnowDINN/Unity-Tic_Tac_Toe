using Quantum;
using Redbean.Content;
using UnityEngine.Scripting;

namespace Redbean.Network
{
	[Preserve]
	public unsafe class OnNetworkEventPublisher : SystemMainThreadFilter<OnNetworkEventPublisher.Filter>
	{
		public struct Filter
		{
			public EntityRef Entity;
			public LocalPlayer* LocalPlayer;
		}

		public override void Update(Frame frame, ref Filter filter)
		{
			if (frame.GetPlayerCommand(filter.LocalPlayer->Player) is QCommandStoneCreate qCommandStoneCreate)
				NetworkSubscriber.Publish(new NetworkEventStream
				{
					Frame = frame,
					Player = filter.LocalPlayer->Player,
					Command = qCommandStoneCreate
				});
			
			if (frame.GetPlayerCommand(filter.LocalPlayer->Player) is QCommandStoneDestroy qCommandStoneDestroy)
				NetworkSubscriber.Publish(new NetworkEventStream
				{
					Frame = frame,
					Player = filter.LocalPlayer->Player,
					Command = qCommandStoneDestroy
				});
			
			if (frame.GetPlayerCommand(filter.LocalPlayer->Player) is QCommandStoneMatch qCommandStoneMatch)
				NetworkSubscriber.Publish(new NetworkEventStream
				{
					Frame = frame,
					Player = filter.LocalPlayer->Player,
					Command = qCommandStoneMatch
				});
			
			if (frame.GetPlayerCommand(filter.LocalPlayer->Player) is QCommandNextTurn qCommandNextTurn)
				NetworkSubscriber.Publish(new NetworkEventStream
				{
					Frame = frame,
					Player = filter.LocalPlayer->Player,
					Command = qCommandNextTurn
				});
		}
	}
}