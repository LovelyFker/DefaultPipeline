using System;

namespace fsm
{
	public interface IState
	{
		void UpdateState();

		void OnEnter();

		void OnExit();
	}
}
