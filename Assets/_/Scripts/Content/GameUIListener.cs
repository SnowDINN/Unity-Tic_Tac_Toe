using System.Linq;
using Quantum;
using R3;
using Redbean.Network;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace Redbean.Content
{
	public class GameUIListener : MonoBehaviour
	{
		[SerializeField]
		private GameObject victory;

		[SerializeField]
		private GameObject defeat;

		[Header("Retry Components")]
		[SerializeField]
		private Button[] retryButtons;
		
		[SerializeField]
		private TextMeshProUGUI[] retryTexts;
		
		private void Awake()
		{
			retryButtons
				.Select(_ => _.AsButtonObservable())
				.Merge()
				.Subscribe(_ =>
				{
					QuantumRunner.DefaultGame.SendCommand(new QCommandGameRetry
					{
						ActorId = NetworkSetting.LocalPlayerId
					});
				}).AddTo(this);
			
			GameSubscriber.OnGameStatus
				.Where(_ => _.Status == GameStatus.Start)
				.Subscribe(_ =>
				{
					foreach (var text in retryTexts)
						text.text = "Retry";
				}).AddTo(this);
			
			GameSubscriber.OnGameStatus
				.Where(_ => _.Status == GameStatus.End)
				.Subscribe(_ =>
				{
					victory.SetActive(_.ActorId == NetworkSetting.LocalPlayerId);
					defeat.SetActive(_.ActorId != NetworkSetting.LocalPlayerId);
				}).AddTo(this);

			GameSubscriber.OnGameRetry
				.Subscribe(_ =>
				{
					foreach (var text in retryTexts)
						text.text = $"Retry ({_.RequestRetryCount}/{_.RequireRetryCount})";
				}).AddTo(this);
			
			GameSubscriber.OnGameStatus
				.Where(_ => _.Status == GameStatus.Reset)
				.Subscribe(_ =>
				{
					victory.SetActive(false);
					defeat.SetActive(false);
				}).AddTo(this);
		}
	}
}