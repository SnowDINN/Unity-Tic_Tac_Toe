using R3;
using Redbean.Network;
using UnityEngine;
using UnityEngine.UI;

namespace Redbean.Game
{
	public class BoardUnit : MonoBehaviour
	{
		[HideInInspector]
		public StoneUnit CurrentStone;
		
		[SerializeField]
		private GameObject spawnGO;
		
		[SerializeField]
		private Button button;

		private GameObject spawnInstance;
		private int x;
		private int y;

		private void Awake()
		{
			button.AsButtonObservable()
				.Subscribe(_ =>
				{
					RxGame.SetBoardSelect(new EVT_Position
					{
						X = x,
						Y = y
					});
				}).AddTo(this);

			RxGame.OnStoneCreate
				.Where(_ => _.Position.X == x && _.Position.Y == y)
				.Subscribe(_ =>
				{
					spawnInstance = Instantiate(spawnGO, transform);
					CurrentStone = spawnInstance.GetComponent<StoneUnit>();
					CurrentStone.UpdateView(x, y, _.OwnerId == NetworkPlayer.LocalPlayerId);
					
					SetInteraction(false);
				}).AddTo(this);

			RxGame.OnStoneDestroy
				.Where(_ => _.X == x && _.Y == y)
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
			this.x = x;
			this.y = y;
		}
	}
}