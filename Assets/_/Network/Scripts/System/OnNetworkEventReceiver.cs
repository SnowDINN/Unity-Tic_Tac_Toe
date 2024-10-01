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
				.Where(_ => _.Command.GetType() == typeof(QCommandStoneCreate))
				.Subscribe(_ =>
				{
					OnStoneCreateSystem(_.Frame, _.Player, _.Command as QCommandStoneCreate);
				}).AddTo(disposables);
			
			NetworkSubscriber.OnNetworkEvent
				.Where(_ => _.Command.GetType() == typeof(QCommandStoneDestroy))
				.Subscribe(_ =>
				{
					OnStoneDestroySystem(_.Frame, _.Player, _.Command as QCommandStoneDestroy);
				}).AddTo(disposables);
			
			NetworkSubscriber.OnNetworkEvent
				.Where(_ => _.Command.GetType() == typeof(QCommandStoneMatch))
				.Subscribe(_ =>
				{
					OnStoneMatchSystem(_.Frame, _.Player, _.Command as QCommandStoneMatch);
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

		private void OnStoneCreateSystem(Frame frame, PlayerRef player, QCommandStoneCreate command)
		{
			frame.Set(frame.Create(command.Entity), new Stone
			{
				X = command.X,
				Y = command.Y,
				OwnerId = frame.PlayerToActorId(player).Value,
				DestroyTurn = frame.GetSingleton<Game>().TurnCount + NetworkSetting.StoneDestroyTurn
			});
		}
		
		private void OnStoneDestroySystem(Frame frame, PlayerRef player, QCommandStoneDestroy command)
		{
			frame.Destroy(command.Entity);
		}
		
		private void OnStoneMatchSystem(Frame frame, PlayerRef player, QCommandStoneMatch command)
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
						if (QuantumRunner.Default.NetworkClient.LocalPlayer.IsMasterClient)
							QuantumRunner.DefaultGame.SendCommand(new QCommandStoneDestroy
							{
								Entity = entity
							});

						break;
					}
				}
			}
		}

#endregion
	}
}