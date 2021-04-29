using System;

namespace fsm
{
	public class CompoundState : IState
	{
		private readonly IState[] m_states;

		public Action OnUpdateEvent = delegate
		{
		};

		public Action OnEnterEvent = delegate
		{
		};

		public Action OnExitEvent = delegate
		{
		};

		public CompoundState(params IState[] states)
		{
			this.m_states = states;
		}

		public void OnEnter()
		{
			IState[] states = this.m_states;
			for (int i = 0; i < states.Length; i++)
			{
				IState state = states[i];
				state.OnEnter();
			}
			this.OnEnterEvent();
		}

		public void OnExit()
		{
			IState[] states = this.m_states;
			for (int i = 0; i < states.Length; i++)
			{
				IState state = states[i];
				state.OnExit();
			}
			this.OnExitEvent();
		}

		public void UpdateState()
		{
			IState[] states = this.m_states;
			for (int i = 0; i < states.Length; i++)
			{
				IState state = states[i];
				state.UpdateState();
			}
			this.OnUpdateEvent();
		}
	}
}
