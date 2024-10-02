using System.Linq;
using Quantum;
using Redbean.Content;
using UnityEngine;
using UnityEngine.Scripting;

namespace Redbean.Network
{
	[Preserve]
	public class OnMatchValidationSystem : SystemSignalsOnly, ISignalOnMatchValidation, ISignalOnRemoveStone
	{
		public void OnMatchValidation(Frame f, int x, int y)
		{
			if (!BoardManager.IsOwner(x, y))
				return;

			if (BoardManager.IsOwner(x + 1, y) && BoardManager.IsOwner(x + 2, y))
			{
				MatchSuccess();
				return;
			}

			if (BoardManager.IsOwner(x - 1, y) && BoardManager.IsOwner(x - 2, y))
			{
				MatchSuccess();
				return;
			}

			if (BoardManager.IsOwner(x, y + 1) && BoardManager.IsOwner(x, y + 2))
			{
				MatchSuccess();
				return;
			}

			if (BoardManager.IsOwner(x, y - 1) && BoardManager.IsOwner(x, y - 2))
			{
				MatchSuccess();
				return;
			}

			if (BoardManager.IsOwner(x + 1, y + 1) && BoardManager.IsOwner(x + 2, y + 2))
			{
				MatchSuccess();
				return;
			}

			if (BoardManager.IsOwner(x - 1, y - 1) && BoardManager.IsOwner(x - 2, y - 2))
			{
				MatchSuccess();
				return;
			}

			if (BoardManager.IsOwner(x + 1, y - 1) && BoardManager.IsOwner(x + 2, y - 2))
			{
				MatchSuccess();
				return;
			}

			if (BoardManager.IsOwner(x - 1, y + 1) && BoardManager.IsOwner(x - 2, y + 2))
			{
				MatchSuccess();
				return;
			}

			// Center Pivot
			if (BoardManager.IsOwner(x + 1, y) && BoardManager.IsOwner(x - 1, y))
			{
				MatchSuccess();
				return;
			}

			if (BoardManager.IsOwner(x, y + 1) && BoardManager.IsOwner(x, y - 1))
			{
				MatchSuccess();
				return;
			}

			if (BoardManager.IsOwner(x + 1, y + 1) && BoardManager.IsOwner(x - 1, y - 1))
			{
				MatchSuccess();
				return;
			}

			if (BoardManager.IsOwner(x - 1, y - 1) && BoardManager.IsOwner(x + 1, y + 1))
			{
				MatchSuccess();
				return;
			}

			var system = f.GetSingleton<QComponentSystem>();
			var otherTurn = f.ResolveList(system.CurrentPlayers)
				.FirstOrDefault(_ => f.PlayerToActorId(_).Value != system.CurrentPlayerTurn);
			
			QuantumRunner.DefaultGame.SendCommand(new QCommandNextTurn
			{
				NextPlayerTurn = f.PlayerToActorId(otherTurn).Value
			});
		}
		
		public void OnRemoveStone(Frame f, EntityRef entity)
		{
			f.Destroy(entity);
		}

		private void MatchSuccess()
		{
			QuantumRunner.DefaultGame.SendCommand(new QCommandGameEnd
			{
				WinnerId = NetworkSetting.LocalPlayerId
			});
		}
	}
}