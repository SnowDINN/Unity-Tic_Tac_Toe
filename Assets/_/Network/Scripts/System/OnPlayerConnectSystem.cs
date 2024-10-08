using Quantum;
using R3;
using UnityEngine.Scripting;

namespace Redbean.Network
{
	[Preserve]
	public class OnPlayerConnectSystem : SystemSignalsOnly, ISignalOnPlayerConnected, ISignalOnPlayerDisconnected
	{
		private readonly CompositeDisposable disposables = new();
		
		public override void OnInit(Frame frame)
		{
			frame.SetSingleton(new QComponentSystem());

			LobbySubscriber.OnDisconnect
				.Where(_ => _.Status == ConnectStatus.Before)
				.Subscribe(_ =>
				{
					foreach (var system in frame.SystemsAll)
						frame.SystemDisable(system);
					
					disposables?.Clear();
					disposables?.Dispose();
				}).AddTo(disposables);
		}

		public void OnPlayerConnected(Frame frame, PlayerRef player)
		{
			var entity = frame.Create();
			var component = new QComponentPlayer
			{
				Player = player
			};
			frame.Add(entity, component);
			
			if (frame.PlayerConnectedCount < frame.PlayerCount)
				return;
			
			frame.Signals.OnGameStatus((int)GameStatus.Ready);
		}
		
		public void OnPlayerDisconnected(Frame frame, PlayerRef player)
		{
			var filter = frame.Filter<QComponentPlayer>();
			while (filter.Next(out var entity, out var localPlayer))
			{
				if (frame.PlayerToActorId(localPlayer.Player) == frame.PlayerToActorId(player))
					frame.Destroy(entity);
			}
			
			frame.Signals.OnGameStatus((int)GameStatus.Ready);
		}
	}
}