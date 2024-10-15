using Quantum;
using R3;
using UnityEngine;

namespace Redbean.Game
{
	public class GameManager : MonoBehaviour
	{
		public static bool IsReady;
		public static bool IsMyTurn;

		private void Awake()
		{
			RxLobby.OnDisconnect
				.Where(_ => _.Status is ConnectionStatus.Before)
				.Subscribe(_ =>
				{
					foreach (var system in this.GetFrame().SystemsAll)
						this.GetFrame().SystemDisable(system);
				}).AddTo(this);

			RxGame.OnGameStatus
				.Where(_ => _.Type is GameStatus.NextTurn)
				.Subscribe(_ =>
				{
					IsMyTurn = _.ActorId == this.GetActorId();
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