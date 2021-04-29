using System;
using System.Collections.Generic;

namespace fsm
{
	public class StateMachine
	{
		public ITrigger CurrentTrigger;

		public Action OnStateChanged;

		private List<IState> m_states = new List<IState>(8);

		private Dictionary<Type, TransitionManager> m_triggerToTransitionManagers = new Dictionary<Type, TransitionManager>();

		public float CurrentStateTime
		{
			get;
			set;
		}

		public IState CurrentState
		{
			get;
			set;
		}

		public void ChangeState(IState state)
		{
			if (this.CurrentState != null)
			{
				this.CurrentState.OnExit();
			}
			this.CurrentState = state;
			this.CurrentStateTime = 0f;
			if (this.CurrentState != null)
			{
				this.CurrentState.OnEnter();
			}
			if (this.OnStateChanged != null)
			{
				this.OnStateChanged();
			}
		}

		public void UpdateState(float dt)
		{
			if (this.CurrentState != null)
			{
				this.CurrentState.UpdateState();
			}
			this.CurrentStateTime += dt;
		}

		public StateConfigurator Configure(IState state)
		{
			return new StateConfigurator(this, state);
		}

		public TransitionManager FindTransitionManager(Type trigger)
		{
			TransitionManager transitionManager;
			if (this.m_triggerToTransitionManagers.TryGetValue(trigger, out transitionManager))
			{
				return transitionManager;
			}
			transitionManager = new TransitionManager();
			this.m_triggerToTransitionManagers.Add(trigger, transitionManager);
			return transitionManager;
		}

		public void Trigger(ITrigger trigger)
		{
			this.CurrentTrigger = trigger;
			TransitionManager transitionManager;
			if (this.m_triggerToTransitionManagers.TryGetValue(trigger.GetType(), out transitionManager))
			{
				transitionManager.Process(this);
			}
		}

		public void Trigger<T>() where T : ITrigger
		{
			this.CurrentTrigger = null;
			TransitionManager transitionManager;
			if (this.m_triggerToTransitionManagers.TryGetValue(typeof(T), out transitionManager))
			{
				transitionManager.Process(this);
			}
		}

		public TransitionManager GetTransistionManager<T>()
		{
			TransitionManager result;
			if (this.m_triggerToTransitionManagers.TryGetValue(typeof(T), out result))
			{
				return result;
			}
			return null;
		}

		public void RegisterStates(params IState[] states)
		{
			for (int i = 0; i < states.Length; i++)
			{
				IState item = states[i];
				this.m_states.Add(item);
			}
		}

		private int StateToIndex(IState state)
		{
			return this.m_states.FindIndex((IState a) => a == state);
		}

		private IState IndexToState(int index)
		{
			return this.m_states[index];
		}
	}
}
