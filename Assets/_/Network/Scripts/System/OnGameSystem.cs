using Quantum;
using UnityEngine.Scripting;

namespace Redbean.Network
{
	[Preserve]
	public unsafe class OnGameSystem : SystemSignalsOnly, ISignalOnNextTurn
	{
		public override void OnInit(Frame f)
		{
			f.SetSingleton(new Game());
		}

		public void OnNextTurn(Frame f)
		{
			f.Unsafe.GetPointerSingleton<Game>()->NextTurn();
		}
	}
}