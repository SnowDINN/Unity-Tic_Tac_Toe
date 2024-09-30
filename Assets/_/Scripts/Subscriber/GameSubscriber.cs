using R3;

namespace Redbean.Content
{
	public class GameSubscriber
	{
		private static readonly Subject<int> onInteraction = new();
		public static Observable<int> OnInteraction => onInteraction.Share();
		
		private static readonly Subject<(int position, int owner)> onSpawn = new();
		public static Observable<(int position, int owner)> OnSpawn => onSpawn.Share();

		public static void Interaction(int position) => onInteraction.OnNext(position);
		public static void Spawn(int position, int owner) => onSpawn.OnNext((position, owner));
	}
}