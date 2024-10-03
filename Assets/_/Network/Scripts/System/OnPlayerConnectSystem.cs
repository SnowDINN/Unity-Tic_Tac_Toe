using Quantum;
using Redbean.Content;
using UnityEngine.Scripting;

namespace Redbean.Network
{
	[Preserve]
	public unsafe class OnPlayerConnectSystem : SystemSignalsOnly, ISignalOnPlayerConnected, ISignalOnPlayerDisconnected
	{
		public override void OnInit(Frame frame)
		{
			frame.SetSingleton(new QComponentSystem());
		}

		public void OnPlayerConnected(Frame frame, PlayerRef player)
		{
			var asset = frame.FindAsset(NetworkAsset.Player);
			var entity = frame.Create(asset);

			frame.Set(entity, new QComponentPlayer
			{
				Player = player
			});

			GameStart(frame, frame.Unsafe.GetPointerSingleton<QComponentSystem>());
		}
		
		public void OnPlayerDisconnected(Frame frame, PlayerRef player)
		{
			var filter = frame.Filter<QComponentPlayer>();
			while (filter.Next(out var entity, out var localPlayer))
			{
				if (frame.PlayerToActorId(localPlayer.Player) == frame.PlayerToActorId(player))
					frame.Destroy(entity);
			}
			
			GameStart(frame, frame.Unsafe.GetPointerSingleton<QComponentSystem>());
		}

		private void GameStart(Frame frame, QComponentSystem* system)
		{
			GameReset(frame, system);
			
			if (frame.PlayerConnectedCount > 1)
			{
				var random = frame.RNG->Next(0, frame.PlayerConnectedCount + 1);
				var players = frame.AllocateList<PlayerRef>();

				var filter = frame.Filter<QComponentPlayer>();
				while (filter.Next(out _, out var player))
					players.Add(player.Player);

				system->CurrentPlayers = players;
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

		private void GameReset(Frame frame, QComponentSystem* system)
		{
			system->CurrentPlayers = default;
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
				Status = GameStatus.Restart
			});
		}
	}
}