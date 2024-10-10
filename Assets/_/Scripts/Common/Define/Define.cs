namespace Redbean
{
	public enum ConnectionStatus
	{
		Before,
		After
	}

	public enum GameVote
	{
		Ready,
		Retry
	}
	
	public enum GameStatus
	{
		Start,
		End,
		Next,
		Ready,
		Retry
	}
	
	public class EVT_ConnectionStatus
	{
		public ConnectionStatus Status;
		public int ReasonCode;
	}

	public class EVT_GameStatus
	{
		public GameStatus Type;
		public int ActorId;
	}

	public class EVT_GameVote
	{
		public GameVote Type;
		public int CurrentCount;
		public int TotalCount;
	}
	
	public class EVT_Position
	{
		public int X;
		public int Y;
	}
	
	public class EVT_PositionAndOwner
	{
		public int OwnerId;
		public EVT_Position Position;
	}
}