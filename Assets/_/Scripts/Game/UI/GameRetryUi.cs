using System.Linq;
using R3;
using Redbean.Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Redbean.Game
{
	public class GameRetryUi : MonoBehaviour
	{
		[SerializeField] private Button[] retryButtons;
		[SerializeField] private TextMeshProUGUI[] retryTexts;
		
		private void Awake()
		{
			retryButtons
				.Select(_ => _.AsButtonObservable())
				.Merge()
				.Subscribe(_ =>
				{
					this.NetworkEventPublish(new QCommandGameVote
					{
						VoteType = (int)GameVote.Retry,
						ActorId = NetworkPlayer.LocalPlayerId
					});
				}).AddTo(this);
			
			RxGame.OnGameStatus
				.Where(_ => _.Type == GameStatus.Start)
				.Subscribe(_ =>
				{
					foreach (var text in retryTexts)
						text.text = "Retry";
				}).AddTo(this);
			
			RxGame.OnGameVote
				.Where(_ => _.Type == GameVote.Retry)
				.Subscribe(_ =>
				{
					foreach (var text in retryTexts)
						text.text = $"Retry ({_.CurrentCount}/{_.TotalCount})";
				}).AddTo(this);
		}
	}
}