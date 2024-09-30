using System.Linq;
using UnityEngine;

namespace Redbean.Content
{
	public class BoardManager : MonoBehaviour
	{
		private void Awake()
		{
			var components = GetComponentsInChildren<BoardLine>().Select((value, index) => (value, index));
			foreach (var component in components)
				component.value.SetPosition(component.index);
		}
	}
}