using Quantum;

namespace Redbean
{
	public static unsafe class NetworkExtension
	{
		public static void SessionReset(this Frame frame)
		{
			// Game Data Reset
			var qSystem = frame.Unsafe.GetPointerSingleton<QComponentSystem>();
			qSystem->Players = frame.AllocateList<PlayerRef>();
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
	}
}