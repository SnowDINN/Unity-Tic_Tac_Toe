using System;
using UnityEngine;

namespace Redbean.Content
{
	public class GameManager : MonoBehaviour
	{
		public static GameManager Default;

		private void Awake()
		{
			Default = this;
		}

		private void Start()
		{
			Default = this;
		}

		private void OnDestroy()
		{
			Default = default;
		}
	}
}