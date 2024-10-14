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
			frame.SessionReset();
			
			var entity = frame.Create();
			var qPlayer = new QComponentPlayer
			{
				Player = player
			};
			frame.Add(entity, qPlayer);
			
			if (frame.PlayerConnectedCount < frame.PlayerCount)
				return;
			
			frame.Signals.OnGameVote(new QEventGameVote
			{
				Type = GameVote.Ready
			});
		}
		
		public void OnPlayerDisconnected(Frame frame, PlayerRef player)
		{
			frame.SessionReset();
			
			var qPlayerFilter = frame.Filter<QComponentPlayer>();
			while (qPlayerFilter.Next(out var entity, out var qPlayer))
			{
				if (frame.PlayerToActorId(qPlayer.Player) == frame.PlayerToActorId(player))
					frame.Destroy(entity);
			}
			
			frame.Signals.OnGameVote(new QEventGameVote
			{
				Type = GameVote.Ready
			});
		}
	}
}