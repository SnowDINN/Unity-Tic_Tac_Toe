namespace Quantum
{
	public class QEventGameVote
	{
		public GameVote Type;
		public int CurrentCount;
		public int TotalCount;
	}
	
	public enum GameVote
	{
		Ready,
		Retry
	}
}