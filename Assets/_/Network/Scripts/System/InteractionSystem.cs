using Quantum;
using UnityEngine.Scripting;

namespace Redbean.Network
{
	[Preserve]
	public unsafe class InteractionSystem : SystemMainThreadFilter<InteractionSystem.Filter>
	{
		public struct Filter
		{
			public EntityRef EntityRef;
			public Transform2D* Transform2D;
		}

		public override void Update(Frame f, ref Filter filter)
		{
			
		}
	}
}