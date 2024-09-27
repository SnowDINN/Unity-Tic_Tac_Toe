using Quantum;
using R3;
using Redbean.Content;
using UnityEngine.Scripting;

namespace Redbean.Network
{
	[Preserve]
	public unsafe class InteractionSystem : SystemSignalsOnly, ISignalOnInteraction
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

		public void OnInteraction(Frame f, int index)
		{
			if (!AssetGuid.TryParse("1461148EC9CA1107", out var guid, false))
				return;

			var asset = f.FindAsset<EntityPrototype>(guid);
			var entity = f.Create(asset);

			var component = f.Unsafe.GetPointer<Stone>(entity);
			component->index = index;
		}
	}
}