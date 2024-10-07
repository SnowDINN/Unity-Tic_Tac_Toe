using System.Linq;
using Photon.Deterministic;
using Quantum;
using UnityEngine;
using UnityEngine.Scripting;

namespace Redbean.Network
{
	[Preserve]
	public unsafe class OnNetworkEventReceiver : SystemSignalsOnly, ISignalOnEventReceive
	{
		public void OnEventReceive(Frame frame, PlayerRef player, DeterministicCommand command)
		{
			switch (command)
			{
				case QCommandGameEnd qCommand:
					OnGameEnd(frame, player, qCommand);
					break;
				
				case QCommandGameRetry qCommand:
					OnGameRetry(frame, player, qCommand);
					break;

				case QCommandTurnEnd qCommand:
					OnTurnEnd(frame, player, qCommand);
					break;
			}
		}
		
#region Event Method
		
		private void OnGameEnd(Frame frame, PlayerRef player, QCommandGameEnd command)
		{
			GameSubscriber.SetGameStatus(new EVT_GameStatus
			{
				Status = GameStatus.End,
				ActorId = command.WinnerId
			});
		}

		private void OnGameRetry(Frame frame, PlayerRef player, QCommandGameRetry command)
		{
			var system = frame.Unsafe.GetPointerSingleton<QComponentSystem>();
			var retryPlayers = frame.ResolveList(system->RetryPlayers);
			if (!retryPlayers.Contains(command.ActorId))
				retryPlayers.Add(command.ActorId);

			system->RetryPlayers = retryPlayers;

			if (frame.PlayerConnectedCount == retryPlayers.Count)
				frame.Signals.OnGameStart();
			
			GameSubscriber.SetGameRetry(new EVT_GameRetry
			{
				RequestRetryCount = retryPlayers.Count,
				RequireRetryCount = frame.PlayerConnectedCount
			});
		}

		private void OnTurnEnd(Frame frame, PlayerRef player, QCommandTurnEnd command)
		{
			var system = frame.Unsafe.GetPointerSingleton<QComponentSystem>();
			var nextPlayer = frame.ResolveList(system->Players)
				.FirstOrDefault(_ => frame.PlayerToActorId(_).Value != system->CurrentPlayerTurn);
			system->CurrentPlayerTurn = frame.PlayerToActorId(nextPlayer).Value;
			system->CurrentTurn += 1;

			var createdEntity = frame.Create(command.Entity);
			var createdStone = new QComponentStone
			{
				X = command.X,
				Y = command.Y,
				OwnerId = frame.PlayerToActorId(player).Value,
				DestroyTurn = system->CurrentTurn + NetworkSetting.StoneDestroyTurn
			};
			frame.Set(createdEntity, createdStone);
			frame.Events.OnStoneCreated(createdStone);
			
			var filter = frame.Filter<QComponentStone>();
			while (filter.Next(out var destroyedEntity, out var destroyedStone))
			{
				switch (destroyedStone.DestroyTurn - system->CurrentTurn)
				{
					case 1:
						frame.Events.OnStoneHighlighted(destroyedStone);
						break;

					case <= 0:
						frame.Events.OnStoneDestroyed(destroyedStone);
						frame.Destroy(destroyedEntity);
						break;
				}
			}
			frame.Events.OnStoneMatchValidation(command.X, command.Y);
			
			GameSubscriber.SetGameStatus(new EVT_GameStatus
			{
				Status = GameStatus.Start,
				ActorId = system->CurrentPlayerTurn
			});
		}

#endregion
	}
}