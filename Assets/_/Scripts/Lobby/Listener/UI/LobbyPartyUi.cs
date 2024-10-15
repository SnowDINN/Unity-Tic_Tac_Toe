using Quantum;
using R3;
using TMPro;
using UnityEngine;

namespace Redbean.Lobby
{
	public class LobbyPartyUi : MonoBehaviour
	{
		[Header("Common ui")]
		[SerializeField] private GameObject mainGO;
		[SerializeField] private GameObject clientGO;
		[SerializeField] private TextMeshProUGUI progressText;
		
		[Header("Show ui by client ")]
		[SerializeField] private GameObject masterClientGO;
		[SerializeField] private GameObject inviteClientGO;

		[Header("Join user name")]
		[SerializeField] private TextMeshProUGUI[] clientNameTexts;

		private void Awake()
		{
			RxLobby.OnConnect
				.Where(_ => _.Type is RoomType.CreateRoom or RoomType.JoinRoom)
				.Where(_ => _.OrderType is OrderType.Before)
				.Subscribe(_ =>
				{
					mainGO.SetActive(true);
					progressText.SetActive(true);

					foreach (var name in clientNameTexts)
						name.text = "";
				}).AddTo(this);
			
			RxLobby.OnConnect
				.Where(_ => _.Type is RoomType.CreateRoom or RoomType.JoinRoom)
				.Where(_ => _.OrderType is OrderType.After)
				.Subscribe(_ =>
				{
					clientGO.SetActive(true);
					progressText.SetActive(false);
					
					masterClientGO.SetActive(this.IsMasterClient());
					inviteClientGO.SetActive(!this.IsMasterClient());
				}).AddTo(this);
			
			RxLobby.OnDisconnect
				.Subscribe(_ =>
				{
					mainGO.SetActive(false);
					clientGO.SetActive(false);
					
					masterClientGO.SetActive(false);
					inviteClientGO.SetActive(false);
				}).AddTo(this);

			RxLobby.OnPlayers
				.Where(_ => _.Type is ConnectionType.Connect)
				.Subscribe(_ =>
				{
					for (var i = 0; i < _.Players.Count; i++)
						clientNameTexts[i].text = this.GetFrame().GetPlayerData(_.Players[i]).PlayerNickname;
				}).AddTo(this);
			
			RxLobby.OnPlayers
				.Where(_ => _.Type is ConnectionType.Disconnect)
				.Subscribe(_ =>
				{
					for (var i = 0; i < _.Players.Count; i++)
						clientNameTexts[i].text = this.GetFrame().GetPlayerData(_.Players[i]).PlayerNickname;
				}).AddTo(this);

			RxLobby.OnProgress
				.Subscribe(_ =>
				{
					progressText.text = _;
				}).AddTo(this);
		}
	}
}