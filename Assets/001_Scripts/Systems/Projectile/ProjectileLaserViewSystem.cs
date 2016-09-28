using UnityEngine;
using System.Collections;
using Entitas;

public class ProjectileLaserViewSystem : IReactiveSystem, ISetPool {
	#region ISetPool implementation
	Pool _pool;
	Group _groupPrjLaser;
	public void SetPool (Pool pool)
	{
		_pool = pool;
		_groupPrjLaser = _pool.GetGroup (Matcher.AllOf(Matcher.ProjectileLaser, Matcher.View));
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if (_groupPrjLaser.count <= 0) {
			return;
		}
			
		var ens = _groupPrjLaser.GetEntities ();
		for (int i = 0; i < ens.Length; i++) {
			CreateLine (ens [i]);
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

	void CreateLine(Entity e){
		var line = e.view.go.GetComponent<LineRenderer> ();

		line.SetPosition (0, e.position.value);
		line.SetPosition (1, e.destination.value);

		line.material.mainTextureScale = new Vector2 (Vector3.Distance(e.position.value, e.destination.value) * 2, 1f);
	}

}
