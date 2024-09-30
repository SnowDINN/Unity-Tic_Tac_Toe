using System.Linq;
using UnityEngine;

namespace Redbean.Content
{
	public class BoardLine : MonoBehaviour
	{
		public void SetPosition(int x)
		{
			var components = GetComponentsInChildren<BoardSpot>().Select((value, index) => (value, index));
			foreach (var component in components)
				component.value.SetPosition(x, component.index);
		}
	}
}