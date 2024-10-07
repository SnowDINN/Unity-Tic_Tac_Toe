using System.Linq;
using Quantum;
using R3;
using Redbean.Network;
using UnityEngine;

namespace Redbean.Content
{
	public class GameLogicListener : MonoBehaviour
	{
		private static BoardLine[] components;
		
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

			GameSubscriber.OnStoneMatchValidation
				.Subscribe(_ =>
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
				}).AddTo(this);
			
			components = GetComponentsInChildren<BoardLine>();
			
			var componentArray = components.Select((value, index) => (value, index));
			foreach (var component in componentArray)
				component.value.SetPosition(component.index);
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