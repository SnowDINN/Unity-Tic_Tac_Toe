using System.Linq;
using UnityEngine;

namespace Redbean.Game
{
	public class BoardLine : MonoBehaviour
	{
		private BoardPosition[] components;
		
		public void SetPosition(int y)
		{
			components = GetComponentsInChildren<BoardPosition>();
			
			var componentArray = components.Select((value, index) => (value, index));
			foreach (var component in componentArray)
				component.value.SetPosition(component.index, y);
		}

		public StoneEntity GetStone(int x)
		{
			if (x < 0 || x >= components.Length)
				return default;

			return components[x]
				? components[x].CurrentStone 
				: default;
		}
	}
}