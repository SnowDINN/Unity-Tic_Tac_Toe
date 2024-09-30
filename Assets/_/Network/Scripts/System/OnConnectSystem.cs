using Quantum;
using UnityEngine.Scripting;

namespace Redbean.Network
{
	[Preserve]
	public class OnConnectSystem : SystemSignalsOnly, ISignalOnPlayerAdded
	{
		public void OnPlayerAdded(Frame f, PlayerRef player, bool firstTime)
		{
			var asset = f.FindAsset(NetworkAsset.Player);
			var entity = f.Create(asset);

			f.Set(entity, new LocalPlayer
			{
				Player = player,
			});
		}
	}
}