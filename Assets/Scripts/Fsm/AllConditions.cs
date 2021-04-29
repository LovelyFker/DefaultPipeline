using System;
using System.Collections.Generic;

namespace fsm
{
	public class AllConditions : ICondition
	{
		private readonly IEnumerable<ICondition> m_conditions;

		public AllConditions(params ICondition[] conditions)
		{
			this.m_conditions = conditions;
		}

		public AllConditions(params Func<bool>[] conditions)
		{
			ICondition[] array = new FuncCondition[conditions.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new FuncCondition(conditions[i]);
			}
			this.m_conditions = array;
		}

		public bool Validate(IContext context)
		{
			foreach (ICondition current in this.m_conditions)
			{
				if (!current.Validate(context))
				{
					return false;
				}
			}
			return true;
		}
	}
}
