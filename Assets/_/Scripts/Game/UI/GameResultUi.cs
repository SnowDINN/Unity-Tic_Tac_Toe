using Quantum;
using R3;
using UnityEngine;

namespace Redbean.Game
{
	public class GameResultUi : MonoBehaviour
	{
		[SerializeField] private GameObject victory;
		[SerializeField] private GameObject defeat;

		private void Awake()
		{
			RxGame.OnGameStatus
				.Where(_ => _.Type == GameStatus.End)
				.Subscribe(_ =>
				{
					victory.SetActive(_.ActorId == NetworkPlayer.LocalPlayerId);
					defeat.SetActive(_.ActorId != NetworkPlayer.LocalPlayerId);
				}).AddTo(this);
			
			RxGame.OnGameStatus
				.Where(_ => _.Type is GameStatus.Start or GameStatus.Ready)
				.Subscribe(_ =>
				{
					victory.SetActive(false);
					defeat.SetActive(false);
				}).AddTo(this);
		}
	}
}