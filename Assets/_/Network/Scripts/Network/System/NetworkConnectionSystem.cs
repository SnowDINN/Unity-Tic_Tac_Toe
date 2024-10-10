using Quantum;
using UnityEngine.Scripting;

namespace Redbean.Network
{
	[Preserve]
	public class NetworkConnectionSystem : SystemSignalsOnly, ISignalOnPlayerConnected, ISignalOnPlayerDisconnected
	{
		public override void OnInit(Frame frame)
		{
			frame.SetSingleton(new QComponentSystem());
		}

		public void OnPlayerConnected(Frame frame, PlayerRef player)
		{
			var entity = frame.Create();
			var qPlayer = new QComponentPlayer
			{
				Player = player
			};
			frame.Add(entity, qPlayer);
			
			if (frame.PlayerConnectedCount < frame.PlayerCount)
				return;
			
			frame.Signals.OnGameStatus(new QEventGameStatus
			{
				Type = GameStatus.Ready
			});
		}
		
		public void OnPlayerDisconnected(Frame frame, PlayerRef player)
		{
			var qPlayerFilter = frame.Filter<QComponentPlayer>();
			while (qPlayerFilter.Next(out var entity, out var qPlayer))
			{
				if (frame.PlayerToActorId(qPlayer.Player) == frame.PlayerToActorId(player))
					frame.Destroy(entity);
			}
			
			frame.Signals.OnGameStatus(new QEventGameStatus
			{
				Type = GameStatus.Ready
			});
		}
	}
}