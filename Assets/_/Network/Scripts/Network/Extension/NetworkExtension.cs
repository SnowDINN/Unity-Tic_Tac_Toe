using System.Collections.Generic;
using System.Linq;
using Quantum;

namespace Redbean
{
	public static unsafe class NetworkExtension
	{
		public static void SessionReset(this Frame frame)
		{
			// Game Data Reset
			var qSystem = frame.Unsafe.GetPointerSingleton<QComponentSystem>();
			qSystem->ReadyPlayers = frame.AllocateList<PlayerRef>();
			qSystem->RetryPlayers = frame.AllocateList<PlayerRef>();
			qSystem->CurrentPlayerTurn = default;
			qSystem->CurrentTurn = default;
			
			var qStone = frame.Filter<QComponentStone>();
			while (qStone.Next(out var entity, out var stone))
			{
				frame.Events.OnStoneDestroyed(stone);
				frame.Destroy(entity);
			}
		}

		public static void AddPlayer(this Frame frame, PlayerRef player)
		{
			var entity = frame.Create();
			var qPlayer = new QComponentPlayer
			{
				Player = player
			};
			frame.Add(entity, qPlayer);
			
			RefreshPlayers(frame);
		}

		public static void RemovePlayer(this Frame frame, PlayerRef player)
		{
			var qPlayerFilter = frame.Filter<QComponentPlayer>();
			while (qPlayerFilter.Next(out var entity, out var qPlayer))
			{
				if (frame.PlayerToActorId(qPlayer.Player) == frame.PlayerToActorId(player))
					frame.Destroy(entity);
			}

			RefreshPlayers(frame);
		}

		public static List<PlayerRef> GetPlayers(this Frame frame)
		{
			var qSystem = frame.Unsafe.GetPointerSingleton<QComponentSystem>();
			var qPlayers = frame.ResolveList(qSystem->Players);

			return qPlayers.ToList();
		}

		private static void RefreshPlayers(Frame frame)
		{
			var qPlayers = frame.AllocateList<PlayerRef>();
			var qPlayersFilter = frame.Filter<QComponentPlayer>();
			while (qPlayersFilter.Next(out _, out var qPlayerValue))
				qPlayers.Add(qPlayerValue.Player);
			
			var qSystem = frame.Unsafe.GetPointerSingleton<QComponentSystem>();
			qSystem->Players = qPlayers;
		}
	}
}