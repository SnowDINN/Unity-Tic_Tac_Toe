using Quantum;
using UnityEngine.Scripting;

namespace Redbean.Network
{
	[Preserve]
	public class OnConnectSystem : SystemSignalsOnly, ISignalOnPlayerAdded
	{
		public override void OnInit(Frame f)
		{
			f.SetSingleton(new QComponentSystem());
		}
		
		public void OnPlayerAdded(Frame frame, PlayerRef player, bool firstTime)
		{
			var asset = frame.FindAsset(NetworkAsset.Player);
			var entity = frame.Create(asset);

			frame.Set(entity, new QComponentPlayer
			{
				Player = player,
			});
		}
	}
}