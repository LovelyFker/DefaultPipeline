using System;
using System.Collections.Generic;

namespace fsm
{
	public class TransitionManager
	{
		private readonly Dictionary<IState, List<Transition>> m_transitions = new Dictionary<IState, List<Transition>>();

		public TransitionManager AddTransition(IState from, IState to, ICondition condition = null, IAction action = null)
		{
			List<Transition> list;
			if (!this.m_transitions.TryGetValue(from, out list))
			{
				list = new List<Transition>();
				this.m_transitions.Add(from, list);
			}
			list.Add(new Transition(from, to, condition, action));
			return this;
		}

		public TransitionManager AddTransition(IState from, IState to, Func<bool> condition = null, Action action = null)
		{
			return this.AddTransition(from, to, new FuncCondition(condition), new FuncAction(action));
		}

		public bool Process(StateMachine stateMachine)
		{
			List<Transition> conditionAndStatePairList;
			return this.m_transitions.TryGetValue(stateMachine.CurrentState, out conditionAndStatePairList) && this.ProcessTransitionList(stateMachine, conditionAndStatePairList);
		}

		public bool ProcessTransitionList(StateMachine stateMachine, List<Transition> conditionAndStatePairList)
		{
			for (int i = 0; i < conditionAndStatePairList.Count; i++)
			{
				Transition transition = conditionAndStatePairList[i];
				if (transition.Condition == null || transition.Condition.Validate(null))
				{
					if (transition.Action != null)
					{
						transition.Action.Perform(null);
					}
					stateMachine.ChangeState(transition.TargetState);
					return true;
				}
			}
			return false;
		}
	}
}
