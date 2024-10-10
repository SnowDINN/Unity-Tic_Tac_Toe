using Quantum;
using R3;
using UnityEngine;

namespace Redbean.Game
{
	public class GameManager : MonoBehaviour
	{
		public static bool IsReady;

		private void Awake()
		{
			var frame = QuantumRunner.DefaultGame.Frames.Verified;
			
			RxLobby.OnDisconnect
				.Where(_ => _.Status == ConnectionStatus.Before)
				.Subscribe(_ =>
				{
					foreach (var system in frame.SystemsAll)
						frame.SystemDisable(system);
				}).AddTo(this);
		}

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