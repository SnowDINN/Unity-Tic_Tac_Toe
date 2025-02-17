using System.Threading.Tasks;
using Quantum;
using Quantum.Menu;
using R3;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

namespace Redbean.Network
{
	public class NetworkInitializer
	{
		[RuntimeInitializeOnLoadMethod]
		public static void Setup()
		{
			var networkGO = new GameObject("[Network Manager]");
			Object.DontDestroyOnLoad(networkGO);
			
			var connection = networkGO.AddComponent<QuantumMenuConnectionBehaviourSDK>();
			var network = networkGO.AddComponent<NetworkManager>();
			network.ConnectionSetup(connection);

			var eventGO = new GameObject("[Event System]", typeof(EventSystem), typeof(InputSystemUIInputModule));
			eventGO.transform.SetParent(networkGO.transform);

			NetworkManager.Default = network;
		}
	}
	
	public class NetworkManager : QuantumMonoBehaviour
	{
		public static NetworkManager Default;
		
		private QuantumMenuConnectArgs networkArgs => NetworkAsset.ConnectionArgs;
		private QuantumMenuConnectionBehaviour connection;
		
		public void ConnectionSetup(QuantumMenuConnectionBehaviour connection)
		{
			this.connection = connection;

			connection.OnProgress = new UnityEvent<string>();
			connection.OnProgress
				.AsObservable()
				.Subscribe(RxLobby.SetProgress)
				.AddTo(this);
		}

		private void OnEnable()
		{
			SceneManager.activeSceneChanged += OnActiveSceneChanged;
		}

		private void OnDisable()
		{
			SceneManager.activeSceneChanged -= OnActiveSceneChanged;
		}

		private void OnActiveSceneChanged(Scene current, Scene next)
		{
			RxLobby.SetSceneChanged(next);
		}

		public async Task ConnectAsync(SessionType type, string session = default)
		{
			RxLobby.SetConnect(new EVT_RoomStatus
			{
				Type = type,
				OrderType = SessionOrderType.Before,
				ReasonCode = 0
			});

			switch (type)
			{
				case SessionType.Quick:
				{
					NetworkAsset.ConnectionArgs.SetDefaults(NetworkAsset.QuickConfigure);

					networkArgs.Username = NetworkAsset.QuickConfigure.CodeGenerator.Create();
					networkArgs.AppVersion = $"{Application.version} MatchMaking";
					networkArgs.Session = default;
					networkArgs.Creating = false;
					break;
				}

				case SessionType.Create:
				{
					NetworkAsset.ConnectionArgs.SetDefaults(NetworkAsset.PartyConfigure);
					
					networkArgs.Username = NetworkAsset.PartyConfigure.CodeGenerator.Create();
					networkArgs.AppVersion = $"{Application.version} Party";
					networkArgs.Session = session;
					networkArgs.Creating = true;
					break;
				}
				
				case SessionType.Join:
				{
					NetworkAsset.ConnectionArgs.SetDefaults(NetworkAsset.PartyConfigure);
					
					networkArgs.Username = NetworkAsset.PartyConfigure.CodeGenerator.Create();
					networkArgs.AppVersion = $"{Application.version} Party";
					networkArgs.Session = session;
					networkArgs.Creating = false;
					break;
				}
			}
			networkArgs.Region = networkArgs.PreferredRegion;
			
			var result = await connection.ConnectAsync(networkArgs);
			
			RxLobby.SetConnect(new EVT_RoomStatus
			{
				Type = type,
				OrderType = SessionOrderType.After,
				ReasonCode = result.FailReason
			});
		}
		
		public async Task Disconnect(int reason)
		{
			RxLobby.SetDisconnect(new EVT_RoomStatus
			{
				OrderType = SessionOrderType.Before,
				ReasonCode = reason
			});
			
			await connection.DisconnectAsync(reason);
			
			RxLobby.SetDisconnect(new EVT_RoomStatus
			{
				OrderType = SessionOrderType.After,
				ReasonCode = reason
			});
		}
	}
}