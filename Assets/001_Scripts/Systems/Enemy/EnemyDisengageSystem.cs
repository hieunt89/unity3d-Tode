using UnityEngine;
using System.Collections;
using Entitas;

public class EnemyDisengageSystem : IReactiveSystem, IEnsureComponents {
	#region IEnsureComponents implementation

	public IMatcher ensureComponents {
		get {
			return Matcher.AllOf (Matcher.Enemy, Matcher.Active);
		}
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		for (int i = 0; i < entities.Count; i++) {
			var enemy = entities [i];

			if (!enemy.hasCloseCombat && !enemy.hasEngage && !enemy.isMovable) {
				enemy.IsMovable (true);
			}

		}
	}

	#endregion

	#region IReactiveSystem implementation

	public TriggerOnEvent trigger {
		get {
			return Matcher.AnyOf(Matcher.CloseCombat, Matcher.Engage).OnEntityRemoved ();
		}
	}

	#endregion
}
