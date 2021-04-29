using System;

namespace fsm
{
	public class FuncCondition : ICondition
	{
		private readonly Func<bool> m_function;

		public FuncCondition(Func<bool> function)
		{
			this.m_function = function;
		}

		public bool Validate(IContext context)
		{
			return this.m_function == null || this.m_function();
		}
	}
}
