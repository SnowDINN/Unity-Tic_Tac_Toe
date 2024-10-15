namespace Quantum
{
	public enum ConnectionType
	{
		Connect,
		Disconnect
	}
	
	public enum SessionType
	{
		Quick,
		Create,
		Join
	}
	
	public enum SessionOrderType
	{
		Before,
		After
	}

	public enum CreateOrDestroyType
	{
		Create,
		Destroy
	}
	
	public enum GameStatus
	{
		Start,
		End,
		NextTurn,
	}
	
	public enum GameVote
	{
		Ready,
		Retry
	}
}