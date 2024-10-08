using Quantum;
using UnityEngine.Scripting;

namespace Redbean.Network
{
	[Preserve]
	public unsafe class OnNetworkEventPublisher : SystemMainThreadFilter<OnNetworkEventPublisher.Filter>
	{
		public struct Filter
		{
			public EntityRef Entity;
			public QComponentPlayer* LocalPlayer;
		}

		public override void Update(Frame frame, ref Filter filter)
		{
			var command = frame.GetPlayerCommand(filter.LocalPlayer->Player);
			switch (command)
			{
				case QCommandTurnEnd:
				case QCommandGameEnd:
				case QCommandGameVote:
					frame.Signals.OnEventReceive(filter.LocalPlayer->Player, command);
					break;
			}
		}
	}
}