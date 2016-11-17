using UnityEngine;
using System.Collections;
using Entitas;

public class ProjectileCollideSystem : IReactiveSystem, IEnsureComponents {
	#region IEnsureComponents implementation

	public IMatcher ensureComponents {
		get {
			return Matcher.AllOf (Matcher.ProjectileMark, Matcher.Target).NoneOf (Matcher.ReachedEnd);
		}
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		for (int i = 0; i < entities.Count; i++) {
			var e = entities [i];

			if (e.target.e.hasViewCollider && e.target.e.viewCollider.collider.bounds.Contains(e.position.value)) {
				e.IsReachedEnd (true);
			}
		}
	}

	#endregion

	#region IReactiveSystem implementation

	public TriggerOnEvent trigger {
		get {
			return Matcher.AllOf(Matcher.Position).OnEntityAdded();
		}
	}
	#endregion
}
