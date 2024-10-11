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
				.Where(_ => _.Type is GameStatus.Start)
				.Subscribe(_ =>
				{
					victory.SetActive(false);
					defeat.SetActive(false);
				}).AddTo(this);
			
			RxGame.OnGameStatus
				.Where(_ => _.Type is GameStatus.End)
				.Subscribe(_ =>
				{
					victory.SetActive(_.ActorId == NetworkPlayer.LocalPlayerId);
					defeat.SetActive(_.ActorId != NetworkPlayer.LocalPlayerId);
				}).AddTo(this);
			
			RxGame.OnGameVote
				.Subscribe(_ =>
				{
					victory.SetActive(_.Type is not GameVote.Ready);
					defeat.SetActive(_.Type is not GameVote.Ready);
				}).AddTo(this);
		}
	}
}