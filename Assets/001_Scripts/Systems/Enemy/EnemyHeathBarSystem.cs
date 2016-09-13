using UnityEngine;
using System.Collections;
using Entitas;

public class EnemyHeathBarSystem : IReactiveSystem {
	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		for (int i = 0; i < entities.Count; i++) {
			var e = entities [i];
		}
	}
	#endregion
	#region IReactiveSystem implementation
	public TriggerOnEvent trigger {
		get {
			return Matcher.AllOf (Matcher.Hp, Matcher.HpTotal).OnEntityAdded ();
		}
	}
	#endregion
}
