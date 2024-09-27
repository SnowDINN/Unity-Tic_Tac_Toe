using Quantum;
using UnityEngine.Scripting;

namespace Redbean.Network
{
	[Preserve]
	public class ConnectSystem : SystemSignalsOnly, ISignalOnPlayerAdded
	{
		public void OnPlayerAdded(Frame f, PlayerRef player, bool firstTime)
		{

		}
	}
}