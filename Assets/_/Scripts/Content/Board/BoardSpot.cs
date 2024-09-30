using Quantum;
using R3;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace Redbean.Content
{
	public class BoardSpot : MonoBehaviour
	{
		[SerializeField]
		private GameObject Prefab;
		
		[SerializeField]
		private Button Button;
		
		private int X;
		private int Y;

		private void Awake()
		{
			Button.AsButtonObservable()
				.Subscribe(_ =>
				{
					GameSubscriber.Interaction(new PositionStream
					{
						X = X,
						Y = Y
					});
				}).AddTo(this);

			GameSubscriber.OnSpawn
				.Subscribe(_ =>
				{
					if (_.X != X || _.Y != Y)
						return;

					var go = Instantiate(Prefab, transform);
					var stone = go.GetComponent<StonePrinter>();
					stone.UpdateView(_.Owner == QuantumRunner.Default.NetworkClient.LocalPlayer.ActorNumber);
						
					SetInteraction(false);
				}).AddTo(this);
		}

		private void SetInteraction(bool use)
		{
			Button.interactable = use;
		}

		public void SetPosition(int x, int y)
		{
			X = x;
			Y = y;
		}
	}
}