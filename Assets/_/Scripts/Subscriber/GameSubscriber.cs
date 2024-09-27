using Quantum;
using R3;

namespace Redbean.Content
{
	public class GameSubscriber
	{
		private static readonly Subject<int> onInteraction = new();
		public static Observable<int> OnInteraction => onInteraction.Share();
		
		private static readonly Subject<int> onSpawn = new();
		public static Observable<int> OnSpawn => onSpawn.Share();

		public static void Interaction(int index) => onInteraction.OnNext(index);
		public static void Spawn(int index) => onSpawn.OnNext(index);
	}
}