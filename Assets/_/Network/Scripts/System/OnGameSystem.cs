using Quantum;
using UnityEngine.Scripting;

namespace Redbean.Network
{
	[Preserve]
	public unsafe class OnGameSystem : SystemSignalsOnly, ISignalOnGameStatus
	{
		public override void OnInit(Frame frame)
		{
			ResetSystemComponent(frame);
		}

		public void OnGameStatus(Frame frame, int type)
		{
			switch ((GameStatus)type)
			{
				case GameStatus.Start:
				{
					ResetSystemComponent(frame);
					
					if (frame.PlayerConnectedCount < frame.PlayerCount)
						return;
					
					var random = frame.RNG->Next(0, frame.PlayerConnectedCount);
					var qPlayers = frame.AllocateList<PlayerRef>();

					var qPlayersFilter = frame.Filter<QComponentPlayer>();
					while (qPlayersFilter.Next(out _, out var qPlayer))
						qPlayers.Add(qPlayer.Player);

					var nextTurn = frame.PlayerToActorId(qPlayers[random]).Value;
					var qSystem = frame.Unsafe.GetPointerSingleton<QComponentSystem>();
					qSystem->Players = qPlayers;
					qSystem->ReadyPlayers = frame.AllocateList<int>();
					qSystem->RetryPlayers = frame.AllocateList<int>();
					qSystem->CurrentPlayerTurn = frame.PlayerToActorId(qPlayers[random]).Value;

					frame.Events.OnGameStatus((int)GameStatus.Start, default);
					frame.Events.OnGameStatus((int)GameStatus.Next, nextTurn);
					break;
				}

				case GameStatus.Ready:
				{
					ResetSystemComponent(frame);
					
					frame.MapAssetRef = NetworkAsset.Map;
					frame.Events.OnGameStatus((int)GameStatus.Ready, default);
					break;
				}
				
				case GameStatus.Retry:
				{
					var qSystem = frame.Unsafe.GetPointerSingleton<QComponentSystem>();
					GameSubscriber.SetGameRetry(new EVT_GameRetry
					{
						RequestRetryCount = frame.ResolveList(qSystem->RetryPlayers).Count,
						RequireRetryCount = frame.PlayerConnectedCount
					});
					break;
				}
			}
		}

		private void ResetSystemComponent(Frame frame)
		{
			// Game Data Reset
			var qSystem = frame.Unsafe.GetPointerSingleton<QComponentSystem>();
			qSystem->Players = frame.AllocateList<PlayerRef>();
			qSystem->ReadyPlayers = frame.AllocateList<int>();
			qSystem->RetryPlayers = frame.AllocateList<int>();
			qSystem->CurrentPlayerTurn = default;
			qSystem->CurrentTurn = default;
			
			var qStone = frame.Filter<QComponentStone>();
			while (qStone.Next(out var entity, out var stone))
			{
				frame.Events.OnStoneDestroyed(stone);
				frame.Destroy(entity);
			}
		}
	}
}