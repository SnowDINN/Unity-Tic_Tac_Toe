using System.Collections.Generic;
using System.Linq;
using Quantum;
using R3;
using UnityEngine;

namespace Redbean.Game
{
	public class BoardContainer : MonoBehaviour
	{
		[HideInInspector] public BoardBundle[] Bundles;
		
		private void Awake()
		{
			RxGame.OnMatchValidation
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

			RxGame.OnGameTimeout
				.Subscribe(_ =>
				{
					if (!GameManager.IsMyTurn)
						return;

					var emptyBoardUnits = FindEmptyBoardUnits();
					var randomBoardUnit = emptyBoardUnits[Random.Range(0, emptyBoardUnits.Length)];
						
					this.NetworkEventPublish(new QCommandGameNextTurn
					{
						X = randomBoardUnit.X,
						Y = randomBoardUnit.Y
					});
				}).AddTo(this);
			
			Bundles = GetComponentsInChildren<BoardBundle>();
			
			var componentArray = Bundles.Select((value, index) => (value, index));
			foreach (var component in componentArray)
				component.value.SetPosition(component.index);
		}
		
		private void GameEnd()
		{
			this.NetworkEventPublish(new QCommandGameResult
			{
				WinnerPlayer = this.GetActorId()
			});
		}

		private bool IsMatch(int x, int y)
		{
			if (y < 0 || y >= Bundles.Length)
				return false;
			
			return Bundles[y].GetStone(x) && Bundles[y].GetStone(x).IsOwner;
		}

		private BoardUnit[] FindEmptyBoardUnits() =>
			(from bundle in Bundles from unit in bundle.Units where !unit.HasStone select unit).ToArray();
	}
}