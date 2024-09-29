using Quantum;
using R3;
using Redbean.Content;
using UnityEngine.Scripting;

namespace Redbean.Network
{
	[Preserve]
	public class InteractionSystem : SystemSignalsOnly, ISignalOnInteraction
	{
		private readonly CompositeDisposable disposables = new();
		
		public override void OnEnabled(Frame f)
		{
			GameSubscriber.OnInteraction
				.Subscribe(_ =>
				{
					f.Signals.OnInteraction(_);
				}).AddTo(disposables);
		}

		public override void OnDisabled(Frame f)
		{
			disposables?.Clear();
			disposables?.Dispose();
		}

		public void OnInteraction(Frame f, int index) =>
			f.Set(f.Create(NetworkAsset.Stone), new Stone { index = index });
	}
}