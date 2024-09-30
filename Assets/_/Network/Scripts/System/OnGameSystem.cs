using Quantum;
using UnityEngine.Scripting;

namespace Redbean.Network
{
	[Preserve]
	public unsafe class OnGameSystem : SystemMainThreadFilter<OnGameSystem.Filter>
	{
		public struct Filter
		{
			public EntityRef Entity;
			public LocalPlayer* LocalPlayer;
		}
		
		public override void OnInit(Frame f)
		{
			f.SetSingleton(new Game());
		}
		
		public override void Update(Frame f, ref Filter filter)
		{
			if (f.GetPlayerCommand(filter.LocalPlayer->Player) is not QCommandGame command)
				return;
			
			f.Unsafe.GetPointerSingleton<Game>()->NextTurn();
		}
	}
}