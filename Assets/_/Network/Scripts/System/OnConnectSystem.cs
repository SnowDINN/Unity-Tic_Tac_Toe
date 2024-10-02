using Quantum;
using Redbean.Content;
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
		}

		private void GameStart(Frame frame)
		{
			if (frame.PlayerConnectedCount > 0)
			{
				var random = frame.RNG->Next(0, frame.PlayerConnectedCount);
				var players = QuantumRunner.DefaultGame.GetLocalPlayers();

				GameSubscriber.SetGameStatus(new EVT_GameStatus
				{
					Status = GameStatus.Start,
					ActorId = frame.PlayerToActorId(players[random]).Value
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