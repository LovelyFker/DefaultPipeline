using System;

namespace fsm
{
	public class Transition
	{
		public IState SourceState;

		public IState TargetState;

		public ICondition Condition;

		public IAction Action;

		public Transition(IState sourceState, IState targetState, ICondition condition, IAction action)
		{
			this.SourceState = sourceState;
			this.TargetState = targetState;
			this.Condition = condition;
			this.Action = action;
		}
	}
}
