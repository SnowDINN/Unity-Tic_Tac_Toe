namespace Quantum
{
	public class QEventGameStatus
	{
		public GameStatus Type;
		public int ActorId;
		public object[] Args;
	}
	
	public enum GameStatus
	{
		Start,
		End,
		Ready,
		Retry,
		NextTurn,
	}
}