using R3;
using Redbean.Network;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace Redbean.Game
{
	public class BoardPosition : MonoBehaviour
	{
		[HideInInspector]
		public StoneEntity CurrentStone;
		
		[SerializeField]
		private GameObject Prefab;
		
		[SerializeField]
		private Button Button;

		private GameObject instance;
		private int x;
		private int y;

		private void Awake()
		{
			Button.AsButtonObservable()
				.Subscribe(_ =>
				{
					GameSubscriber.SetBoardSelect(new EVT_Position
					{
						X = x,
						Y = y
					});
				}).AddTo(this);

			GameSubscriber.OnStoneCreate
				.Where(_ => _.Position.X == x && _.Position.Y == y)
				.Subscribe(_ =>
				{
					instance = Instantiate(Prefab, transform);
					CurrentStone = instance.GetComponent<StoneEntity>();
					CurrentStone.UpdateView(x, y, _.OwnerId == NetworkSetting.LocalPlayerId);
					
					SetInteraction(false);
				}).AddTo(this);

			GameSubscriber.OnStoneDestroy
				.Where(_ => _.X == x && _.Y == y)
				.Subscribe(_ =>
				{
					if (instance)
						Destroy(instance);
					
					CurrentStone = default;
					
					SetInteraction(true);
				}).AddTo(this);
		}

		private void SetInteraction(bool use)
		{
			Button.interactable = use;
		}

		public void SetPosition(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
	}
}