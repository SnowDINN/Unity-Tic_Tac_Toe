using System.Linq;
using UnityEngine;

namespace Redbean.Content
{
	public class InteractionManager : MonoBehaviour
	{
		[SerializeField]
		private InteractionButton[] buttons;
		
		private static InteractionButton[] staticButtons;

		private void Awake() => staticButtons = buttons;

		public static int GetIndex(int instanceID)
		{
			return staticButtons.Select((value, index) => (index, value))
				.FirstOrDefault(_ => _.value.GetInstanceID() == instanceID).index + 1;
		}
	}
}