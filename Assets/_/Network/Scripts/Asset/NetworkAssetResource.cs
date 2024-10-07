using Quantum;
using UnityEngine;

namespace Redbean.Network
{
	[CreateAssetMenu(fileName = "NetworkAsset", menuName = "Redbean/Network/NetworkAsset")]
	public class NetworkAssetResource : ScriptableObject
	{
		[Header("Player Asset")]
		public AssetRef<EntityPrototype> Player;
		
		[Header("Stone Asset")]
		public AssetRef<EntityPrototype> Stone;
	}

	public class NetworkAsset
	{
		private static NetworkAssetResource system => Resources.Load<NetworkAssetResource>("NetworkAsset");
		
		public static AssetRef<EntityPrototype> Player => system.Player;
		public static AssetRef<EntityPrototype> Stone => system.Stone;
	}

	public class NetworkSetting
	{
		public static int LocalPlayerId => QuantumRunner.Default.NetworkClient.LocalPlayer.ActorNumber;
		
		public const int StoneDestroyTurn = 6;
	}
}