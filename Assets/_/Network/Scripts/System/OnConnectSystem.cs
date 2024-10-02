using Quantum;
using Redbean.Content;
using UnityEngine.Scripting;

namespace Redbean.Network
{
	[Preserve]
	public unsafe class OnConnectSystem : SystemSignalsOnly, ISignalOnPlayerAdded
	{
		public override void OnInit(Frame frame)
		{
			frame.SetSingleton(new QComponentSystem());
		}

		public void OnPlayerAdded(Frame frame, PlayerRef player, bool firstTime)
		{
			var asset = frame.FindAsset(NetworkAsset.Player);
			var entity = frame.Create(asset);

			frame.Set(entity, new QComponentPlayer
			{
				Player = player
			});

			GameStart(frame, frame.Unsafe.GetPointerSingleton<QComponentSystem>());
		}

		private void GameStart(Frame frame, QComponentSystem* system)
		{
			if (frame.PlayerConnectedCount > 0)
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