using UnityEngine;
using System.Collections;
using Entitas;

public class EnemyUpdateViewSystem : IReactiveSystem {

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		for (int i = 0; i < entities.Count; i++) {
			entities [i].view.go.transform.position = entities [i].position.value;
		}
	}

	#endregion

	#region IReactiveSystem implementation

	public TriggerOnEvent trigger {
		get {
			return Matcher.AllOf(Matcher.Enemy, Matcher.View, Matcher.Position).OnEntityAdded ();
		}
	}

	#endregion


}
