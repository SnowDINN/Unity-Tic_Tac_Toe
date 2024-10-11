using System.Linq;
using UnityEngine;

namespace Redbean.Game
{
	public class BoardBundle : MonoBehaviour
	{
		[HideInInspector]
		public BoardUnit[] Units;
		
		public void SetPosition(int y)
		{
			Units = GetComponentsInChildren<BoardUnit>();
			
			var componentArray = Units.Select((value, index) => (value, index));
			foreach (var component in componentArray)
				component.value.SetPosition(component.index, y);
		}

		public StoneUnit GetStone(int x)
		{
			if (x < 0 || x >= Units.Length)
				return default;

			return Units[x]
				? Units[x].CurrentStone 
				: default;
		}
	}
}