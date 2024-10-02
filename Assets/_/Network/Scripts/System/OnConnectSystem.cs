using System.Collections.Generic;
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
				Player = player,
			});

			GameStart(frame);
			
			Debug.Log($"[{frame.GetPlayerData(player).PlayerNickname}] Connect | Actor ID : {frame.PlayerToActorId(player)}");
			Debug.Log($"Player Connected Count : {frame.PlayerConnectedCount}");
		}

		private void GameStart(Frame frame)
		{
			if (frame.PlayerConnectedCount > 0)
			{
				var random = frame.RNG->Next(0, frame.PlayerConnectedCount + 1);
				var players = new List<QComponentPlayer>();

				var filter = frame.Filter<QComponentPlayer>();
				while (filter.Next(out _, out var player))
					players.Add(player);

				GameSubscriber.SetGameStatus(new EVT_GameStatus
				{
					Status = GameStatus.Start,
					ActorId = frame.PlayerToActorId(players[random].Player).Value
				});
			}
			else
				GameSubscriber.SetGameStatus(new EVT_GameStatus
				{
					Status = GameStatus.Wait
				});
		}
	}
}