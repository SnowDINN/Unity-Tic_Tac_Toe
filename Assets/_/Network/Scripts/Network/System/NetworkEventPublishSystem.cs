using Quantum;
using UnityEngine.Scripting;

namespace Redbean.Network
{
	[Preserve]
	public unsafe class NetworkEventPublishSystem : SystemMainThreadFilter<NetworkEventPublishSystem.Filter>
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
				case QCommandGameStatus:
				case QCommandGameNextTurn:
				case QCommandGameVote:
					frame.Signals.OnEvent(new QEventCommand
					{
						Player = filter.LocalPlayer->Player,
						Command = command
					});
					break;
			}
		}
	}
}