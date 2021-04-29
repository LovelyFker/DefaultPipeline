using System;

public class FuncAction : IAction
{
	public Action Action;

	public FuncAction(Action action)
	{
		this.Action = action;
	}

	public void Perform(IContext context)
	{
		if (this.Action != null)
		{
			this.Action();
		}
	}
}
