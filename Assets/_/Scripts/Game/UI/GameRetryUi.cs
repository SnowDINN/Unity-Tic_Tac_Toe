using System.Linq;
using Quantum;
using R3;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

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
						VotePlayer = NetworkPlayer.LocalPlayerId
					});
				}).AddTo(this);
			
			RxGame.OnGameStatus
				.Where(_ => _.Type is GameStatus.Start or GameStatus.End)
				.Subscribe(_ =>
				{
					foreach (var text in retryTexts)
						text.text = "Retry";
				}).AddTo(this);
			
			RxGame.OnGameVote
				.Where(_ => _.Type is GameVote.Retry)
				.Subscribe(_ =>
				{
					foreach (var text in retryTexts)
						text.text = $"Retry ({_.CurrentCount}/{_.TotalCount})";
				}).AddTo(this);
		}
	}
}