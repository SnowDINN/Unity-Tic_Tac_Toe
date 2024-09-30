using System.Linq;
using UnityEngine;

namespace Redbean.Content
{
	public class BoardManager : MonoBehaviour
	{
		private static BoardLine[] components;
		
		private void Awake()
		{
			components = GetComponentsInChildren<BoardLine>();
			
			var componentArray = components.Select((value, index) => (value, index));
			foreach (var component in componentArray)
				component.value.SetPosition(component.index);
		}

		public static bool IsOwner(int x, int y)
		{
			if (x < 0 || x >= components.Length)
				return false;
			
			return components[x].GetStone(y) && components[x].GetStone(y).IsOwner;
		}
	}
}