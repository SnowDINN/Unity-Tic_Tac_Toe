using System.Linq;
using Quantum;
using R3;
using Redbean.Network;
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
					QuantumRunner.DefaultGame.SendCommand(new QCommandGameVote
					{
						VoteType = (int)GameVote.Retry,
						ActorId = NetworkSetting.LocalPlayerId
					});
				}).AddTo(this);
			
			GameSubscriber.OnGameStatus
				.Where(_ => _.Type == GameStatus.Start)
				.Subscribe(_ =>
				{
					foreach (var text in retryTexts)
						text.text = "Retry";
				}).AddTo(this);
			
			GameSubscriber.OnGameRetry
				.Subscribe(_ =>
				{
					foreach (var text in retryTexts)
						text.text = $"Retry ({_.RequestRetryCount}/{_.RequireRetryCount})";
				}).AddTo(this);
		}
	}
}