using System.Linq;
using Quantum;
using R3;
using Redbean.Content;
using UnityEngine.Scripting;

namespace Redbean.Network
{
	[Preserve]
	public unsafe class OnNetworkEventReceiver : SystemSignalsOnly
	{
		private readonly CompositeDisposable disposables = new();

		public override void OnEnabled(Frame frame)
		{
			NetworkSubscriber.OnNetworkEvent
				.Where(_ => _.Command.GetType() == typeof(QCommandGameEnd))
				.Subscribe(_ =>
				{
					OnGameEnd(_.Frame, _.Player, _.Command as QCommandGameEnd);
				}).AddTo(disposables);
			
			NetworkSubscriber.OnNetworkEvent
				.Where(_ => _.Command.GetType() == typeof(QCommandTurnEnd))
				.Subscribe(_ =>
				{
					OnTurnEnd(_.Frame, _.Player, _.Command as QCommandTurnEnd);
				}).AddTo(disposables);
		}

		public override void OnDisabled(Frame frame)
		{
			disposables?.Clear();
			disposables?.Dispose();
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

		private void OnTurnEnd(Frame frame, PlayerRef player, QCommandTurnEnd command)
		{
			var system = frame.Unsafe.GetPointerSingleton<QComponentSystem>();
			var nextPlayer = frame.ResolveList(system->CurrentPlayers)
				.FirstOrDefault(_ => frame.PlayerToActorId(_).Value != system->CurrentPlayerTurn);
			system->CurrentPlayerTurn = frame.PlayerToActorId(nextPlayer).Value;
			system->TurnCount += 1;

			var createdEntity = frame.Create(command.Entity);
			var createdStone = new QComponentStone
			{
				X = command.X,
				Y = command.Y,
				OwnerId = frame.PlayerToActorId(player).Value,
				DestroyTurn = system->TurnCount + NetworkSetting.StoneDestroyTurn
			};
			frame.Set(createdEntity, createdStone);
			frame.Events.OnStoneCreated(createdStone);
			
			var destroyedStones = frame.Filter<QComponentStone>();
			while (destroyedStones.Next(out var destroyedEntity, out var destroyedStone))
			{
				switch (destroyedStone.DestroyTurn - system->TurnCount)
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