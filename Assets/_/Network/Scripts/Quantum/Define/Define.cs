namespace Quantum
{
	public enum ConnectionType
	{
		Connect,
		Disconnect
	}
	
	public enum OrderType
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