using R3;

namespace Redbean.Content
{
	public class GameSubscriber
	{
		
		
		private static readonly Subject<PositionStream> onInteraction = new();
		public static Observable<PositionStream> OnInteraction => onInteraction.Share();
		
		private static readonly Subject<StoneCreateStream> onStoneCreate = new();
		public static Observable<StoneCreateStream> OnStoneCreate => onStoneCreate.Share();
		
		private static readonly Subject<StoneDestroyStream> onStoneDestroy = new();
		public static Observable<StoneDestroyStream> OnStoneDestroy => onStoneDestroy.Share();

		public static void Interaction(PositionStream stream) => onInteraction.OnNext(stream);
		public static void StoneCreate(StoneCreateStream stream) => onStoneCreate.OnNext(stream);
		public static void StoneDestroy(StoneDestroyStream stream) => onStoneDestroy.OnNext(stream);
	}

	public class PositionStream
	{
		public int X;
		public int Y;
	}

	public class StoneCreateStream
	{
		public int OwnerId;
		public int X;
		public int Y;
	}
	
	public class StoneDestroyStream
	{
		public int X;
		public int Y;
	}
}