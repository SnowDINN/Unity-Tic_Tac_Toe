using Quantum;
using Quantum.Menu;
using UnityEngine;

namespace Redbean
{
	[CreateAssetMenu(fileName = "NetworkAsset", menuName = "Redbean/Network/NetworkAsset")]
	public class NetworkAssetResource : ScriptableObject
	{
		[Header("")]
		public QuantumMenuConfig MatchmakingConfigure;
		public QuantumMenuConfig PartyConfigure;
		
		[Header("Quantum common args")]
		public QuantumMenuConnectArgs connectionArgs;
		
		[Header("Map Asset")]
		public AssetRef<Map> Map;
	}

	public class NetworkAsset
	{
		private static NetworkAssetResource networkAssetResource;
		private static NetworkAssetResource asset
		{
			get
			{
				if (!networkAssetResource)
					networkAssetResource = Resources.Load<NetworkAssetResource>("NetworkAsset");

				return networkAssetResource;
			}
		}

		public static QuantumMenuConfig MatchmakingConfigure => asset.MatchmakingConfigure;
		public static QuantumMenuConfig PartyConfigure => asset.PartyConfigure;
		public static QuantumMenuConnectArgs ConnectionArgs => asset.connectionArgs;
		public static AssetRef<Map> Map => asset.Map;
	}

	public class NetworkCommonValue
	{
		public const int StoneDestroyTurn = 6;
		public const int UserLeave = 1;
	}
}