namespace Redbean
{
	public enum ConnectStatus
	{
		Before,
		After
	}
	
	public enum GameStatus
	{
		Start,
		Wait,
		End,
		Reset
	}
	
	public class EVT_ConnectStatus
	{
		public ConnectStatus Status;
		public int ReasonCode;
	}

	public class EVT_GameStatus
	{
		public GameStatus Status;
		public int ActorId;
	}

	public class EVT_GameRetry
	{
		public int RequestRetryCount;
		public int RequireRetryCount;
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