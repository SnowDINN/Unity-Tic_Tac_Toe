namespace Quantum
{
	public partial struct QComponentSystem
	{
		public int NextTurn() => TurnCount += 1;
	}
} 