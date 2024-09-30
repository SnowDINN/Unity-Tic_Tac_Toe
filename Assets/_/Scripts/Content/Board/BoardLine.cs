using System.Linq;
using UnityEngine;

namespace Redbean.Content
{
	public class BoardLine : MonoBehaviour
	{
		private BoardSpot[] components;
		
		public void SetPosition(int y)
		{
			components = GetComponentsInChildren<BoardSpot>();
			
			var componentArray = components.Select((value, index) => (value, index));
			foreach (var component in componentArray)
				component.value.SetPosition(component.index, y);
		}

		public StoneSpot GetStone(int x)
		{
			if (x < 0 || x >= components.Length)
				return default;

			return components[x]
				? components[x].CurrentStone 
				: default;
		}
	}
}