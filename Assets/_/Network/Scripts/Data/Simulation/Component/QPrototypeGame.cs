using UnityEngine;

namespace Quantum
{
	public partial struct Game
	{
		public void NextTurn()
		{
			CurrentTurn += 1;
			
			Debug.Log(CurrentTurn);
		}
	}
}