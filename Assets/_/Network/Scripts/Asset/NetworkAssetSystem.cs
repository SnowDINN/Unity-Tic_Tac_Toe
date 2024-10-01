using Quantum;
using UnityEngine;

namespace Redbean.Network
{
	[CreateAssetMenu(fileName = "NetworkAsset", menuName = "Redbean/Network/NetworkAsset")]
	public class NetworkAssetSystem : ScriptableObject
	{
		[Header("Player Asset")]
		public AssetRef<EntityPrototype> Player;
		
		[Header("Stone Asset")]
		public AssetRef<EntityPrototype> Stone;
	}

	public class NetworkAsset
	{
		private static NetworkAssetSystem system => Resources.Load<NetworkAssetSystem>("NetworkAsset");
		
		public static AssetRef<EntityPrototype> Player => system.Player;
		public static AssetRef<EntityPrototype> Stone => system.Stone;
	}

	public class NetworkSetting
	{
		public const int StoneDestroyTurn = 4;
	}
}