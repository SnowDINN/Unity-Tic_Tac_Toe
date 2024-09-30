using System.Linq;
using UnityEngine;

namespace Redbean.Content
{
	public class BoardLine : MonoBehaviour
	{
		private BoardSpot[] components;
		
		public void SetPosition(int x)
		{
			components = GetComponentsInChildren<BoardSpot>();
			
			var componentArray = components.Select((value, index) => (value, index));
			foreach (var component in componentArray)
				component.value.SetPosition(x, component.index);
		}

		public StoneSpot GetStone(int y)
		{
			if (y < 0 || y >= components.Length)
				return default;
			
			return components[y].CurrentStone;
		}
	}
}