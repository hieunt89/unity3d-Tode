using UnityEngine;
using System.Collections.Generic;
using System;
using Entitas;

[ExecuteInEditMode]
public class Test : IReactiveSystem, IExecuteSystem {
	#region IExecuteSystem implementation

	public void Execute ()
	{
		throw new NotImplementedException ();
	}

	#endregion

	#region IReactiveExecuteSystem implementation
	public void Execute (List<Entity> entities)
	{
		throw new NotImplementedException ();
	}
	#endregion

	#region IReactiveSystem implementation
	public TriggerOnEvent trigger {
		get {
			throw new NotImplementedException ();
		}
	}
	#endregion
	
}
