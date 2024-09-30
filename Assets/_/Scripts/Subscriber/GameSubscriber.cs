using R3;

namespace Redbean.Content
{
	public class GameSubscriber
	{
		private static readonly Subject<PositionStream> onInteraction = new();
		public static Observable<PositionStream> OnInteraction => onInteraction.Share();
		
		private static readonly Subject<SpawnStream> onSpawn = new();
		public static Observable<SpawnStream> OnSpawn => onSpawn.Share();

		public static void Interaction(PositionStream position) => onInteraction.OnNext(position);
		public static void Spawn(SpawnStream position) => onSpawn.OnNext(position);
	}

	public class PositionStream
	{
		public int X;
		public int Y;
	}

	public class SpawnStream
	{
		public int Owner;
		public int X;
		public int Y;
	}
}