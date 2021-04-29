using System;

namespace fsm
{
	public class CallbackState : IState
	{
		private readonly IState m_state;

		public Action UpdateStateEvent = delegate
		{
		};

		public Action OnEnterEvent = delegate
		{
		};

		public Action OnExitEvent = delegate
		{
		};

		public CallbackState(IState state)
		{
			this.m_state = state;
		}

		public CallbackState AddUpdateStateAction(Action updateStateAction)
		{
			this.UpdateStateEvent = (Action)Delegate.Combine(this.UpdateStateEvent, updateStateAction);
			return this;
		}

		public void UpdateState()
		{
			this.m_state.UpdateState();
			this.UpdateStateEvent();
		}

		public void OnEnter()
		{
			this.m_state.OnEnter();
			this.OnEnterEvent();
		}

		public void OnExit()
		{
			this.m_state.OnExit();
			this.OnExitEvent();
		}
	}
}
