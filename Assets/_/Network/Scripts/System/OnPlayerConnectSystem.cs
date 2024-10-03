using Quantum;
using Redbean.Content;
using UnityEngine;
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
		
		public void OnPlayerDisconnected(Frame f, PlayerRef player)
		{
			Debug.Log("!");
		}

		private void GameStart(Frame frame, QComponentSystem* system)
		{
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
	}
}