using System.Collections.Generic;
using System.Linq;
using Quantum;
using R3;
using Redbean.Network;
using UnityEngine;

namespace Redbean.Content
{
	public class BoardManager : MonoBehaviour
	{
		private static BoardLine[] components;

		private readonly List<DispatcherSubscription> subscriptions = new();
		
		private void Awake()
		{
			GameSubscriber.OnBoardSelect
				.Subscribe(_ =>
				{
					QuantumRunner.DefaultGame.SendCommand(new QCommandTurnEnd
					{
						Entity = NetworkAsset.Stone,
						X = _.X,
						Y = _.Y,
					});
				}).AddTo(this);
			
			components = GetComponentsInChildren<BoardLine>();
			
			var componentArray = components.Select((value, index) => (value, index));
			foreach (var component in componentArray)
				component.value.SetPosition(component.index);
		}

		private void OnEnable()
		{
			var onStoneCreated = QuantumEvent.Subscribe<EventOnStoneCreated>(this, _ =>
			{
				GameSubscriber.SetStoneCreate(new EVT_PositionAndOwner
				{
					OwnerId = _.Stone.OwnerId,
					Position = new EVT_Position
					{
						X = _.Stone.X,
						Y = _.Stone.Y
					}
				});
			});
			subscriptions.Add(onStoneCreated);
			
			var onStoneDestroyed = QuantumEvent.Subscribe<EventOnStoneDestroyed>(this, _ =>
			{
				GameSubscriber.SetStoneDestroy(new EVT_Position
				{
					X = _.Stone.X,
					Y = _.Stone.Y
				});
			});
			subscriptions.Add(onStoneDestroyed);
			
			var onStoneHighlighted = QuantumEvent.Subscribe<EventOnStoneHighlighted>(this, _ =>
			{
				GameSubscriber.SetStoneHighlight(_.Stone);
			});
			subscriptions.Add(onStoneHighlighted);
			
			var onStoneMatchValidation = QuantumEvent.Subscribe<EventOnStoneMatchValidation>(this, _ =>
			{
				if (!IsMatch(_.X, _.Y))
					return;

				if (IsMatch(_.X + 1, _.Y) && IsMatch(_.X + 2, _.Y))
					GameEnd();
				else if (IsMatch(_.X - 1, _.Y) && IsMatch(_.X - 2, _.Y))
					GameEnd();
				else if (IsMatch(_.X, _.Y + 1) && IsMatch(_.X, _.Y + 2))
					GameEnd();
				else if (IsMatch(_.X, _.Y - 1) && IsMatch(_.X, _.Y - 2))
					GameEnd();
				else if (IsMatch(_.X + 1, _.Y + 1) && IsMatch(_.X + 2, _.Y + 2))
					GameEnd();
				else if (IsMatch(_.X - 1, _.Y - 1) && IsMatch(_.X - 2, _.Y - 2))
					GameEnd();
				else if (IsMatch(_.X + 1, _.Y - 1) && IsMatch(_.X + 2, _.Y - 2))
					GameEnd();
				else if (IsMatch(_.X - 1, _.Y + 1) && IsMatch(_.X - 2, _.Y + 2))
					GameEnd();

				// Center Pivot
				else if (IsMatch(_.X + 1, _.Y) && IsMatch(_.X - 1, _.Y))
					GameEnd();
				else if (IsMatch(_.X, _.Y + 1) && IsMatch(_.X, _.Y - 1))
					GameEnd();
				else if (IsMatch(_.X + 1, _.Y + 1) && IsMatch(_.X - 1, _.Y - 1))
					GameEnd();
				else if (IsMatch(_.X - 1, _.Y - 1) && IsMatch(_.X + 1, _.Y + 1))
					GameEnd();
			});
			subscriptions.Add(onStoneMatchValidation);
		}

		private void OnDisable()
		{
			foreach (var subscription in subscriptions)
				QuantumEvent.Unsubscribe(subscription);
		}
		
		private void GameEnd()
		{
			QuantumRunner.DefaultGame.SendCommand(new QCommandGameEnd
			{
				WinnerId = NetworkSetting.LocalPlayerId
			});
		}

		private bool IsMatch(int x, int y)
		{
			if (y < 0 || y >= components.Length)
				return false;
			
			return components[y].GetStone(x) && components[y].GetStone(x).IsOwner;
		}
	}
}