﻿using Quantum;
using R3;
using Redbean.Content;
using UnityEngine.Scripting;

namespace Redbean.Network
{
	[Preserve]
	public unsafe class OnNetworkEventReceiver : SystemSignalsOnly
	{
		private readonly CompositeDisposable disposables = new();

		public override void OnEnabled(Frame f)
		{
			NetworkSubscriber.OnNetworkEvent
				.Where(_ => _.Command.GetType() == typeof(QCommandTurnEnd))
				.Subscribe(_ =>
				{
					OnBoardInteraction(_.Frame, _.Player, _.Command as QCommandTurnEnd);
				}).AddTo(disposables);
			
			NetworkSubscriber.OnNetworkEvent
				.Where(_ => _.Command.GetType() == typeof(QCommandGameEnd))
				.Subscribe(_ =>
				{
					OnGameEnd(_.Frame, _.Player, _.Command as QCommandGameEnd);
				}).AddTo(disposables);
			
			NetworkSubscriber.OnNetworkEvent
				.Where(_ => _.Command.GetType() == typeof(QCommandNextTurn))
				.Subscribe(_ =>
				{
					OnNextTurnSystem(_.Frame, _.Player, _.Command as QCommandNextTurn);
				}).AddTo(disposables);
		}

		public override void OnDisabled(Frame f)
		{
			disposables?.Clear();
			disposables?.Dispose();
		}

#region Event Method

		private void OnBoardInteraction(Frame frame, PlayerRef player, QCommandTurnEnd command)
		{
			frame.Set(frame.Create(command.Entity), new QComponentStone
			{
				X = command.X,
				Y = command.Y,
				OwnerId = frame.PlayerToActorId(player).Value,
				DestroyTurn = frame.GetSingleton<QComponentSystem>().TurnCount + NetworkSetting.StoneDestroyTurn
			});
		}
		
		private void OnGameEnd(Frame frame, PlayerRef player, QCommandGameEnd command)
		{
			GameSubscriber.SetGameStatus(new EVT_GameStatus
			{
				Status = GameStatus.End,
				ActorId = command.WinnerId
			});
		}

		private void OnNextTurnSystem(Frame frame, PlayerRef player, QCommandNextTurn command)
		{
			var currentTurn = frame.Unsafe.GetPointerSingleton<QComponentSystem>()->NextTurn();
			
			var stones = frame.Filter<QComponentStone>();
			while (stones.Next(out var entity, out var stone))
			{
				switch (stone.DestroyTurn - currentTurn)
				{
					case 1:
					{
						GameSubscriber.SetStoneHighlight(stone);
						break;
					}

					case <= 0:
					{
						frame.Signals.OnRemoveStone(entity);
						break;
					}
				}
			}
		}

#endregion
	}
}