using System;
using Quantum;
using R3;
using TMPro;
using UnityEngine;

namespace Redbean.Game
{
	public class GameTimerUi : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI text;

		private IDisposable disposable;

		private void Awake()
		{
			RxGame.OnGameStatus
				.Where(_ => _.Type is GameStatus.NextTurn)
				.Subscribe(_ =>
				{
					var timer = 0;
					
					text.text = "";
					disposable?.Dispose();
					disposable = Observable
						.Interval(TimeSpan.FromSeconds(1))
						.Select(_ => GameConst.TimerSecond - timer++)
						.Subscribe(_ =>
						{
							switch (_)
							{
								case > GameConst.ShowTimerSecond:
									text.text = "";
									return;

								case >= 0:
									text.text = $"{_}";
									break;
								
								case < 0:
									RxGame.SetGameTimeout();
									break;
							}
						});
				}).AddTo(this);

			RxGame.OnGameVote
				.Subscribe(_ =>
				{
					text.text = "";
					disposable?.Dispose();
				}).AddTo(this);
		}

		private void OnDestroy()
		{
			text.text = "";
			disposable?.Dispose();
		}
	}
}