using UnityEngine;
using System.Collections;
using Entitas;

public class HpWatchSystem : IReactiveSystem, IEnsureComponents {
	#region IEnsureComponents implementation

	public IMatcher ensureComponents {
		get {
			return Matcher.Hp;
		}
	}

	#endregion

	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		for (int i = 0; i < entities.Count; i++) {
			if(entities[i].hp.value <= 0){
				entities [i].IsMarkedForDestroy (true);
			}
		}
	}
	#endregion
	#region IReactiveSystem implementation
	public TriggerOnEvent trigger {
		get {
			return Matcher.Hp.OnEntityAdded ();
		}
	}
	#endregion
	
}
