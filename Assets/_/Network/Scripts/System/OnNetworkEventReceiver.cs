using Quantum;
using R3;
using Redbean.Content;
using UnityEngine;
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
				.Where(_ => _.Command.GetType() == typeof(QCommandBoardInteraction))
				.Subscribe(_ =>
				{
					OnBoardInteraction(_.Frame, _.Player, _.Command as QCommandBoardInteraction);
				}).AddTo(disposables);
			
			NetworkSubscriber.OnNetworkEvent
				.Where(_ => _.Command.GetType() == typeof(QCommandBoardMatch))
				.Subscribe(_ =>
				{
					OnBoardMatchSystem(_.Frame, _.Player, _.Command as QCommandBoardMatch);
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

		private void OnBoardInteraction(Frame frame, PlayerRef player, QCommandBoardInteraction command)
		{
			frame.Set(frame.Create(command.Entity), new Stone
			{
				X = command.X,
				Y = command.Y,
				OwnerId = frame.PlayerToActorId(player).Value,
				DestroyTurn = frame.GetSingleton<Game>().TurnCount + NetworkSetting.StoneDestroyTurn
			});
		}
		
		private void OnBoardMatchSystem(Frame frame, PlayerRef player, QCommandBoardMatch command)
		{
			Debug.Log($"[ {frame.GetPlayerData(player).PlayerNickname} ] Match !!");
		}

		private void OnNextTurnSystem(Frame frame, PlayerRef player, QCommandNextTurn command)
		{
			var currentTurn = frame.Unsafe.GetPointerSingleton<Game>()->NextTurn();
			
			var stones = frame.Filter<Stone>();
			while (stones.Next(out var entity, out var stone))
			{
				switch (stone.DestroyTurn - currentTurn)
				{
					case 1:
					{
						frame.Events.StoneHighlight(stone);
						break;
					}

					case <= 0:
					{
						frame.Signals.OnStoneDestroy(entity);
						break;
					}
				}
			}
		}

#endregion
	}
}