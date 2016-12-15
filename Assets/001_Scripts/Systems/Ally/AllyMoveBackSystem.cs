﻿using UnityEngine;
using System.Collections;
using Entitas;

public class AllyMoveBackSystem : IReactiveSystem, IEnsureComponents {
	#region IEnsureComponents implementation

	public IMatcher ensureComponents {
		get {
			return Matcher.AllOf (Matcher.Ally, Matcher.Active);
		}
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		for (int i = 0; i < entities.Count; i++) {
			var ally = entities [i];

			if (ally.position.value != ally.rallyPoint.position) {
				ally.ReplaceDestination (ally.rallyPoint.position);
				if (!ally.isMovable) {
					ally.IsMovable (true);
				}
			}
		}
	}

	#endregion

	#region IReactiveSystem implementation

	public TriggerOnEvent trigger {
		get {
			return Matcher.AnyOf(Matcher.Target, Matcher.Engage).OnEntityRemoved ();
		}
	}

	#endregion
}
