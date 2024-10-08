using System.Linq;
using Quantum.Menu;
using R3;
using Redbean.Lobby;
using UnityEngine;
using UnityEngine.UI;

namespace Redbean.Game
{
	public class GameDisconnectUi : MonoBehaviour
	{
		[SerializeField] private Button[] disconnectButtons;
		
		private void Awake()
		{
			disconnectButtons
				.Select(_ => _.AsButtonObservable())
				.Merge()
				.Subscribe(async _ =>
				{
					await NetworkManager.Default.Disconnect(ConnectFailReason.UserRequest);
				}).AddTo(this);
		}
	}
}