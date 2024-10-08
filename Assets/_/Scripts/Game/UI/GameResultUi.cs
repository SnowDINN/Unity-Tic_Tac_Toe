using R3;
using Redbean.Network;
using UnityEngine;

namespace Redbean.Game
{
	public class GameResultUi : MonoBehaviour
	{
		[SerializeField] private GameObject victory;
		[SerializeField] private GameObject defeat;

		private void Awake()
		{
			GameSubscriber.OnGameStatus
				.Where(_ => _.Type == GameStatus.End)
				.Subscribe(_ =>
				{
					victory.SetActive(_.ActorId == NetworkSetting.LocalPlayerId);
					defeat.SetActive(_.ActorId != NetworkSetting.LocalPlayerId);
				}).AddTo(this);
			
			GameSubscriber.OnGameStatus
				.Where(_ => _.Type is GameStatus.Start or GameStatus.Ready)
				.Subscribe(_ =>
				{
					victory.SetActive(false);
					defeat.SetActive(false);
				}).AddTo(this);
		}
	}
}