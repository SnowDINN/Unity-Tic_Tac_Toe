using Quantum;

namespace Redbean.Network
{
	public class InteractionSystem : SystemMainThreadFilter<InteractionSystem.Filter>
	{
		public struct Filter
		{
			public EntityRef EntityRef;
		}

		public override void Update(Frame f, ref Filter filter)
		{
			
		}
	}
}