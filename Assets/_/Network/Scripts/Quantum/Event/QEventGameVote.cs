namespace Quantum
{
	/// <summary>
	/// 게임 투표 로컬 이벤트
	/// </summary>
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