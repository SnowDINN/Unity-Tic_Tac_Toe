using System.Linq;
using Quantum;
using R3;
using Redbean.Network;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace Redbean.Lobby
{
	public class LobbyPartyButtonUi : MonoBehaviour
	{
		[SerializeField] private Button startButton;
		[SerializeField] private Button[] exitButton;
		
		private void Awake()
		{
			startButton.AsButtonObservable()
				.Subscribe(_ =>
				{
					this.NetworkEventPublish(new QCommandGameStatus
					{
						Type = (int)GameStatus.Start
					});
				}).AddTo(this);
			
			exitButton
				.Select(_ => _.AsButtonObservable())
				.Merge()
				.Subscribe(async _ =>
				{
					await NetworkManager.Default.Disconnect(NetworkCommonValue.UserLeave);
				}).AddTo(this);
		}
	}
}