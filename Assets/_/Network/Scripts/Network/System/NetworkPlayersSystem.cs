using Quantum;
using UnityEngine.Scripting;

namespace Redbean.Network
{
	[Preserve]
	public class NetworkPlayersSystem : SystemSignalsOnly, ISignalOnPlayers
	{
		public override void OnInit(Frame frame)
		{
			frame.SetSingleton(new QComponentSystem());
		}

		public void OnPlayers(Frame frame, QEventPlayers evt)
		{
			frame.Events.OnPlayers(new QEventPlayers
			{
				Type = evt.Type,
				Players = evt.Players
			});
		}
	}
}