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

		public StoneSpot CurrentStone;

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
					
					SetInteraction(false);

					var go = Instantiate(Prefab, transform);
					var stone = go.GetComponent<StoneSpot>();
					stone.UpdateView(_.Owner == QuantumRunner.Default.NetworkClient.LocalPlayer.ActorNumber);

					CurrentStone = stone;
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