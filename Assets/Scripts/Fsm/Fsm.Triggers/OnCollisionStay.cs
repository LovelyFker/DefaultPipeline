using System;
using UnityEngine;

namespace fsm.triggers
{
	public class OnCollisionStay : ITrigger
	{
		public readonly Collision Collision;

		public OnCollisionStay(Collision collision)
		{
			this.Collision = collision;
		}
	}
}
