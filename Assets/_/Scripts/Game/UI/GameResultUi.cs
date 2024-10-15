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
			RxGame.OnGameVote
				.Where(_ => _.Type is GameVote.Ready)
				.Subscribe(_ =>
				{
					victory.SetActive(false);
					defeat.SetActive(false);
				}).AddTo(this);
			
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
					victory.SetActive(_.ActorId == this.GetActorId());
					defeat.SetActive(_.ActorId != this.GetActorId());
				}).AddTo(this);
		}
	}
}