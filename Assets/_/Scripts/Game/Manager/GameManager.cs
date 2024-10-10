using UnityEngine;

namespace Redbean.Game
{
	public class GameManager : MonoBehaviour
	{
		public static bool IsReady;

		private void Start()
		{
			IsReady = true;
		}

		private void OnDestroy()
		{
			IsReady = false;
		}
	}
}