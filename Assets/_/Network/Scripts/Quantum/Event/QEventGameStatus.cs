namespace Quantum
{
	/// <summary>
	/// 게임 상태 로컬 이벤트
	/// </summary>
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
		NextTurn,
	}
}