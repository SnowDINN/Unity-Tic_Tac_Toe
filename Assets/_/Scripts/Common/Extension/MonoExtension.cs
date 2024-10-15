using System;
using Photon.Deterministic;
using Quantum;
using R3;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace Redbean
{
	public static class MonoExtension
	{
		public static Observable<Unit> AsButtonObservable(this Button button, int inputThrottle = 200) =>
			inputThrottle > 0 
				? button.onClick.AsObservable().Share().ThrottleFirst(TimeSpan.FromMilliseconds(inputThrottle)) 
				: button.onClick.AsObservable().Share();

		public static void SetActive(this Component component, bool active) => 
			component.gameObject.SetActive(active);

		/// <summary>
		/// 네트워크 이벤트 전송
		/// </summary>
		public static void NetworkEventPublish<T>(this MonoBehaviour mono, T value) where T : DeterministicCommand =>
			QuantumRunner.DefaultGame.SendCommand(value);

		/// <summary>
		/// 로컬 사용자 아이디
		/// </summary>
		public static int GetActorId(this MonoBehaviour mono) => 
			QuantumRunner.Default.NetworkClient.LocalPlayer.ActorNumber;

		/// <summary>
		/// Quantum 프레임
		/// </summary>
		public static Frame GetFrame(this MonoBehaviour mono) => 
			QuantumRunner.DefaultGame.Frames.Verified;
		
		public static bool IsMasterClient(this MonoBehaviour mono) => 
			QuantumRunner.Default.NetworkClient.LocalPlayer.IsMasterClient;
	}
}