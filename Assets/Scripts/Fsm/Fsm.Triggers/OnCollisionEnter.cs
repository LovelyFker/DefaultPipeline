using System;
using UnityEngine;

namespace fsm.triggers
{
	public class OnCollisionEnter : ITrigger
	{
		public readonly Collision Collision;

		public OnCollisionEnter(Collision collision)
		{
			this.Collision = collision;
		}
	}
}
