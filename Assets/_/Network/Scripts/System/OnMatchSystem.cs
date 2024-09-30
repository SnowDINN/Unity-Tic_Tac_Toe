using Quantum;
using Redbean.Content;
using UnityEngine;
using UnityEngine.Scripting;

namespace Redbean.Network
{
	[Preserve]
	public class OnMatchSystem : SystemSignalsOnly, ISignalOnMatch
	{
		public void OnMatch(Frame f, int x, int y)
		{
			if (BoardManager.IsOwner(x + 1, y))
				if (BoardManager.IsOwner(x + 2, y))
				{
					Debug.Log("Match !!");
					return;
				}
			
			if (BoardManager.IsOwner(x - 1, y))
				if (BoardManager.IsOwner(x - 2, y))
				{
					Debug.Log("Match !!");
					return;
				}
			
			if (BoardManager.IsOwner(x, y + 1))
				if (BoardManager.IsOwner(x, y + 2))
				{
					Debug.Log("Match !!");
					return;
				}
			
			if (BoardManager.IsOwner(x, y - 1))
				if (BoardManager.IsOwner(x, y - 2))
				{
					Debug.Log("Match !!");
					return;
				}
			
			if (BoardManager.IsOwner(x + 1, y + 1))
				if (BoardManager.IsOwner(x + 2, y + 2))
				{
					Debug.Log("Match !!");
					return;
				}
			
			if (BoardManager.IsOwner(x - 1, y - 1))
				if (BoardManager.IsOwner(x - 2, y - 2))
				{
					Debug.Log("Match !!");
					return;
				}
			
			if (BoardManager.IsOwner(x + 1, y - 1))
				if (BoardManager.IsOwner(x + 2, y - 2))
				{
					Debug.Log("Match !!");
					return;
				}
			
			if (BoardManager.IsOwner(x - 1, y + 1))
				if (BoardManager.IsOwner(x - 2, y + 2))
				{
					Debug.Log("Match !!");
					return;
				}
			
			// Center Pivot
			if (BoardManager.IsOwner(x + 1, y))
				if (BoardManager.IsOwner(x - 1, y))
				{
					Debug.Log("Match !!");
					return;
				}
			
			if (BoardManager.IsOwner(x, y + 1))
				if (BoardManager.IsOwner(x, y - 1))
				{
					Debug.Log("Match !!");
					return;
				}
			
			if (BoardManager.IsOwner(x + 1, y + 1))
				if (BoardManager.IsOwner(x - 1, y - 1))
				{
					Debug.Log("Match !!");
					return;
				}
			
			if (BoardManager.IsOwner(x - 1, y - 1))
				if (BoardManager.IsOwner(x + 1, y + 1))
				{
					Debug.Log("Match !!");
					return;
				}
			
			Debug.Log("Continue Game");
		}
	}
}