using Quantum;
using R3;
using UnityEngine.Scripting;

namespace Redbean.Network
{
	[Preserve]
	public unsafe class OnPlayerConnectSystem : SystemSignalsOnly, ISignalOnPlayerConnected, ISignalOnPlayerDisconnected, ISignalOnGameStart
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
			var asset = frame.FindAsset(NetworkAsset.Player);
			var entity = frame.Create(asset);

			frame.Set(entity, new QComponentPlayer
			{
				Player = player
			});

			frame.Signals.OnGameStart();
		}
		
		public void OnPlayerDisconnected(Frame frame, PlayerRef player)
		{
			var filter = frame.Filter<QComponentPlayer>();
			while (filter.Next(out var entity, out var localPlayer))
			{
				if (frame.PlayerToActorId(localPlayer.Player) == frame.PlayerToActorId(player))
					frame.Destroy(entity);
			}
			
			frame.Signals.OnGameStart();
		}
		
		public void OnGameStart(Frame frame)
		{
			var system = GameReset(frame);
			if (frame.PlayerConnectedCount > 1)
			{
				var random = frame.RNG->Next(0, frame.PlayerConnectedCount);
				var players = frame.AllocateList<PlayerRef>();
				var retryPlayers = frame.AllocateList<int>();

				var filter = frame.Filter<QComponentPlayer>();
				while (filter.Next(out _, out var player))
					players.Add(player.Player);
				
				system->Players = players;
				system->RetryPlayers = retryPlayers;
				system->CurrentPlayerTurn = frame.PlayerToActorId(players[random]).Value;

				GameSubscriber.SetGameStatus(new EVT_GameStatus
				{
					Status = GameStatus.Start,
					ActorId = frame.PlayerToActorId(players[random]).Value
				});
			}
			else
			{
				GameSubscriber.SetGameStatus(new EVT_GameStatus
				{
					Status = GameStatus.Wait
				});
			}
		}

		private QComponentSystem* GameReset(Frame frame)
		{
			var system = frame.Unsafe.GetPointerSingleton<QComponentSystem>();
			system->Players = default;
			system->RetryPlayers = default;
			system->CurrentPlayerTurn = default;
			system->CurrentTurn = default;
			
			var filter = frame.Filter<QComponentStone>();
			while (filter.Next(out var entity, out var stone))
			{
				frame.Events.OnStoneDestroyed(stone);
				frame.Destroy(entity);
			}
			
			GameSubscriber.SetGameStatus(new EVT_GameStatus
			{
				Status = GameStatus.Reset
			});

			return system;
		}
	}
}