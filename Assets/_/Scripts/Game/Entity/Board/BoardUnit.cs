using Quantum;
using R3;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace Redbean.Game
{
	public class BoardUnit : MonoBehaviour
	{
		public bool HasStone => CurrentStone;
		
		[HideInInspector] public StoneUnit CurrentStone;
		[HideInInspector] public int X;
		[HideInInspector] public int Y;
		
		[SerializeField] private GameObject spawnGO;
		[SerializeField] private Button button;

		private GameObject spawnInstance;

		private void Awake()
		{
			button.AsButtonObservable()
				.Subscribe(_ =>
				{
					this.NetworkEventPublish(new QCommandGameNextTurn
					{
						X = X,
						Y = Y,
					});
				}).AddTo(this);

			RxGame.OnStoneCreateOrDestroy
				.Where(_ => _.Type == CreateOrDestroyType.Create)
				.Where(_ => _.X == X && _.Y == Y)
				.Subscribe(_ =>
				{
					spawnInstance = Instantiate(spawnGO, transform);
					CurrentStone = spawnInstance.GetComponent<StoneUnit>();
					CurrentStone.UpdateView(X, Y, _.OwnerId == this.GetActorId());
					
					SetInteraction(false);
				}).AddTo(this);

			RxGame.OnStoneCreateOrDestroy
				.Where(_ => _.Type == CreateOrDestroyType.Destroy)
				.Where(_ => _.X == X && _.Y == Y)
				.Subscribe(_ =>
				{
					if (spawnInstance)
						Destroy(spawnInstance);
					
					CurrentStone = default;
					
					SetInteraction(true);
				}).AddTo(this);
		}

		private void SetInteraction(bool use)
		{
			button.interactable = use;
		}

		public void SetPosition(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}
	}
}