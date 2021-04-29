using System;
using System.Collections.Generic;

namespace fsm
{
	public class AnyConditions : ICondition
	{
		private readonly IEnumerable<ICondition> m_conditions;

		public AnyConditions(params ICondition[] conditions)
		{
			this.m_conditions = conditions;
		}

		public bool Validate(IContext context)
		{
			foreach (ICondition current in this.m_conditions)
			{
				if (current.Validate(context))
				{
					return true;
				}
			}
			return false;
		}
	}
}
