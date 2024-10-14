using Quantum;

namespace Redbean
{
	public enum ConnectionType
	{
		Matchmaking,
		CreateRoom,
		JoinRoom
	}
	
	public enum ConnectionStatus
	{
		Before,
		After
	}

	public enum CreateOrDestroyType
	{
		Create,
		Destroy
	}

	public class GameConst
	{
		public const int TimerSecond = 10;
		public const int ShowTimerSecond = 8;
	}
	
	public class EVT_ConnectionStatus
	{
		public ConnectionType Type;
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
	
	public class EVT_PositionAndOwner : EVT_Position
	{
		public CreateOrDestroyType Type;
		public int OwnerId;
	}
}