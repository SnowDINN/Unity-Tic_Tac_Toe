using System.Linq;
using Photon.Deterministic;
using Quantum;
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
				
				case QCommandGameVote qCommand:
					OnGameVote(frame, player, qCommand);
					break;

				case QCommandTurnEnd qCommand:
					OnTurnEnd(frame, player, qCommand);
					break;
			}
		}
		
#region Event Method
		
		private void OnGameEnd(Frame frame, PlayerRef player, QCommandGameEnd command)
		{
			frame.Events.OnGameStatus((int)GameStatus.End, command.WinnerId);
		}

		private void OnGameVote(Frame frame, PlayerRef player, QCommandGameVote command)
		{
			switch ((GameVote)command.VoteType)
			{
				case GameVote.Ready:
				{
					var qSystem = frame.Unsafe.GetPointerSingleton<QComponentSystem>();
					var readyPlayers = frame.ResolveList(qSystem->ReadyPlayers);
					if (!readyPlayers.Contains(command.ActorId))
						readyPlayers.Add(command.ActorId);
					
					qSystem->ReadyPlayers = readyPlayers;

					if (frame.PlayerConnectedCount == readyPlayers.Count)
						frame.Signals.OnGameStatus((int)GameStatus.Start);
					
					break;
				}

				case GameVote.Retry:
				{
					var qSystem = frame.Unsafe.GetPointerSingleton<QComponentSystem>();
					var retryPlayers = frame.ResolveList(qSystem->RetryPlayers);
					if (!retryPlayers.Contains(command.ActorId))
						retryPlayers.Add(command.ActorId);

					qSystem->RetryPlayers = retryPlayers;

					if (frame.PlayerConnectedCount == retryPlayers.Count)
						frame.Signals.OnGameStatus((int)GameStatus.Start);
					else
						frame.Signals.OnGameStatus((int)GameStatus.Retry);

					break;
				}
			}
		}

		private void OnTurnEnd(Frame frame, PlayerRef player, QCommandTurnEnd command)
		{
			var qSystem = frame.Unsafe.GetPointerSingleton<QComponentSystem>();
			var nextPlayer = frame.ResolveList(qSystem->Players)
				.FirstOrDefault(_ => frame.PlayerToActorId(_).Value != qSystem->CurrentPlayerTurn);
			qSystem->CurrentPlayerTurn = frame.PlayerToActorId(nextPlayer).Value;
			qSystem->CurrentTurn += 1;

			var entity = frame.Create(command.Entity);
			var qStone = new QComponentStone
			{
				X = command.X,
				Y = command.Y,
				OwnerId = frame.PlayerToActorId(player).Value,
				DestroyTurn = qSystem->CurrentTurn + NetworkSetting.StoneDestroyTurn
			};
			frame.Set(entity, qStone);
			frame.Events.OnStoneCreated(qStone);
			
			var qStoneFilter = frame.Filter<QComponentStone>();
			while (qStoneFilter.Next(out var destroyedEntity, out var destroyedStone))
			{
				switch (destroyedStone.DestroyTurn - qSystem->CurrentTurn)
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
			frame.Events.OnGameStatus((int)GameStatus.Next, qSystem->CurrentPlayerTurn);
		}

#endregion
	}
}