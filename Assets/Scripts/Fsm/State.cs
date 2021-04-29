using System;

namespace fsm
{
	public class State : IState
	{
		public Action UpdateStateEvent = delegate
		{
		};

		public Action OnEnterEvent = delegate
		{
		};

		public Action OnExitEvent = delegate
		{
		};

		public void UpdateState()
		{
			this.UpdateStateEvent();
		}

		public void OnEnter()
		{
			this.OnEnterEvent();
		}

		public void OnExit()
		{
			this.OnExitEvent();
		}
	}
}
