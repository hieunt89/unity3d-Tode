using UnityEngine;
using System.Collections;
using Entitas;
using System.Collections.Generic;

public class ProjectileReachEndSystem : IReactiveSystem {
	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		for (int i = 0; i < entities.Count; i++) {
			entities [i].IsMarkedForDestroy (true);
		}
	}

	#endregion

	#region IReactiveSystem implementation

	public TriggerOnEvent trigger {
		get {
			return Matcher.AllOf (Matcher.ProjectileMark, Matcher.ReachedEnd).OnEntityAdded();
		}
	}

	#endregion
}
