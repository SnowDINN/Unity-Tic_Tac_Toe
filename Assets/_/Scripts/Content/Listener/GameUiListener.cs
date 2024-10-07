using System.Linq;
using Quantum;
using Quantum.Menu;
using R3;
using Redbean.Lobby;
using Redbean.Network;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace Redbean.Content
{
	public class GameUiListener : MonoBehaviour
	{
		[Header("Game Wait")]
		[SerializeField] private GameObject wait;
		
		[Header("Game Result")]
		[SerializeField] private GameObject victory;
		[SerializeField] private GameObject defeat;

		[Header("Retry Components")]
		[SerializeField] private Button[] disconnectButtons;
		[SerializeField] private Button[] retryButtons;
		[SerializeField] private TextMeshProUGUI[] retryTexts;
		
		private void Awake()
		{
			disconnectButtons
				.Select(_ => _.AsButtonObservable())
				.Merge()
				.Subscribe(async _ =>
				{
					await NetworkManager.Default.Disconnect(ConnectFailReason.UserRequest);
				}).AddTo(this);
			
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
				.Subscribe(_ =>
				{
					wait.SetActive(_.Status == GameStatus.Wait);
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