using Quantum;
using Redbean.Content;
using UnityEngine;
using UnityEngine.Scripting;

namespace Redbean.Network
{
	[Preserve]
	public unsafe class OnConnectSystem : SystemSignalsOnly, ISignalOnPlayerAdded
	{
		public override void OnInit(Frame f)
		{
			f.SetSingleton(new QComponentSystem());
		}

		public void OnPlayerAdded(Frame frame, PlayerRef player, bool firstTime)
		{
			var asset = frame.FindAsset(NetworkAsset.Player);
			var entity = frame.Create(asset);

			frame.Set(entity, new QComponentPlayer
			{
				Player = player
			});
			
			Debug.Log($"[{frame.GetPlayerData(player).PlayerNickname}] Connect | Actor ID : {frame.PlayerToActorId(player)}");
			Debug.Log($"Player Connected Count : {frame.PlayerConnectedCount}");

			GameStart(frame, frame.Unsafe.GetPointerSingleton<QComponentSystem>());
		}

		private void GameStart(Frame frame, QComponentSystem* system)
		{
			if (frame.PlayerConnectedCount > 0)
			{
				if (system->CurrentGameStatus != (int)GameStatus.Wait)
					return;
				
				var random = frame.RNG->Next(0, frame.PlayerConnectedCount + 1);
				var players = frame.AllocateList<PlayerRef>();

				var filter = frame.Filter<QComponentPlayer>();
				while (filter.Next(out _, out var player))
					players.Add(player.Player);

				system->CurrentPlayers = players;
				system->CurrentPlayerTurn = frame.PlayerToActorId(players[random]).Value;
				system->CurrentGameStatus = (int)GameStatus.Start;

				GameSubscriber.SetGameStatus(new EVT_GameStatus
				{
					Status = GameStatus.Start,
					ActorId = frame.PlayerToActorId(players[random]).Value
				});
			}
			else
			{
				system->CurrentGameStatus = (int)GameStatus.Wait;
				
				GameSubscriber.SetGameStatus(new EVT_GameStatus
				{
					Status = GameStatus.Wait
				});
			}
		}
	}
}