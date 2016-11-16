using UnityEngine;
using System.Collections;
using Entitas;

public class ProjectileCollideSystem : IReactiveSystem, ISetPool {
	#region ISetPool implementation
	Group _groupProjectile;
	public void SetPool (Pool pool)
	{
		_groupProjectile = pool.GetGroup (Matcher.AllOf(Matcher.ProjectileMark, Matcher.Target).NoneOf(Matcher.ReachedEnd));
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if (_groupProjectile.count <= 0) {
			return;
		}

		var ens = _groupProjectile.GetEntities ();
		for (int i = 0; i < ens.Length; i++) {
			var e = ens [i];

			if (e.target.e.hasViewCollider && e.target.e.viewCollider.collider.bounds.Contains(e.position.value)) {
				e.IsReachedEnd (true);
			}


		}
	}

	#endregion

	#region IReactiveSystem implementation

	public TriggerOnEvent trigger {
		get {
			return Matcher.Tick.OnEntityAdded ();
		}
	}
	#endregion
}
