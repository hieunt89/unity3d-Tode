using UnityEngine;
using System.Collections;
using Entitas;

public class InitEnemyViewSystem : IReactiveSystem, ISetPool {
	Pool _pool;
	#region ISetPool implementation

	public void SetPool (Pool pool)
	{
		_pool = pool;
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		GameObject go;
		for (int i = 0; i < entities.Count; i++) {
			var e = entities [i];
			if(e.hasEnemy){
				go = Resources.Load<GameObject> ("Enemy/" + e.enemy.eClass + "/" + e.enemy.eType);

			}
		}
	}

	#endregion

	#region IReactiveSystem implementation

	public TriggerOnEvent trigger {
		get {
			return Matcher.Activable.OnEntityRemoved ();
		}
	}

	#endregion

}
